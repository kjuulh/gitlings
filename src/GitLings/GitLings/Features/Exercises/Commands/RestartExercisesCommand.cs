using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace GitLings.Features.Exercises.Commands
{
    [Command("exercises restart",Description = "Restarts a given exercise")]
    public class RestartExercisesCommand : ICommand
    {
        [CommandParameter(0, Name = "number")]
        public int Number { get; init; } = 1;
                
        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteAsync($"Start exercise: {Number}");
        }
    }
}