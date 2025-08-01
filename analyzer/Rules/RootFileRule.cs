using Roslynator.Analyzer.Enums;

namespace Roslynator.Analyzer.Rules
{
    public sealed class RootFileRule(MessageLevel errorLevel, string fileName, string? excludeDirectory = null) : Rule<string>(errorLevel)
    {
        public override void Analyze(string rootPath)
        {
            FileInfo file = new(Path.Combine(rootPath, fileName));
            if (!file.Exists)
            {
                AddError($"Необходимо добавить файл {file}");
            }

            IEnumerable<FileInfo> items = new DirectoryInfo(rootPath)
                .GetFiles(fileName, SearchOption.AllDirectories)
                .Where(x => x.DirectoryName != rootPath);

            if (excludeDirectory is not null)
            {
                items = items.Where(x => !x.DirectoryName!.EndsWith(excludeDirectory));
            }

            FileInfo[] files = items.ToArray();
            foreach (FileInfo item in files)
            {
                AddError($"Необходимо удалить файл {item}");
            }
        }
    }
}