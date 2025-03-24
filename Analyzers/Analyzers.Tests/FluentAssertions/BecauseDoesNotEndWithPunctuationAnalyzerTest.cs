using System.Threading.Tasks;
using Analyzers.Analyzers.FluentAssertions;
using Analyzers.Tests.Analyzers;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Analyzers.Tests.FluentAssertions;

public class BecauseDoesNotEndWithPunctuationAnalyzerTest
    : AnalyzerTest<BecauseDoesNotEndWithPunctuationAnalyzer>
{
    [TestCase(".", TestName = "ShouldWarn_WhenCallingBooleanAssertion_GivenBecauseEndsWithPeriod")]
    [TestCase(",", TestName = "ShouldWarn_WhenCallingBooleanAssertion_GivenBecauseEndsWithComma")]
    [TestCase(
        "!",
        TestName = "ShouldWarn_WhenCallingBooleanAssertion_GivenBecauseEndsWithExclamation"
    )]
    [TestCase(
        "?",
        TestName = "ShouldWarn_WhenCallingBooleanAssertion_GivenBecauseEndsWithQuestionMark"
    )]
    [TestCase(
        ";",
        TestName = "ShouldWarn_WhenCallingBooleanAssertion_GivenBecauseEndsWithSemicolon"
    )]
    [TestCase(":", TestName = "ShouldWarn_WhenCallingBooleanAssertion_GivenBecauseEndsWithColon")]
    public async Task ShouldWarn_WhenCallingBooleanAssertion_GivenPunctuationAtTheEndOfBecause(
        string punctuation
    )
    {
        string assertionWithPunctuation = $$"""
            using FluentAssertions;
            using NUnit.Framework;

            public class BecausePresence
            {
                [Test]
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

    [Test]
    public async Task ShouldNotWarn_WhenCallingBooleanAssertion_GivenNoPunctuationAtTheEndOfBecause()
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
    public async Task ShouldNotWarn_WhenCallingBooleanAssertion_GivenEmptyString()
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
                    aValue.Should().Be(true, "");
                }
            }
            """;

        await ShouldNotHaveDiagnostic(assertionWithoutPunctuation);
    }
}
