using Roslynator.Analyzer.Enums;

namespace Roslynator.Analyzer.Rules
{
    public sealed class FileExistsRule(MessageLevel errorLevel, string fileName, string errorMessage) : Rule<string>(errorLevel)
    {
        public override void Analyze(string directoryPath)
        {
            if (File.Exists(Path.Combine(directoryPath, fileName)))
            {
                AddError(errorMessage);
            }
        }
    }
}