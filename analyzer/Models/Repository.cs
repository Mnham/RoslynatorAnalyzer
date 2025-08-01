using Microsoft.Build.Construction;

namespace Roslynator.Analyzer.Models
{
    public sealed class Repository
    {
        public required string RootPath { get; init; }
        public required SolutionData[] Solutions { get; init; }
        public required string[] CsprojPaths { get; init; }
        public required string[] PropsPaths { get; init; }
        public required string CsprojCommonPath { get; init; }

        public static Repository Parse(string path)
        {
            string[] csprojPaths = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories);

            return new Repository
            {
                RootPath = path,
                CsprojPaths = csprojPaths,
                CsprojCommonPath = GetCommonPath(csprojPaths),

                Solutions = Directory
                    .GetFiles(path, "*.sln", SearchOption.AllDirectories)
                    .Where(sln => !Path.GetDirectoryName(sln)!.EndsWith("development"))
                    .Select(sln =>
                    {
                        string directory = Path.GetDirectoryName(sln)!;

                        return new SolutionData
                        {
                            Sln = sln,
                            Directory = directory,
                            PreviousDirectory = GetPreviousDirectory(sln),
                            File = SolutionFile.Parse(sln),
                            HasProtos = Directory.EnumerateFiles(directory, "*.proto", SearchOption.AllDirectories).Any(),
                        };
                    })
                    .ToArray(),

                PropsPaths = Directory
                    .GetFiles(path, "*.props", SearchOption.AllDirectories)
                    .Where(props => !(props.EndsWith(".g.props", StringComparison.OrdinalIgnoreCase)
                        || props.EndsWith("Directory.Packages.props", StringComparison.OrdinalIgnoreCase)
                        || Path.GetFileName(props).StartsWith("msbuild.", StringComparison.OrdinalIgnoreCase)))
                    .ToArray(),
            };
        }

        private static string GetCommonPath(string[] paths)
        {
            if (paths.Length == 0)
            {
                return "";
            }

            if (paths.Length == 1)
            {
                return Path.GetDirectoryName(paths[0])!;
            }

            string[][] segments = paths
                .Select(x => x.Split(Path.DirectorySeparatorChar))
                .ToArray();

            int min = segments.Min(x => x.Length);
            List<string> commonSegments = [];
            for (int i = 0; i < min; i++)
            {
                string current = segments[0][i];
                if (segments.All(x => x[i] == current))
                {
                    commonSegments.Add(current);
                }
                else
                {
                    break;
                }
            }

            string result = Path.Combine(commonSegments.ToArray());
            if (!Path.IsPathRooted(result))
            {
                result = Path.DirectorySeparatorChar + result;
            }

            return result;
        }

        private static string? GetPreviousDirectory(string sln)
        {
            string directory = Path.GetDirectoryName(sln)!;
            string[] parts = directory.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                return null;
            }

            return parts[parts.Length - 2];
        }
    }
}