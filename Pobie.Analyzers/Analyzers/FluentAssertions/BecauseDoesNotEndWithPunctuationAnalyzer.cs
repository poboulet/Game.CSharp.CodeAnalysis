using System.Collections.Immutable;
using Analyzers.Analyzers.Parameter;
using Analyzers.Validations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.Analyzers.FluentAssertions;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class BecauseDoesNotEndWithPunctuationAnalyzer : ParameterValueAnalyzer
{
    public const string Id = "UT0003";

    private static readonly DiagnosticDescriptor DiagnosticDescriptor =
        new(
            Id,
            Resources.UT0003Title,
            Resources.UT0003MessageFormat,
            DiagnosticCategory.Usage.ToString(),
            DiagnosticSeverity.Warning,
            true,
            Resources.UT0003Description
        );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(Rule);

    protected override string TargetClassName => "BooleanAssertions";
    protected override string TargetMethodName => "*";
    protected override string TargetParameterName => "because";
    protected override DiagnosticDescriptor Rule => DiagnosticDescriptor;

    protected override IValidation Validation { get; } = new StringDoesNotEndWithPunctuation();
}
