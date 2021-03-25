using System.Threading.Tasks;
using CliFx.Infrastructure;

namespace GitLings.Features.Exercises
{
    public interface IExerciseCreator
    {
        Task Create(string repositoryPath, IConsole console);
        Task GetHint(int number, IConsole console);
        Task GetSolution(IConsole console);
        int ExerciseOrder { get; set; }
    }
}