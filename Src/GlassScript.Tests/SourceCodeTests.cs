using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassScript.Language;
using Xunit;

namespace GlassScript.Tests
{
    public class SourceCodeTests
    {
        private const string program =
@"var name = 'Me';
name == 'Me';
return name;
// Final Line";

        private SourceCode code = new SourceCode(program);

        [Fact]
        public void ContentsMatch()
        {
            Assert.Equal(program, code.Contents);
        }

        [Fact]
        public void SourceCodeHandlesOneSubset()
        {
            string[] selectedLines = code.GetLines(1, 1);

            Assert.True(selectedLines.Length == 1);
            Assert.Equal(code.GetLine(1), selectedLines.First());
        }

        [Fact]
        public void SourceCodeShouldReturnCorrectLine()
        {
            Assert.True(code.GetLine(1).Substring(0, 3) == "var");

            Assert.True(code.GetLine(4).StartsWith("//"));

            Assert.False(code.GetLine(2).Contains("return"));
        }

        [Fact]
        public void SourceCodeShouldReturnCorrectSubset()
        {
            string[] selectedLines = code.GetLines(2, 4);

            Assert.True(selectedLines.Length == 3);

            Assert.DoesNotContain(code.GetLine(1), selectedLines);
            Assert.Contains(code.GetLine(2), selectedLines);
        }

        [Fact]
        public void SubsetThrowsOutOfRange()
        {
            Assert.Throws<IndexOutOfRangeException>(() => code.GetLines(0, int.MaxValue));
            Assert.Throws<IndexOutOfRangeException>(() => code.GetLine(0));
            Assert.Throws<IndexOutOfRangeException>(() => code.GetLine(-1));
        }
    }
}
