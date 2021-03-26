using System.IO;
using FluentResults;
using static FluentResults.Result;

namespace GitLings.Features.Exercises
{
    public interface IGitProvider
    {
        Result<string> CreateRepository(string path);
    }

    public class GitProvider : IGitProvider
    {
        public Result<string> CreateRepository(string path)
        {
            if (Directory.Exists($"{path}/exercise"))
                return Fail<string>("Directory already exists");
            
            return Ok(LibGit2Sharp.Repository.Init($"{path}/exercise"));
            ;
        }
    }
}