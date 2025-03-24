using System.Collections.Immutable;
using Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Pobie.Analyzers.Analyzers.Parameter;

namespace Pobie.Analyzers.Analyzers.FluentAssertions;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class BecausePresenceAnalyzer : ParameterPresenceAnalyzer
{
    public const string Id = "UT0001";

    private static readonly DiagnosticDescriptor DiagnosticDescriptor =
        new(
            Id,
            Resources.UT0001Title,
            Resources.UT0001MessageFormat,
            DiagnosticCategory.Usage.ToString(),
            DiagnosticSeverity.Warning,
            true,
            Resources.UT0001Description
        );

    protected override DiagnosticDescriptor Rule => DiagnosticDescriptor;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(Rule);

    protected override string TargetClassName => "*Assertions";
    protected override string TargetMethodName => "*";
    protected override string TargetParameterName => "because";
}
