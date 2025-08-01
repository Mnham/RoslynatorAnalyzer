using Roslynator.Analyzer.Enums;
using Roslynator.Analyzer.Helpers;

namespace Roslynator.Analyzer.Rules
{
    public sealed class EditorconfigRule(MessageLevel errorLevel) : Rule<string>(errorLevel)
    {
        public override void Analyze(string rootPath)
        {
            FileInfo file = new(Path.Combine(rootPath, ".editorconfig"));
            if (!file.Exists)
            {
                return;
            }

            if (Utils.AreFilesDifferent(LocalFileHelper.MainEditorconfigPath, file.FullName))
            {
                AddError(".editorconfig необходимо заменить на стандартный");
            }
        }
    }
}