using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Pobie.Roslyn.Analyzers.Parameter;
using Pobie.Roslyn.Validations;

namespace Pobie.Roslyn.Analyzers.FluentAssertions;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class BecauseStartsWithKeywordAnalyzer : ParameterValueAnalyzer
{
    public const string Id = "UT0002";

    private static readonly DiagnosticDescriptor DiagnosticDescriptor =
        new(
            Id,
            Resources.UT0002Title,
            Resources.UT0002MessageFormat,
            DiagnosticCategory.Usage.ToString(),
            DiagnosticSeverity.Warning,
            true,
            Resources.UT0002Description
        );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(Rule);

    protected override string TargetClassName => ".*Assertions";
    protected override string TargetMethodName => ".*";
    protected override string TargetParameterName => "because";

    protected override DiagnosticDescriptor Rule => DiagnosticDescriptor;
    protected override IValidation Validation { get; } = new StringStartsWithValidation("because ");
}
