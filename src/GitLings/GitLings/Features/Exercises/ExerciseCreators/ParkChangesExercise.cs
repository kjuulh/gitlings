using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CliFx.Infrastructure;
using LibGit2Sharp;

namespace GitLings.Features.Exercises.ExerciseCreators
{
    public class ParkChangesExercise : IExerciseCreator
    {
        private const string Hint = @"
Git stash is a stack of commit somewhat disconnected from the normal git tree. As such they can be added to and removed from using a stack.

Example:
$ git add file.txt
$ git stash -m ""Some file changes""

$ git status
# Empty";

        public async Task Create(string repositoryPath, IConsole console)
        {
            console.Output.WriteLine(@"
# Park Changes

## Introduction

The goal of this exercise is to teach you how to park some changes without committing them.

## Situation

You are currently working on some code, but a colleague has notified you that there is a bug in the last commit on master.
You want to park your current work, fix the bug, and the resume what you were doing.");

            console.Output.WriteLine(@"
echo ""this file has a bug"" > file.txt

git add file.txt && git commit -m ""Add file change""

echo ""New work"" >> file.txt
");
            var file = "file.txt";
            var repo = new Repository($"{repositoryPath}/.git");
            using var fs = File.Create($"{repositoryPath}/{file}");
            await fs.WriteAsync(Encoding.UTF8.GetBytes("Oh no, this file has a bug # BUG!!!!"));
            LibGit2Sharp.Commands.Stage(repo, file);
            var author = new Signature("Kasper Juul Hermansen", "contact@kjuulh.io", DateTimeOffset.Now.AddYears(3));
            repo.Commit("Add file change", author, author);
            await fs.WriteAsync(Encoding.UTF8.GetBytes("Some very important change yes!"));
            fs.Close();
        }

        public async Task GetHint(int number, IConsole console)
        {
            switch (number)
            {
                case 1:
                    await console.Output.WriteLineAsync(Hint);
                    break;

                default:
                    await console.Output.WriteLineAsync("Sorry, but that hint doesn't exist, try a lower number");
                    break;
            }
        }

        public async Task GetSolution(IConsole console)
        {
            await console.Output.WriteLineAsync(@"
## Solution

git add file.txt
git stash -m ""This is my working changes""

$ echo ""This file doesn't have a bug"" > file.txt
$ git add file.txt 
$ git commit --amend
$ git stash pop

## Conclusion

Git stash is a smart way of parking your changes while you work on something else. Do note that sometimes a stash works, but for stuff you want to keep it is usually better to create a branch for it instead.");
        }

        public int ExerciseOrder { get; set; } = 2;
    }
}