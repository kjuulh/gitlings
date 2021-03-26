using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GitLings
{
    public static class FileUtilities
    {
        public static async Task WriteToFile(string path, string contents)
        {
            using var fs = File.AppendText(path);
            await fs.WriteLineAsync(contents);
            await fs.FlushAsync();
            fs.Close();
        }
    }
}