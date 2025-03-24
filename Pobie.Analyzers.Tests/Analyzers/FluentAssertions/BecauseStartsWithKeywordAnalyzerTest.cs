using System.Threading.Tasks;
using Analyzers.Analyzers.FluentAssertions;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Analyzers.Tests.Analyzers.FluentAssertions;

public class BecauseStartsWithKeywordAnalyzerTest : AnalyzerTest<BecauseStartsWithKeywordAnalyzer>
{
    [Fact]
    public async Task ShouldWarn_WhenCallingBooleanAssertion_GivenBecauseDoesNotStartWithKeyword()
    {
        const string assertionWithPunctuation = """
            using FluentAssertions;
            using Xunit;

            public class BecausePresence
            {
                [Fact]
                public void ShouldReturnTrue()
                {
                    var aValue = true;
                    aValue.Should().Be(true, "it should be true.");
                }
            }
            """;

        await ShouldHaveDiagnostic(
            BecauseStartsWithKeywordAnalyzer.Id,
            10,
            9,
            assertionWithPunctuation,
            DiagnosticSeverity.Warning
        );
    }

    [Fact]
    public async Task ShouldNotWarn_WhenCallingBooleanAssertion_GivenBecauseStartsWithKeyword()
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
    public async Task ShouldNotWarn_WhenCallingBooleanAssertion_GivenNoBecause()
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
                    aValue.Should().Be(true);
                }
            }
            """;

        await ShouldNotHaveDiagnostic(assertionWithoutPunctuation);
    }
}
