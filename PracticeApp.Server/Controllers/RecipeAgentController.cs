using Microsoft.AspNetCore.Mvc;
using PracticeApp.Core.Agents.Recipes;
using PracticeApp.Core.Agents.Recipes.Models;

namespace PracticeApp.Server.Controllers
{
    public sealed record RunAgentRequest(string? Goal);

    [ApiController]
    [Route("api/agents/recipe")]
    public class RecipeAgentController(IRecipeAgentService service) : ControllerBase
    {
        [HttpGet("info")]
        public RecipeAgentInfo GetInfo()
        {
            return service.GetInfo();
        }

        [HttpPost("run")]
        public Task<RecipeRunResult> Run([FromBody] RunAgentRequest request, CancellationToken cancellationToken)
        {
            return service.RunAsync(request.Goal, cancellationToken);
        }

        [HttpGet("history")]
        public IReadOnlyList<Recipe> GetHistory()
        {
            return service.GetHistory();
        }

        [HttpDelete("history/{id}")]
        public IActionResult DeleteHistoryItem(string id)
        {
            return service.DeleteHistoryItem(id) ? NoContent() : NotFound();
        }

        [HttpDelete("history")]
        public IActionResult ClearHistory()
        {
            service.ClearHistory();
            return NoContent();
        }
    }
}
