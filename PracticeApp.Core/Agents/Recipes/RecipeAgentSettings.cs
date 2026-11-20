namespace PracticeApp.Core.Agents.Recipes
{
    /// <summary>Behaviour of the recipe agent, bound from the "Agents:Recipe" section of appsettings.json.</summary>
    public sealed class RecipeAgentSettings
    {
        public const string SectionName = "Agents:Recipe";

        public int MaxTurns { get; set; } = 8;
        public string DefaultGoal { get; set; } = "Pick tonight's desi dinner.";
        public string SystemPrompt { get; set; } = "You are a desi dinner agent.";

        public string WebSearchToolType { get; set; } = "web_search_20250305";
        public int WebSearchMaxUses { get; set; } = 3;

        /// <summary>Path of the history file, relative to the server's content root.</summary>
        public string HistoryFile { get; set; } = "App_Data/recipes.json";
    }
}
