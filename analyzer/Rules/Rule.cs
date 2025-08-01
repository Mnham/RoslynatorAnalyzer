using Roslynator.Analyzer.Enums;
using Roslynator.Analyzer.Models;

namespace Roslynator.Analyzer.Rules
{
    public abstract class Rule<T>(MessageLevel errorLevel)
    {
        public List<Message> Messages { get; } = [];
        public abstract void Analyze(T itemToValidate);
        public void AddException(Exception ex) => Messages.Add(new Message(MessageLevel.None, ex.ToString()));
        protected void AddError(string message) => Messages.Add(new Message(errorLevel, message));
    }
}