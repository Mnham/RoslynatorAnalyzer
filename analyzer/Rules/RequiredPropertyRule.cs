using Microsoft.Build.Construction;
using Roslynator.Analyzer.Enums;

namespace Roslynator.Analyzer.Rules
{
    public sealed class RequiredPropertyRule(MessageLevel errorLevel, string name, string value) : Rule<ProjectRootElement>(errorLevel)
    {
        public override void Analyze(ProjectRootElement project)
        {
            string property = $"<{name}>{value}</{name}>";

            if (project.PropertyGroups.Count == 0)
            {
                AddError($"Необходимо создать секцию <PropertyGroup> и добавить параметр {property}");

                return;
            }

            ProjectPropertyElement[] properties = project.PropertyGroups.First().Properties
                .Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                    && x.Value.Equals(value, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (properties.Length == 0)
            {
                AddError($"Необходимо добавить параметр {property} в первую секцию <PropertyGroup>");
            }

            properties = properties.Skip(1)
                .Concat(project.PropertyGroups
                    .Skip(1)
                    .SelectMany(x => x.Properties)
                    .Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                        && x.Value.Equals(value, StringComparison.OrdinalIgnoreCase)))
                .ToArray();

            foreach (ProjectPropertyElement item in properties)
            {
                AddError($"{item.Location.LocationString}: Необходимо удалить параметр {property}");
            }
        }
    }
}