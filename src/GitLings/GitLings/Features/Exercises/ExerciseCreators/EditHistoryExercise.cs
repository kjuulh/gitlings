using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliFx.Infrastructure;
using LibGit2Sharp;

namespace GitLings.Features.Exercises.ExerciseCreators
{
    public class EditHistoryExercise : IExerciseCreator
    {
        public async Task Create(string repositoryPath, IConsole console)
        {
            var author = new Signature("Kasper Juul Hermansen", "contact@kjuulh.io", DateTimeOffset.Now.AddYears(3));
            var file = "file.txt";

            await WriteText(console);

            var repo = new Repository($"{repositoryPath}/.git");
            using var fs = File.CreateText($"{repositoryPath}/{file}");
            
            await fs.WriteLineAsync("Some change");
            await fs.FlushAsync();
            LibGit2Sharp.Commands.Stage(repo, file);
            repo.Commit("Add file change", author, author);

            await fs.WriteLineAsync("Some change");
            await fs.FlushAsync();
            LibGit2Sharp.Commands.Stage(repo, file);
            repo.Commit("Add file change", author, author);

            await fs.WriteLineAsync("Some very secret secret: #!@#!@#@!#!@#!@#!@$!@$");
            await fs.FlushAsync();
            LibGit2Sharp.Commands.Stage(repo, file);
            repo.Commit("some innocent commit", author, author);

            await fs.WriteLineAsync("Some change");
            await fs.FlushAsync();
            LibGit2Sharp.Commands.Stage(repo, file);
            repo.Commit("Add file change", author, author);

            await fs.WriteLineAsync("Some change");
            await fs.FlushAsync();
            LibGit2Sharp.Commands.Stage(repo, file);
            repo.Commit("Add file change", author, author);
            
            fs.Close();
        }

        private static async Task WriteText(IConsole console)
        {
            await console.Output.WriteLineAsync(@"
# Edit History

## Introduction

Sometimes it is useful to edit the history to either remove a commit or change a file somewhere in the commit history.

Do note that this is a destructive maneuver, as such it can cause irrecoverable damage to a repository.

## Situation

You have been working on your branch for a while, and however, suddenly, your tech lead notifies you that you have committed a secret to the git repository.

You are tasked with editing the commit to remove it.

## Code

echo 'some changes' > file.txt
git add file.txt && git commit -m 'Some work'

echo 'some additional changes' >> file.txt
git commit -am 'Some more work'

echo 'Some very secret secret: %LA!092*3912u3lslafs' >> file.txt
git commit -am 'Some more innocent work'

echo 'some additional changes' >> file.txt
git commit -am 'Some more work'

echo 'some additional changes' >> file.txt
git commit -am 'Some more work'

## Goal

cat file.txt # Contains no secret
");
        }


        public async Task GetHint(int number, IConsole console)
        {
            if (number == 1)
                await console.Output.WriteLineAsync(@"
Rebase is the swiss army knife of git, a lot of tools use it under the hood, but today, we are going to use it directly.

With rebase you can edit the history, from a chosen point, until the current HEAD. `-i` btw. is for interactive.

git rebase -i COMMIT");
            else
                await console.Output.WriteLineAsync(@"Hint not found, try a lower number");
        }

        public async Task GetSolution(IConsole console)
        {
            await console.Output.WriteLineAsync(@"
git rebase -i COMMIT_SHA
# Remove the commit
# Save and exit");
        }

        public int ExerciseOrder { get; set; } = 3;
    }
}