using Roslynator.Analyzer.Enums;
using Roslynator.Analyzer.Helpers;

namespace Roslynator.Analyzer.Rules
{
    public sealed class DirectoryPackagesRule(MessageLevel errorLevel) : Rule<string>(errorLevel)
    {
        public override void Analyze(string rootPath)
        {
            FileInfo file = new(Path.Combine(rootPath, "Directory.Packages.props"));
            if (!file.Exists)
            {
                return;
            }

            if (Utils.AreFilesDifferent(LocalFileHelper.DirectoryPackagesPath, file.FullName))
            {
                AddError("Directory.Packages.props необходимо заменить на стандартный");
            }
        }
    }
}