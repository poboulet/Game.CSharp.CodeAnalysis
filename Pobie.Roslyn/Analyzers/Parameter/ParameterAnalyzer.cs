using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Pobie.Roslyn.Analyzers.Parameter;

public abstract class ParameterAnalyzer : DiagnosticAnalyzer
{
    protected abstract string TargetClassName { get; }
    protected abstract string TargetMethodName { get; }
    protected abstract string TargetParameterName { get; }
    protected abstract DiagnosticDescriptor Rule { get; }

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterOperationAction(ValidateInvocation, OperationKind.Invocation);
    }

    private bool MatchesTargetMethod(IInvocationOperation invocationOperation)
    {
        IMethodSymbol methodSymbol = invocationOperation.TargetMethod;
        bool isTargetClass = Regex.IsMatch(
            methodSymbol.ReceiverType?.Name ?? string.Empty,
            TargetClassName
        );
        bool isTargetMethod = Regex.IsMatch(methodSymbol.Name, TargetMethodName);
        bool hasTargetParameter = methodSymbol.Parameters.Any(p =>
            Regex.IsMatch(TargetParameterName, p.Name)
        );

        return methodSymbol.MethodKind == MethodKind.Ordinary
            && isTargetClass
            && isTargetMethod
            && hasTargetParameter;
    }

    private void ValidateInvocation(OperationAnalysisContext context)
    {
        if (
            context.Operation is not IInvocationOperation invocationOperation
            || !MatchesTargetMethod(invocationOperation)
            || invocationOperation
                is not {
                    Syntax: InvocationExpressionSyntax syntax,
                    SemanticModel: { } semanticModel
                }
        )
        {
            return;
        }

        AnalyzeInvocation(context, syntax, semanticModel);
    }

    protected static string GetParameterName(
        SemanticModel semanticModel,
        ArgumentSyntax argumentSyntax
    )
    {
        return
            semanticModel.GetOperation(argumentSyntax)
                is IArgumentOperation { Parameter: { } parameter }
            ? parameter.Name
            : string.Empty;
    }

    protected abstract void AnalyzeInvocation(
        OperationAnalysisContext context,
        InvocationExpressionSyntax syntax,
        SemanticModel semanticModel
    );
}
