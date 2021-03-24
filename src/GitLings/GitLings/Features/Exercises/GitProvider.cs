namespace GitLings.Features.Exercises
{
    public interface IGitProvider
    {
        string CreateRepository(string path);
    }

    public class GitProvider : IGitProvider
    {
        public string CreateRepository(string path) => 
            LibGit2Sharp.Repository.Init($"{path}/exercise");
    }
}