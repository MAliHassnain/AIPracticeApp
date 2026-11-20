using System.Text;
using System.Text.Json.Nodes;
using PracticeApp.Core.AI;

namespace PracticeApp.Core.Agents.Common
{
    /// <summary>
    /// Runs the tool-use loop shared by every agent:
    /// send the conversation to the model, run any tool it asks for, send the results back,
    /// and repeat until the model stops asking for tools or the turn limit is reached.
    /// </summary>
    public sealed class AgentRunner(ILlmClient llm)
    {
        private const string StopToolUse = "tool_use";
        private const string StopPauseTurn = "pause_turn";
        private const string StopMaxTurns = "max_turns";

        public async Task<AgentRunOutcome> RunAsync(AgentRunRequest request, CancellationToken cancellationToken = default)
        {
            var steps = new List<AgentStep>();
            var artifacts = new List<object>();
            var messages = new JsonArray
            {
                new JsonObject { ["role"] = "user", ["content"] = request.Goal }
            };
            var toolDefinitions = BuildToolDefinitions(request);

            for (var turn = 1; turn <= request.MaxTurns; turn++)
            {
                var response = await llm.SendAsync(messages, toolDefinitions, request.SystemPrompt, cancellationToken);
                var content = response["content"]!.AsArray();
                var stopReason = response["stop_reason"]?.GetValue<string>() ?? "unknown";

                messages.Add(new JsonObject { ["role"] = "assistant", ["content"] = content.DeepClone() });

                var isFinal = stopReason != StopToolUse && stopReason != StopPauseTurn;
                AgentStepRecorder.RecordBlocks(turn, content, steps, includeText: !isFinal);

                if (stopReason == StopPauseTurn) continue;

                if (isFinal)
                {
                    return new AgentRunOutcome(ExtractText(content), steps, stopReason, turn, artifacts);
                }

                var results = new JsonArray();
                foreach (var block in content)
                {
                    if (block?["type"]?.GetValue<string>() != "tool_use") continue;

                    var (resultBlock, artifact) = await RunToolAsync(turn, block, request.Tools, steps, cancellationToken);
                    results.Add(resultBlock);
                    if (artifact is not null) artifacts.Add(artifact);
                }
                messages.Add(new JsonObject { ["role"] = "user", ["content"] = results });
            }

            steps.Add(new AgentStep(request.MaxTurns, AgentStepKind.Info, "Stopped: reached the turn limit without a final answer."));
            return new AgentRunOutcome("", steps, StopMaxTurns, request.MaxTurns, artifacts);
        }

        private static JsonArray BuildToolDefinitions(AgentRunRequest request)
        {
            var definitions = new JsonArray();
            foreach (var serverTool in request.ServerTools)
            {
                definitions.Add(serverTool.DeepClone());
            }
            foreach (var tool in request.Tools)
            {
                definitions.Add(tool.Definition);
            }
            return definitions;
        }

        private static async Task<(JsonObject Result, object? Artifact)> RunToolAsync(
            int turn,
            JsonNode call,
            IReadOnlyList<IAgentTool> tools,
            List<AgentStep> steps,
            CancellationToken cancellationToken)
        {
            var name = call["name"]!.GetValue<string>();
            var id = call["id"]!.GetValue<string>();

            ToolResult outcome;
            var tool = tools.FirstOrDefault(t => t.Name == name);
            if (tool is null)
            {
                outcome = new ToolResult($"Unknown tool: {name}", IsError: true);
            }
            else
            {
                try
                {
                    outcome = await tool.ExecuteAsync(call["input"], cancellationToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    outcome = new ToolResult($"Tool failed: {ex.Message}", IsError: true);
                }
            }

            steps.Add(new AgentStep(turn, AgentStepKind.ToolResult, $"{name} -> {outcome.Output}"));

            var result = new JsonObject
            {
                ["type"] = "tool_result",
                ["tool_use_id"] = id,
                ["content"] = outcome.Output
            };
            if (outcome.IsError) result["is_error"] = true;

            return (result, outcome.Artifact);
        }

        private static string ExtractText(JsonArray content)
        {
            var text = new StringBuilder();
            foreach (var block in content)
            {
                if (block?["type"]?.GetValue<string>() == "text")
                {
                    text.Append(block["text"]?.GetValue<string>());
                }
            }
            return text.ToString();
        }
    }
}
