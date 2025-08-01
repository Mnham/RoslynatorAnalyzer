using Roslynator.Analyzer.Enums;
using Roslynator.Analyzer.Models;

namespace Roslynator.Analyzer.Rules
{
    public sealed class RepositoryUnnecessaryFileRule(MessageLevel errorLevel, string searchPattern) : Rule<Repository>(errorLevel)
    {
        public override void Analyze(Repository repository)
        {
            foreach (FileInfo item in GetFiles(repository.RootPath, repository.CsprojCommonPath, searchPattern))
            {
                AddError($"Необходимо удалить файл {item}");
            }
        }

        private static IReadOnlyCollection<FileInfo> GetFiles(string rootPath, string commonPath, string searchPattern)
        {
            if (string.IsNullOrEmpty(commonPath))
            {
                return Array.Empty<FileInfo>();
            }

            DirectoryInfo root = new(rootPath);
            DirectoryInfo common = new(commonPath);

            if (root.FullName == common.FullName)
            {
                return root.GetFiles(searchPattern, SearchOption.AllDirectories);
            }

            List<FileInfo> result = new(common.GetFiles(searchPattern, SearchOption.AllDirectories));

            for (DirectoryInfo? parent = common.Parent;
                parent is not null;
                parent = parent.Parent)
            {
                result.AddRange(parent.GetFiles(searchPattern, SearchOption.TopDirectoryOnly));

                if (parent.FullName == root.FullName)
                {
                    break;
                }
            }

            return result;
        }
    }
}