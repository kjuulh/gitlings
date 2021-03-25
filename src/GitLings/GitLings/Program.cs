using System;
using System.Threading.Tasks;
using CliFx;
using GitLings.Features.Exercises;
using GitLings.Features.Exercises.Commands;
using GitLings.Features.Exercises.ExerciseCreators;
using Microsoft.Extensions.DependencyInjection;

namespace GitLings
{
    class Program
    {
        private static IServiceProvider GetServiceProvider() =>
            new ServiceCollection()
                .AddSingleton<CommittingToMasterExercise>()
                .AddSingleton<ParkChangesExercise>()
                .AddSingleton<IExerciseCreator,CommittingToMasterExercise>()
                .AddSingleton<IExerciseCreator,ParkChangesExercise>()
                .AddSingleton<IExercisesProvider, ExercisesProvider>()
                .AddSingleton<IGitProvider, GitProvider>()
                .AddTransient<ExercisesCommand>()
                .AddTransient<RestartExercisesCommand>()
                .AddTransient<ExerciseHintsCommand>()
                .AddTransient<StartExercisesCommand>()
                .AddTransient<ExerciseSolutionCommand>()
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