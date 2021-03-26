using System;
using System.IO;
using System.Threading.Tasks;
using CliFx.Infrastructure;
using LibGit2Sharp;

namespace GitLings.Features.Exercises.ExerciseCreators
{
    public class UpstreamExercise : IExerciseCreator
    {
        public async Task Create(string repositoryPath, IConsole console)
        {
            var author = new Signature("Kasper Juul Hermansen", "contact@kjuulh.io", DateTimeOffset.Now.AddYears(3));
            await WriteText(console);

            var repo = new Repository($"{repositoryPath}/.git");

            repo.Network.Remotes.Add("upstream", "https://github.com/kjuulh/gitlings.git");
            LibGit2Sharp.Commands.Fetch(repo, "upstream", new []{"main"}, new FetchOptions(), "Fetching upstream main");
            LibGit2Sharp.Commands.Checkout(repo, "2fcc1b9135e6c5c613ac2879332ff7aae0293090");
            
            await FileUtilities.WriteToFile($"{repositoryPath}/README.md", "Some change");
            LibGit2Sharp.Commands.Stage(repo, "README.md");
            repo.Commit("Add file change", author, author);
        }

        private static async Task WriteText(IConsole console)
        {
            await console.Output.WriteLineAsync(@"
# Upstream

## Introduction

Some times you want to do work on a project, but don't want to contribute directly to it.

## Situation

You have been tasked with extending an open source project, and you don't want to just vendor it. As such you want to append your changes to it, and then afterwards update when new commits come in.

## Code

git remote add upstream https://github.com/kjuulh/gitlings.git
git fetch upstream
git checkout 2fcc1b9135e6c5c613ac2879332ff7aae0293090

echo 'some changes to readme' >> README.md
git commit -am 'lets create some changes'

# Now I want to update to the newest commit, but I can't
git pull # ERROR

## Goal

$ git log
commit 43777169f6981df977e212bcf61a197cf1104d2b (HEAD)
Author: Kasper Juul Hermansen <contact@kjuulh.io>
Date:   Thu Mar 25 21:40:37 2021 +0100

    Lets create some changes

commit be0f702bc623ab379c822b025bf7592d0daa7eb6 (upstream/main)

");
        }


        public async Task GetHint(int number, IConsole console)
        {
            if (number == 1)
                await console.Output.WriteLineAsync(@"
Rebase is your friend
");
            else
                await console.Output.WriteLineAsync(@"Hint not found, try a lower number");
        }

        public async Task GetSolution(IConsole console)
        {
            await console.Output.WriteLineAsync(@"
git pull --rebase

git log

## Conclusion

Again rebase to the rescue. Using pull with strategy rebase, we are able to append our changes on top.
");
        }

        public int ExerciseOrder { get; set; } = 4;
    }
}