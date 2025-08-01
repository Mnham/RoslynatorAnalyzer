using Roslynator.Analyzer.Enums;
using Roslynator.Analyzer.Models;
using Roslynator.Analyzer.Rules;

namespace Roslynator.Analyzer.Diagnostics
{
    public sealed class RepositoryAnalyzer : AnalyzerBase
    {
        public IReadOnlyCollection<Message> Analyze(Repository repository)
        {
            AddSection($"Проверка репозитория {repository.RootPath}");

            Rule<Repository>[] rules =
            [
                new RepositoryRule(MessageLevel.Error),
                new RepositoryUnnecessaryFileRule(MessageLevel.Error, "global.json"),
                new RepositoryUnnecessaryFileRule(MessageLevel.Error, "*.ruleset"),
            ];

            foreach (string props in repository.PropsPaths)
            {
                AddError($"Необходимо удалить файл {props}");
            }

            Analyze(repository, rules);

            return GetImportantMessages();
        }
    }
}