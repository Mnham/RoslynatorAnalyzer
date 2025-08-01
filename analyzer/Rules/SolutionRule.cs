using Roslynator.Analyzer.Enums;
using Roslynator.Analyzer.Models;

namespace Roslynator.Analyzer.Rules
{
    public sealed class SolutionRule(MessageLevel errorLevel) : Rule<SolutionData>(errorLevel)
    {
        public override void Analyze(SolutionData solution)
        {
            string[] csprojFiles = Directory.GetFiles(solution.Directory, "*.csproj", SearchOption.AllDirectories);
            foreach (string path in csprojFiles)
            {
                if (!solution.File.ProjectsInOrder.Any(x => x.AbsolutePath == path))
                {
                    AddError($"Проект {path} необходимо добавить в решение {solution.Sln}");
                }
            }
        }
    }
}