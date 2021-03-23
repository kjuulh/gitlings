using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using GitLings.Domain;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static FluentResults.Result;

namespace GitLings.Features.Exercises
{
    public interface IExercisesProvider
    {
        Task<Result<IEnumerable<Exercise>>> GetExercises(string path);
    }

    public class ExercisesProvider : IExercisesProvider
    {
        public async Task<Result<IEnumerable<Exercise>>> GetExercises(string path)
        {
            if (!Directory.Exists(path))
                return Fail("Path doesn't exist");

            var exercisesDirectories = Directory
                .GetDirectories(path)
                .Where(d => File.Exists($"{d}/.exerciseManifest.yml"))
                .Select(ed => File.ReadAllTextAsync($"{ed}/.exerciseManifest.yml"));
            var exercisesFiles = await Task.WhenAll(exercisesDirectories);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            var exercises = exercisesFiles
                .Select(e => deserializer
                    .Deserialize<Exercise>(e));

            if (!exercises.Any())
                return Fail("No exercises found");
            
            return Ok(exercises);
        }
    }
}