using System.Reflection;

namespace GitLings.Domain
{
    public record Exercise
    {
        public int Number { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
    }
}