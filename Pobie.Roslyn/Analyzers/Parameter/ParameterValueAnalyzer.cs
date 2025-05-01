using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Pobie.Roslyn.Validations;
using Diagnostic = Microsoft.CodeAnalysis.Diagnostic;
using SemanticModel = Microsoft.CodeAnalysis.SemanticModel;

namespace Pobie.Roslyn.Analyzers.Parameter;

public abstract class ParameterValueAnalyzer : ParameterAnalyzer
{
    protected abstract IValidation Validation { get; }

    protected override void AnalyzeInvocation(
        OperationAnalysisContext context,
        InvocationExpressionSyntax syntax,
        SemanticModel semanticModel
    )
    {
        if (ParameterHasValidValue(syntax, semanticModel, TargetParameterName))
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(Rule, syntax.Expression.GetLocation()));
    }

    private bool ParameterHasValidValue(
        InvocationExpressionSyntax invocationSyntax,
        SemanticModel semanticModel,
        string targetParameterName
    )
    {
        ArgumentSyntax? argument = invocationSyntax.ArgumentList.Arguments.FirstOrDefault(
            argument => GetParameterName(semanticModel, argument).Equals(targetParameterName)
        );

        if (argument is null)
        {
            return true;
        }

        IOperation? operation = semanticModel.GetOperation(argument.Expression);
        return operation?.ConstantValue is { HasValue: true, Value: { } value }
            && Validation.Validate(value);
    }
}
