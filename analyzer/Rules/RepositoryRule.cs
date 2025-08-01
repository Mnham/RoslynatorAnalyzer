using Roslynator.Analyzer.Enums;
using Roslynator.Analyzer.Models;

namespace Roslynator.Analyzer.Rules
{
    public sealed class RepositoryRule(MessageLevel errorLevel) : Rule<Repository>(errorLevel)
    {
        public override void Analyze(Repository repository)
        {
            if (repository.Solutions.Length == 0)
            {
                AddError("В репозитории отсутствуют файлы .sln");
            }

            if (repository.CsprojPaths.Length == 0)
            {
                AddError("В репозитории отсутствуют файлы .csproj");
            }
        }
    }
}