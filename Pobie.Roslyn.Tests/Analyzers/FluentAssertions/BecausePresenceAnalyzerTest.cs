using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Pobie.Roslyn.Analyzers.FluentAssertions;
using Xunit;

namespace Pobie.Roslyn.Tests.Analyzers.FluentAssertions;

public class BecausePresenceAnalyzerTest : AnalyzerTest<BecausePresenceAnalyzer>
{
    [Fact]
    public async Task ShouldWarn_WhenCallingBooleanAssertion_GivenNoBecauseValue()
    {
        const string assertionWithoutReasoning = """
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

        await ShouldHaveDiagnostic(
            BecausePresenceAnalyzer.Id,
            10,
            9,
            assertionWithoutReasoning,
            DiagnosticSeverity.Warning
        );
    }

    [Fact]
    public async Task ShouldNotWarn_WhenCallingBooleanAssertion_GivenABecauseValue()
    {
        const string assertionWithReasoning = """
            using FluentAssertions;
            using Xunit;

            public class BecausePresence
            {
                [Fact]
                public void ShouldReturnTrue(){
                    var aValue = true;
                    aValue.Should().Be(true, "because it is true");
                }
            }
            """;

        await ShouldNotHaveDiagnostic(assertionWithReasoning);
    }
}
