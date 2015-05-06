using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassScript.Language.Lexer;
using GlassScript.Language.Syntax;

namespace GlassScript.Language
{
    public sealed class SourceCode
    {
        private Lazy<string[]> _lines;
        private string _sourceCode;

        public string Contents => _sourceCode;

        public string[] Lines => _lines.Value;

        public char this[int index]
        {
            get { return _sourceCode.CharAt(index); }
        }

        private class Subset<T> : IEnumerable<T>
        {
            private int _end;
            private IEnumerable<T> _set;

            private int _start;

            private struct SubsetEnumerator : IEnumerator<T>
            {
                private bool _disposed;
                private int _index;
                private Subset<T> _subset;

                public T Current => _subset._set.ElementAt(_index);

                object IEnumerator.Current => _subset._set.ElementAt(_index);

                public SubsetEnumerator(Subset<T> subset)
                {
                    _disposed = false;
                    _index = subset._start - 1; // MoveNext() appears to be called before get_Current.
                    _subset = subset;
                }

                public void Dispose()
                {
                    _disposed = true;
                }

                public bool MoveNext()
                {
                    if (_disposed)
                    {
                        throw new ObjectDisposedException("SubsetEnumerator");
                    }
                    if (_index == _subset._end)
                    {
                        return false;
                    }
                    _index++;
                    return true;
                }

                public void Reset()
                {
                    if (_disposed)
                    {
                        throw new ObjectDisposedException("SubsetEnumerator");
                    }
                    _index = _subset._start;
                }
            }

            public Subset(IEnumerable<T> collection, int start, int end)
            {
                _set = collection;
                _start = start;
                _end = end;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new SubsetEnumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new SubsetEnumerator(this);
            }
        }

        public SourceCode(string sourceCode)
        {
            _sourceCode = sourceCode;
            _lines = new Lazy<string[]>(() => _sourceCode.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
        }

        public string GetLine(int line)
        {
            if (line < 1)
            {
                throw new IndexOutOfRangeException($"{nameof(line)} must not be less than 1!");
            }
            if (line > Lines.Length)
            {
                throw new IndexOutOfRangeException($"No line {line}!");
            }

            // Lines start at 1; array indexes start at 0.
            return Lines[line - 1];
        }

        public string[] GetLines(int start, int end)
        {
            if (end < start)
            {
                throw new IndexOutOfRangeException("Cannot retrieve negative range!");
            }
            if (start < 1)
            {
                throw new IndexOutOfRangeException($"{nameof(start)} must not be less than 1!");
            }
            if (end > Lines.Length)
            {
                throw new IndexOutOfRangeException("Cannot retrieve more lines than exist in file!");
            }

            // Line indexes are offset by +1 compared to array indexes.
            return new Subset<string>(Lines, start - 1, end - 1).ToArray();
        }

        public string GetSpan(SourceSpan span)
        {
            int start = span.Start.Index;
            int length = span.Length;
            return _sourceCode.Substring(start, length);
        }

        public string GetSpan(SyntaxNode node)
        {
            return GetSpan(node.Span);
        }
    }
}
