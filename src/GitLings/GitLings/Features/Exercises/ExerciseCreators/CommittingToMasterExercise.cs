using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CliFx.Infrastructure;
using LibGit2Sharp;

namespace GitLings.Features.Exercises.ExerciseCreators
{
    public class CommittingToMasterExercise : IExerciseCreator
    {
        private const string Hint1 = @"
Branches are basically just pointers, as such when created, they will be placed on the current HEAD. This means that creating a branch, will have the current state.
git branch some-new-branch

git log --graph 
# Should show the name of the new branch pointing to master";

        private const string Hint2 = @"
Commits can be removed by jumping to an earlier commit. This is what is needed to be done to reset commits on master
$ git reset --hard HEAD^ # Go back one from HEAD
$ git log 
# Commit should now be removed";

        public int ExerciseOrder { get; set; } = 1;

        public async Task Create(string repositoryPath, IConsole console)
        {
            var file = "file.txt";

            WriteText(console, file);

            var repo = new Repository($"{repositoryPath}/.git");
            await WriteToFile($"{repositoryPath}/{file}", "");
            LibGit2Sharp.Commands.Stage(repo, file);
            var author = new Signature("Kasper Juul Hermansen", "contact@kjuulh.io", DateTimeOffset.Now.AddYears(3));
            repo.Commit("Add file change", author, author);
            await WriteToFile($"{repositoryPath}/{file}", "Some change");
            LibGit2Sharp.Commands.Stage(repo, file);
            repo.Commit("Oh no, move this commit!", author, author);
        }

        private async Task WriteToFile(string path, string contents)
        {
            using var fs = File.OpenWrite(path);
            await fs.WriteAsync(Encoding.UTF8.GetBytes(contents));
            await fs.FlushAsync();
            fs.Close();
        }

        private static void WriteText(IConsole console, string file)
        {
            console.Output.WriteLine($"$ touch {file}");
            console.Output.WriteLine($"$ git add {file}");
            console.Output.WriteLine($"$ git commit -m \"Add file change\"");
            console.Output.WriteLine($"$ echo \"Some file change\" >> {file}");
            console.Output.WriteLine($"$ git add {file}");
            console.Output.WriteLine($"$ git commit -m \"Oh no, move this commit!\"");
            console.Output.WriteLine($"# Type 'git log' to see the result");
            console.Output.WriteLine(@"
The goal of this exercise is to move the first commit onto another branch, and move the master one commit back
 $ git log feature/some-file-change  
 commit (feature/some-file-change)
     Oh no, move this commit!
 
 commit (HEAD -> master)
     Add file change ");
        }

        public async Task GetHint(int number, IConsole console)
        {
            switch (number)
            {
                case 1:
                    await console.Output.WriteLineAsync(Hint1);
                    break;
                case 2:
                    await console.Output.WriteLineAsync(Hint2);
                    break;
                default:
                    await console.Output.WriteLineAsync("Sorry, but that hint doesn't exist, try a lower number");
                    break;
            }
        }

        public async Task GetSolution(IConsole console)
        {
            await console.Output.WriteLineAsync(@"
git branch feature/some-file-change
git reset --hard HEAD^
git log feature/some-file-change");
        }
    }
}