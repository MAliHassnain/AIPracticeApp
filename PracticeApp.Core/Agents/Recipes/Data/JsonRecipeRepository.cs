using System.Text.Json;
using PracticeApp.Core.Agents.Recipes.Models;

namespace PracticeApp.Core.Agents.Recipes.Data
{
    /// <summary>Stores recipes in a JSON file. Thread-safe because web requests can overlap.</summary>
    public sealed class JsonRecipeRepository : IRecipeRepository
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

        private readonly object _lock = new();
        private readonly string _filePath;

        public JsonRecipeRepository(string filePath)
        {
            _filePath = filePath;
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
        }

        public IReadOnlyList<Recipe> GetAll()
        {
            lock (_lock) { return Load(); }
        }

        public IReadOnlyList<string> GetTitles()
        {
            lock (_lock) { return Load().Select(r => r.Title).ToList(); }
        }

        public Recipe Add(Recipe recipe)
        {
            lock (_lock)
            {
                var all = Load();
                all.Add(recipe);
                Save(all);
                return recipe;
            }
        }

        public bool Delete(string id)
        {
            lock (_lock)
            {
                var all = Load();
                var removed = all.RemoveAll(r => r.Id == id) > 0;
                if (removed) Save(all);
                return removed;
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                if (File.Exists(_filePath)) File.Delete(_filePath);
            }
        }

        private List<Recipe> Load()
        {
            if (!File.Exists(_filePath)) return new List<Recipe>();
            try
            {
                return JsonSerializer.Deserialize<List<Recipe>>(File.ReadAllText(_filePath), JsonOptions)
                    ?? new List<Recipe>();
            }
            catch (JsonException)
            {
                return new List<Recipe>();
            }
        }

        private void Save(List<Recipe> recipes) =>
            File.WriteAllText(_filePath, JsonSerializer.Serialize(recipes, JsonOptions));
    }
}
