using System.Threading.Tasks;
using Analyzers.Analyzers.FluentAssertions;
using Analyzers.Tests.Analyzers;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Analyzers.Tests.FluentAssertions;

public class BecausePresenceAnalyzerTest : AnalyzerTest<BecausePresenceAnalyzer>
{
    [Test]
    public async Task ShouldWarn_WhenCallingBooleanAssertion_GivenNoBecauseValue()
    {
        const string assertionWithoutReasoning = """
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

        await ShouldHaveDiagnostic(
            BecausePresenceAnalyzer.Id,
            10,
            9,
            assertionWithoutReasoning,
            DiagnosticSeverity.Warning
        );
    }

    [Test]
    public async Task ShouldNotWarn_WhenCallingBooleanAssertion_GivenABecauseValue()
    {
        const string assertionWithReasoning = """
            using FluentAssertions;
            using NUnit.Framework;

            public class BecausePresence
            {
                [Test]
                public void ShouldReturnTrue(){
                    var aValue = true;
                    aValue.Should().Be(true, "because it is true");
                }
            }
            """;

        await ShouldNotHaveDiagnostic(assertionWithReasoning);
    }
}
