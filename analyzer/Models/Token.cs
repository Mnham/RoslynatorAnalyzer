namespace Roslynator.Analyzer.Models
{
    public record class Token(string FilePath, int Line, int Character, string Text)
    {
        public string GetMessage() => $"{FilePath} ({Line},{Character}): {Text}";
    }
}