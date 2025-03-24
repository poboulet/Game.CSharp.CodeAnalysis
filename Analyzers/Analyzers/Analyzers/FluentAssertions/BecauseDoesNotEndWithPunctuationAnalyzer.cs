using Analyzers.Analyzers.Parameter;
using Analyzers.Validations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.Analyzers.FluentAssertions;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class BecauseDoesNotEndWithPunctuationAnalyzer : ParameterValueAnalyzer
{
    public const string Id = "UT0003";
    protected override string TargetClassName => "BooleanAssertions";
    protected override string TargetMethodName => "*";
    protected override string TargetParameterName => "because";

    protected override IValidation Validation { get; } = new StringDoesNotEndWithPunctuation();

    protected override RuleDefinition RuleDefinition { get; } =
        new(
            Id,
            nameof(Resources.UT0003Title),
            nameof(Resources.UT0003MessageFormat),
            nameof(Resources.UT0003Description),
            DiagnosticSeverity.Warning,
            DiagnosticCategory.Usage
        );
}
