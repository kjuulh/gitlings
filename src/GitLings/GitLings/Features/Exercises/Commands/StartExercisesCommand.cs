using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using GitLings.Features.Exercises.ExerciseCreators;

namespace GitLings.Features.Exercises.Commands
{
    [Command("exercises start", Description = "Starts a given exercise")]
    public class StartExercisesCommand : ICommand
    {
        [CommandOption("path", Description = "The path from which to pull exercises from")]
        public string? Path { get; init; }

        private readonly IExercisesProvider _exercisesProvider;
        private readonly IGitProvider _gitProvider;
        private readonly CommittingToMasterExercise _committingToMasterExercise;

        [CommandParameter(0, Name = "number")] public int Number { get; init; } = 1;

        public StartExercisesCommand(IExercisesProvider exercisesProvider, IGitProvider gitProvider, CommittingToMasterExercise committingToMasterExercise)
        {
            _exercisesProvider = exercisesProvider;
            _gitProvider = gitProvider;
            _committingToMasterExercise = committingToMasterExercise;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var path = Path ?? Environment.CurrentDirectory + "/exercises";
            var exercise = await _exercisesProvider.GetExercise(path!, Number);
            if (exercise.IsFailed)
            {
                await console.Output.WriteLineAsync(exercise.Errors.First().Message);
                return;
            }

            var exercisePath = $"{exercise.Value.path}/exercise";
            if (Directory.Exists(exercisePath))
                Directory.Delete(exercisePath, true);
            
            var output = _gitProvider.CreateRepository(exercise.Value.path);
            await console.Output.WriteLineAsync($"Initialized exercise: {output}");

            await _committingToMasterExercise.Create(exercisePath, console);
        }
    }
}