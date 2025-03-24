using System.Threading.Tasks;
using Analyzers.Analyzers.FluentAssertions;
using Analyzers.Tests.Analyzers;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Analyzers.Tests.FluentAssertions;

public class BecauseStartsWithKeywordAnalyzerTest : AnalyzerTest<BecauseStartsWithKeywordAnalyzer>
{
    [Test]
    public async Task ShouldWarn_WhenCallingBooleanAssertion_GivenBecauseDoesNotStartWithKeyword()
    {
        const string assertionWithPunctuation = """
            using FluentAssertions;
            using NUnit.Framework;

            public class BecausePresence
            {
                [Test]
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

    [Test]
    public async Task ShouldNotWarn_WhenCallingBooleanAssertion_GivenBecauseStartsWithKeyword()
    {
        const string assertionWithoutPunctuation = """
            using FluentAssertions;
            using NUnit.Framework;

            public class BecausePresence
            {
                [Test]
                public void ShouldReturnTrue()
                {
                    var aValue = true;
                    aValue.Should().Be(true, "because it should be true");
                }
            }
            """;

        await ShouldNotHaveDiagnostic(assertionWithoutPunctuation);
    }

    [Test]
    public async Task ShouldNotWarn_WhenCallingBooleanAssertion_GivenNoBecause()
    {
        const string assertionWithoutPunctuation = """
            using FluentAssertions;
            using NUnit.Framework;

            public class BecausePresence
            {
                [Test]
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
