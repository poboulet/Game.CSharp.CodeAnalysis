using Analyzers.Analyzers.Parameter;
using Analyzers.Validations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers.Analyzers.FluentAssertions;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class BecauseStartsWithKeywordAnalyzer : ParameterValueAnalyzer
{
    public const string Id = "UT0002";
    protected override string TargetClassName => "BooleanAssertions";
    protected override string TargetMethodName => "*";
    protected override string TargetParameterName => "because";

    protected override IValidation Validation { get; } = new StringStartsWithValidation("because ");

    protected override RuleDefinition RuleDefinition { get; } =
        new(
            Id,
            nameof(Resources.UT0002Title),
            nameof(Resources.UT0002MessageFormat),
            nameof(Resources.UT0002Description),
            DiagnosticSeverity.Warning,
            DiagnosticCategory.Usage
        );
}
