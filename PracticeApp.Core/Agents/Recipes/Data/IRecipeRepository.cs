using PracticeApp.Core.Agents.Recipes.Models;

namespace PracticeApp.Core.Agents.Recipes.Data
{
    public interface IRecipeRepository
    {
        IReadOnlyList<Recipe> GetAll();
        IReadOnlyList<string> GetTitles();
        Recipe Add(Recipe recipe);
        bool Delete(string id);
        void Clear();
    }
}
