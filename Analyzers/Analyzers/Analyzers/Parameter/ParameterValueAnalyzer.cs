using System.Linq;
using Analyzers.Validations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Diagnostic = Microsoft.CodeAnalysis.Diagnostic;
using DiagnosticDescriptor = Microsoft.CodeAnalysis.DiagnosticDescriptor;
using SemanticModel = Microsoft.CodeAnalysis.SemanticModel;

namespace Analyzers.Analyzers.Parameter;

public abstract class ParameterValueAnalyzer : ParameterAnalyzer
{
    protected abstract IValidation Validation { get; }

    private DiagnosticDescriptor Descriptor => RuleDefinition.ToDiagnosticDescriptor();

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

        context.ReportDiagnostic(Diagnostic.Create(Descriptor, syntax.Expression.GetLocation()));
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
