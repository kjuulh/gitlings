using System;
using System.Threading.Tasks;
using CliFx;
using GitLings.Features.Exercises;
using GitLings.Features.Exercises.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace GitLings
{
    class Program
    {
        private static IServiceProvider GetServiceProvider() =>
            new ServiceCollection()
                .AddSingleton<IExercisesProvider, ExercisesProvider>()
                .AddTransient<ExercisesCommand>()
                .AddTransient<RestartExercisesCommand>()
                .AddTransient<StartExercisesCommand>()
                .AddTransient<ListExercisesCommand>()
                .BuildServiceProvider();

        static async Task<int> Main(string[] args) =>
            await new CliApplicationBuilder()
                .SetDescription("GitLings application for managing exercises, hints and solutions")
                .AddCommandsFromThisAssembly()
                .UseTypeActivator(GetServiceProvider().GetService)
                .Build()
                .RunAsync();
    }
}