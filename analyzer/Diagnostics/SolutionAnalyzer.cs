using Roslynator.Analyzer.Enums;
using Roslynator.Analyzer.Models;
using Roslynator.Analyzer.Rules;

namespace Roslynator.Analyzer.Diagnostics
{
    public sealed class SolutionAnalyzer : AnalyzerBase
    {
        public IReadOnlyCollection<Message> Analyze(SolutionData solution)
        {
            AddSection($"Проверка решения {solution.Sln}");

            Rule<SolutionData>[] rules =
            [
                new SolutionRule(MessageLevel.Error),
            ];

            Analyze(solution, rules);

            return GetImportantMessages();
        }
    }
}