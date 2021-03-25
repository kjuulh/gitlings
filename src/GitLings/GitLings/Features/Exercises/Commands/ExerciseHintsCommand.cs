using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace GitLings.Features.Exercises.Commands
{
    [Command("exercises hints")]
    public class ExerciseHintsCommand : ICommand
    {
        private readonly IEnumerable<IExerciseCreator> _exerciseCreators;

        [CommandParameter(0, Name = "Exercise number")]
        public int ExerciseNumber { get; init; }

        [CommandParameter(
            1,
            Name = "Hint number",
            Description = "The hint given for the exercise, there may be multiple, but is by set to default 1")]
        public int HintNumber { get; init; } = 1;

        public ExerciseHintsCommand(IEnumerable<IExerciseCreator> exerciseCreators)
        {
            _exerciseCreators = exerciseCreators;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var exercise = _exerciseCreators.FirstOrDefault(e => e.ExerciseOrder == ExerciseNumber);
            if (exercise is null)
            {
                console.Output.WriteLine("No exercise found for number");
                return;
            }

            await exercise.GetHint(HintNumber, console);
        }
    }
}

      