using Roslynator.Analyzer.Enums;

namespace Roslynator.Analyzer.Models
{
    public sealed record class Message(MessageLevel Level, string Text);
}