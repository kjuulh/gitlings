using System.Threading.Tasks;
using CliFx.Infrastructure;

namespace GitLings.Features.Exercises
{
    public interface IExerciseCreator
    {
        Task Create(string repositoryPath, IConsole console);
    }
}