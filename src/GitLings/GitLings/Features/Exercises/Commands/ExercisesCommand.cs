using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;

namespace GitLings.Features.Exercises.Commands
{
    [Command("exercises",Description = "Control exercises for the gitlings repository")]
    public class ExercisesCommand : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            throw new CommandException("Please type a command", showHelp: true);
        }
    }
}