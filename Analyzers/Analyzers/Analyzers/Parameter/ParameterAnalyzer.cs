using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Analyzers.Analyzers.Parameter;

public abstract class ParameterAnalyzer : DiagnosticAnalyzer
{
    protected abstract string TargetClassName { get; }
    protected abstract string TargetMethodName { get; }
    protected abstract string TargetParameterName { get; }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(RuleDefinition.ToDiagnosticDescriptor());

    protected abstract RuleDefinition RuleDefinition { get; }

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterOperationAction(ValidateInvocation, OperationKind.Invocation);
    }

    private bool MatchesTargetMethod(IInvocationOperation invocationOperation)
    {
        IMethodSymbol methodSymbol = invocationOperation.TargetMethod;
        bool isTargetClass = methodSymbol.ReceiverType?.Name == TargetClassName;
        bool isTargetMethod = TargetMethodName == "*" || methodSymbol.Name == TargetMethodName;
        bool hasTargetParameter = methodSymbol.Parameters.Any(p => p.Name == TargetParameterName);

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
