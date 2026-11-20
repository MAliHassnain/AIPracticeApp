using PracticeApp.Core.AI;
using PracticeApp.Core.Agents.Common;
using PracticeApp.Core.Agents.Recipes;
using PracticeApp.Core.Agents.Recipes.Data;
using PracticeApp.Core.Agents.Recipes.Tools;

namespace PracticeApp.Server.Extensions
{
    public static class AiServiceCollectionExtensions
    {
        /// <summary>Registers the language model client and the shared agent runner.</summary>
        public static IServiceCollection AddLlm(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection(LlmSettings.SectionName).Get<LlmSettings>() ?? new LlmSettings();
            services.AddSingleton(settings);

            services.AddHttpClient<ILlmClient, ClaudeClient>(client =>
                client.Timeout = TimeSpan.FromMinutes(settings.RequestTimeoutMinutes));

            services.AddTransient<AgentRunner>();
            return services;
        }

        /// <summary>Registers the recipe agent, its tools, history storage and business service.</summary>
        public static IServiceCollection AddRecipeAgent(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            var settings = configuration.GetSection(RecipeAgentSettings.SectionName).Get<RecipeAgentSettings>()
                ?? new RecipeAgentSettings();
            services.AddSingleton(settings);

            var historyPath = Path.Combine(environment.ContentRootPath, settings.HistoryFile);
            services.AddSingleton<IRecipeRepository>(new JsonRecipeRepository(historyPath));

            services.AddTransient<GetPastRecipesTool>();
            services.AddTransient<SaveRecipeTool>();
            services.AddTransient<RecipeAgent>();
            services.AddTransient<IRecipeAgentService, RecipeAgentService>();
            return services;
        }
    }
}
