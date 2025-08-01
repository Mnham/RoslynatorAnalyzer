using Roslynator.Analyzer.Enums;
using Roslynator.Analyzer.Models;
using Roslynator.Analyzer.Rules;

namespace Roslynator.Analyzer.Diagnostics
{
    public sealed class ComponentAnalyzer : AnalyzerBase
    {
        public IReadOnlyCollection<Message> Analyze(string rootPath)
        {
            AddSection($"Проверка компонента {rootPath}");

            Rule<string>[] rules =
            [
                new RootFileRule(MessageLevel.Error, ".editorconfig", excludeDirectory: "Migrations"),
                new RootFileRule(MessageLevel.Error, ".gitignore"),
                new RootFileRule(MessageLevel.Error, "Directory.Packages.props"),
                new EditorconfigRule(MessageLevel.Error),
                new DirectoryPackagesRule(MessageLevel.Error),
            ];

            Analyze(rootPath, rules);

            return GetImportantMessages();
        }
    }
}