using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace GitLings.Features.Exercises.Commands
{
    [Command("exercises start", Description = "Starts a given exercise")]
    public class StartExercisesCommand : ICommand
    {
        [CommandOption("path", Description = "The path from which to pull exercises from")]
        public string? Path { get; init; }

        private readonly IExercisesProvider _exercisesProvider;
        private readonly IGitProvider _gitProvider;
        private readonly IEnumerable<IExerciseCreator> _exerciseCreators;

        [CommandParameter(0, Name = "number")] public int Number { get; init; } = 1;

        public StartExercisesCommand(IExercisesProvider exercisesProvider, IGitProvider gitProvider, IEnumerable<IExerciseCreator> exerciseCreators)
        {
            _exercisesProvider = exercisesProvider;
            _gitProvider = gitProvider;
            _exerciseCreators = exerciseCreators;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var exercise = _exerciseCreators.FirstOrDefault(e => e.ExerciseOrder == Number);
            if (exercise is null)
            {
                console.Output.WriteLine("Exercise could not be found by number");
                return;
            }
            
            var path = Path ?? Environment.CurrentDirectory + "/exercises";
            var exerciseLocation = await _exercisesProvider.GetExercise(path!, Number);
            if (exerciseLocation.IsFailed)
            {
                await console.Output.WriteLineAsync(exerciseLocation.Errors.First().Message);
                return;
            }

            var exerciseLocationTuple = exerciseLocation.Value;
            var exercisePath = $"{exerciseLocationTuple.path}/exercise";
            
            var output = _gitProvider.CreateRepository(exerciseLocationTuple.path);
            if (output.IsFailed)
            {
                await console.Output.WriteLineAsync(output.Errors.First().Message);
                return;
            }
            await console.Output.WriteLineAsync($"Initialized exercise: {output.Value}");
            
            await exercise.Create(exercisePath, console);
        }
    }
}