using System;
using System.IO;
using System.Threading.Tasks;
using CliFx.Infrastructure;
using LibGit2Sharp;

namespace GitLings.Features.Exercises.ExerciseCreators
{
    public class CommittingToMasterExercise : IExerciseCreator
    {
        public Task Create(string repositoryPath, IConsole console)
        {
            var file = "file.txt";
            File.Create($"{repositoryPath}/{file}");
            console.Output.WriteLine($"Created: {file}");
            
            var repo = new Repository($"{repositoryPath}/.git");

            LibGit2Sharp.Commands.Stage(repo, file);
            console.Output.WriteLine($"Staging: {file}");

            var author = new Signature("Kasper Juul Hermansen", "contact@kjuulh.io", DateTimeOffset.Now.AddYears(3));
            repo.Commit("Add file change", author, author);
            console.Output.WriteLine($"Committing changes: {file}");
            
            return Task.CompletedTask;
        }
    }
}