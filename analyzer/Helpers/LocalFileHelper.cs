using System.Reflection;

namespace Roslynator.Analyzer.Helpers
{
    public static class LocalFileHelper
    {
        public static string MainEditorconfigPath { get; }
        public static string DirectoryPackagesPath { get; }
        public static string ProtolintPath { get; }

        static LocalFileHelper()
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;

            MainEditorconfigPath = Path.Combine(path, "config", ".editorconfig");
            ProtolintPath = Path.Combine(path, "config", ".protolint.yaml");
            DirectoryPackagesPath = Path.Combine(path, "config", "Directory.Packages.props");
        }
    }
}