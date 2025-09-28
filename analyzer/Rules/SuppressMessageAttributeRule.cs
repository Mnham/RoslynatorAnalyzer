using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Roslynator.Analyzer.Enums;
using Roslynator.Analyzer.Models;

namespace Roslynator.Analyzer.Rules
{
    public sealed class SuppressMessageAttributeRule(MessageLevel errorLevel) : Rule<IReadOnlyCollection<SyntaxNode>>(errorLevel)
    {
        public override void Analyze(IReadOnlyCollection<SyntaxNode> nodes)
        {
            List<Token> tokens = GetTokens(nodes);
            if (tokens.Count > 0)
            {
                AddError("Обнаружены атрибуты подавления:");
            }

            foreach (Token token in tokens)
            {
                AddError(token.GetMessage());
            }
        }

        private static List<Token> GetTokens(IReadOnlyCollection<SyntaxNode> nodes)
        {
            List<Token> result = [];
            foreach (SyntaxNode node in nodes)
            {
                IEnumerable<Token> items = node
                    .DescendantNodes()
                    .OfType<AttributeSyntax>()
                    .Where(x => x.Name.ToString().Contains("SuppressMessage"))
                    .Select(x =>
                    {
                        LinePosition position = x.SyntaxTree.GetLineSpan(x.Span).StartLinePosition;

                        return new Token
                        (
                            FilePath: x.SyntaxTree.FilePath,
                            Line: position.Line + 1,
                            Character: position.Character + 1,
                            Text: x.ToString()
                        );
                    });

                result.AddRange(items);
            }

            return result;
        }
    }
}