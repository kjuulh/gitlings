using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace GitLings.Features.Exercises.Commands
{
    [Command("exercises list", Description = "Lists all exercises in the exercises folder")]
    public class ListExercisesCommand : ICommand
    {
        [CommandOption("path", Description = "The path from which to pull exercises from")]
        public string? Path { get; init; }

        [CommandOption("full", Description = "Display all information about the exercises")]
        public bool Full { get; set; } = false;

        private readonly IExercisesProvider _exercisesProvider;

        public ListExercisesCommand(IExercisesProvider exercisesProvider)
        {
            _exercisesProvider = exercisesProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var path = Path ?? Environment.CurrentDirectory + "/exercises";
            var exerciseResult = await _exercisesProvider.GetExercises($"{path}");
            await console.Output.WriteLineAsync($"Getting exercises from path: {path}");
            if (exerciseResult.IsFailed)
            {
                foreach (var error in exerciseResult.Errors)
                {
                    await console.Output.WriteLineAsync(error.Message);
                }

                return;
            }

            await console.Output.WriteLineAsync("Exercises");
            foreach (var exercise in exerciseResult.Value.Select(e => e.exercise))
            {
                await console.Output.WriteLineAsync($"{exercise.Number} - {exercise.Name}");

                if (Full)
                {
                    var exerciseDescription = SplitText(exercise.Description);
                    foreach (var line in exerciseDescription)
                    {
                        if (line.StartsWith(" "))
                            await console.Output.WriteLineAsync($" {line}");
                        else
                            await console.Output.WriteLineAsync($"  {line}");
                    }
                await console.Output.WriteLineAsync();
                }
            }
        }

        private static IEnumerable<string> SplitText(string text) =>
            text
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .SelectMany(SplitParagraph);

        private static IEnumerable<string> SplitParagraph(string text)
        {
            double partSize = 78;
            int k = 0;
            return text
                .ToLookup(c => Math.Floor(k++ / partSize))
                .Select(e => new string(e.ToArray()))
                .Where(e => e.Any());
        }
    }
}