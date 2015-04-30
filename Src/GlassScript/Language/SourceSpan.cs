using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassScript.Language
{
    public struct SourceSpan : IEquatable<SourceSpan>
    {
        private readonly SourceLocation _end;
        private readonly SourceLocation _start;

        public SourceLocation End => _end;

        public int Length => _end.Index - _start.Index;

        public SourceLocation Start => _start;

        public SourceSpan(SourceLocation start, SourceLocation end)
        {
            _start = start;
            _end = end;
        }

        public static bool operator !=(SourceSpan left, SourceSpan right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(SourceSpan left, SourceSpan right)
        {
            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (obj is SourceSpan)
            {
                return Equals((SourceSpan)obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(SourceSpan other)
        {
            return other.Start == Start && other.End == End;
        }

        public override int GetHashCode()
        {
            return 0x509CE ^ Start.GetHashCode() ^ End.GetHashCode();
        }

        public override string ToString()
        {
            return $"{_start.Line} {_start.Column} {Length}";
        }
    }
}
