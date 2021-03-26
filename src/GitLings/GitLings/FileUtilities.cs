using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GitLings
{
    public static class FileUtilities
    {
        public static async Task WriteToFile(string path, string contents)
        {
            using var fs = File.OpenWrite(path);
            await fs.WriteAsync(Encoding.UTF8.GetBytes(contents));
            await fs.FlushAsync();
            fs.Close();
        }
    }
}