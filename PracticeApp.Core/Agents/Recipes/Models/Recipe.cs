namespace PracticeApp.Core.Agents.Recipes.Models
{
    public sealed class Ingredient
    {
        public string Item { get; set; } = "";
        public string Quantity { get; set; } = "";
    }

    public sealed class Recipe
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int? Servings { get; set; }
        public string TotalTime { get; set; } = "";
        public List<Ingredient> Ingredients { get; set; } = new();
        public List<string> Method { get; set; } = new();
        public string SourceUrl { get; set; } = "";
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
