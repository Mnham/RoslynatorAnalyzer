using Microsoft.Build.Construction;
using Roslynator.Analyzer.Enums;

namespace Roslynator.Analyzer.Rules
{
    public sealed class UnnecessaryPropertyRule(MessageLevel errorLevel, string searchPattern) : Rule<ProjectRootElement>(errorLevel)
    {
        public override void Analyze(ProjectRootElement project)
        {
            ProjectPropertyElement[] properties = project.Properties
                .Where(x => x.Name.StartsWith(searchPattern, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            foreach (ProjectPropertyElement item in properties)
            {
                AddError($"{item.Location.LocationString}: Необходимо удалить параметр <{item.Name}>{item.Value}</{item.Name}>");
            }
        }
    }
}