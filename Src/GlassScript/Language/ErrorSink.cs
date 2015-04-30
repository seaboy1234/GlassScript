using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language
{
    public sealed class ErrorEntry
    {
        public string[] Lines { get; }

        public string Message { get; }

        public Severity Severity { get; }

        public SourceSpan Span { get; }

        public ErrorEntry(string message, string[] lines, Severity severity, SourceSpan span)
        {
            Message = message;
            Lines = lines;
            Span = span;
            Severity = severity;
        }
    }

    public class ErrorSink : IEnumerable<ErrorEntry>
    {
        private List<ErrorEntry> _errors;

        public IEnumerable<ErrorEntry> Errors => _errors.AsReadOnly();

        public ErrorSink()
        {
            _errors = new List<ErrorEntry>();
        }

        public void AddError(string message, SourceCode sourceCode, Severity severity, SourceSpan span)
        {
            _errors.Add(new ErrorEntry(message, sourceCode.GetLines(span.Start.Line, span.End.Line), severity, span));
        }

        public void Clear()
        {
            _errors.Clear();
        }

        public IEnumerator<ErrorEntry> GetEnumerator()
        {
            return _errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _errors.GetEnumerator();
        }
    }

    public enum Severity
    {
        None,
        Message,
        Warning,
        Error,
        Fatal
    }
}
