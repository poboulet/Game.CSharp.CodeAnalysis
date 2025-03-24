using System.Threading.Tasks;
using Analyzers.Analyzers.FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Analyzers.Tests.Analyzers.FluentAssertions;

public class BecauseDoesNotEndWithPunctuationAnalyzerTest
    : AnalyzerTest<BecauseDoesNotEndWithPunctuationAnalyzer>
{
    [Theory]
    [InlineData(".")]
    [InlineData(",")]
    [InlineData("!")]
    [InlineData("?")]
    [InlineData(";")]
    [InlineData(":")]
    public async Task ShouldWarn_WhenCallingBooleanAssertion_GivenPunctuationAtTheEndOfBecause(
        string punctuation
    )
    {
        string assertionWithPunctuation = $$"""
            using FluentAssertions;
            using Xunit;

            public class BecausePresence
            {
                [Fact]
                public void ShouldReturnTrue()
                {
                    var aValue = true;
                    aValue.Should().Be(true, "because it should be true{{punctuation}}");
                }
            }
            """;

        await ShouldHaveDiagnostic(
            BecauseDoesNotEndWithPunctuationAnalyzer.Id,
            10,
            9,
            assertionWithPunctuation,
            DiagnosticSeverity.Warning
        );
    }

    [Fact]
    public async Task ShouldNotWarn_WhenCallingBooleanAssertion_GivenNoPunctuationAtTheEndOfBecause()
    {
        const string assertionWithoutPunctuation = """
            using FluentAssertions;
            using Xunit;

            public class BecausePresence
            {
                [Fact]
                public void ShouldReturnTrue()
                {
                    var aValue = true;
                    aValue.Should().Be(true, "because it should be true");
                }
            }
            """;

        await ShouldNotHaveDiagnostic(assertionWithoutPunctuation);
    }

    [Fact]
    public async Task ShouldNotWarn_WhenCallingBooleanAssertion_GivenEmptyString()
    {
        const string assertionWithoutPunctuation = """
            using FluentAssertions;
            using Xunit;

            public class BecausePresence
            {
                [Fact]
                public void ShouldReturnTrue()
                {
                    var aValue = true;
                    aValue.Should().Be(true, "");
                }
            }
            """;

        await ShouldNotHaveDiagnostic(assertionWithoutPunctuation);
    }
}
