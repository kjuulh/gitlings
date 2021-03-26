using System.IO;

namespace GitLings.Features.Exercises
{
    public interface IGitProvider
    {
        string CreateRepository(string path);
    }

    public class GitProvider : IGitProvider
    {
        public string CreateRepository(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            return LibGit2Sharp.Repository.Init($"{path}/exercise");
        }
    }
}