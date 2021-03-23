using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace GitLings.Features.Exercises.Commands
{
    [Command("exercises start",Description = "Starts a given exercise")]
    public class StartExercisesCommand : ICommand
    {
        [CommandParameter(0, Name = "number")]
        public int Number { get; init; } = 1;
            
        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteAsync($"Start exercise: {Number}");
        }
    }
}