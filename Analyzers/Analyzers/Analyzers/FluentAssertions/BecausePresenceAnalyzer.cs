using Analyzers.Analyzers.Parameter;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.Analyzers.FluentAssertions;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class BecausePresenceAnalyzer : ParameterPresenceAnalyzer
{
    public const string Id = "UT0001";
    protected override string TargetClassName => "BooleanAssertions";
    protected override string TargetMethodName => "*";
    protected override string TargetParameterName => "because";

    protected override RuleDefinition RuleDefinition { get; } =
        new(
            Id,
            nameof(Resources.UT0001Title),
            nameof(Resources.UT0001MessageFormat),
            nameof(Resources.UT0001Description),
            DiagnosticSeverity.Warning,
            DiagnosticCategory.Usage
        );
}
