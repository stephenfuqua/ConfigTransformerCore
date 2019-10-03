using System.IO;

namespace ConfigTransformerCore
{
    public interface IFilesystemAdapter
    {
        bool FileExist(string path);
    }

    public class FilesystemAdapter : IFilesystemAdapter
    {
        public bool FileExist(string path) => File.Exists(path);
    }
}
