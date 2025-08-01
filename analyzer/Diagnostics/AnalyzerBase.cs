using Roslynator.Analyzer.Enums;
using Roslynator.Analyzer.Models;
using Roslynator.Analyzer.Rules;

namespace Roslynator.Analyzer.Diagnostics
{
    public abstract class AnalyzerBase
    {
        private readonly List<Message> _messages = [];

        protected void AddSection(string message) => _messages.Add(new Message(MessageLevel.Section, message));
        protected void AddError(string message) => _messages.Add(new Message(MessageLevel.Error, message));

        protected void Analyze<T>(T itemToValidate, IEnumerable<Rule<T>> rules)
        {
            foreach (Rule<T> rule in rules)
            {
                try
                {
                    rule.Analyze(itemToValidate);
                }
                catch (Exception ex)
                {
                    rule.AddException(ex);
                }
            }

            _messages.AddRange(rules.SelectMany(x => x.Messages));
        }

        protected IReadOnlyCollection<Message> GetImportantMessages()
        {
            if (_messages.Any(x => x.Level is MessageLevel.Warning or MessageLevel.Error))
            {
                return _messages;
            }

            return Array.Empty<Message>();
        }
    }
}