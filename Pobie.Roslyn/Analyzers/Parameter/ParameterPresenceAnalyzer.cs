using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Diagnostic = Microsoft.CodeAnalysis.Diagnostic;
using SemanticModel = Microsoft.CodeAnalysis.SemanticModel;

namespace Pobie.Roslyn.Analyzers.Parameter;

public abstract class ParameterPresenceAnalyzer : ParameterAnalyzer
{
    protected override void AnalyzeInvocation(
        OperationAnalysisContext context,
        InvocationExpressionSyntax syntax,
        SemanticModel semanticModel
    )
    {
        if (HasTargetParameter(syntax, semanticModel))
        {
            return;
        }

        context.ReportDiagnostic(
            Diagnostic.Create(SupportedDiagnostics.First(), syntax.Expression.GetLocation())
        );
    }

    private bool HasTargetParameter(
        InvocationExpressionSyntax invocationSyntax,
        SemanticModel semanticModel
    )
    {
        return invocationSyntax.ArgumentList.Arguments.Any(arg =>
            GetParameterName(semanticModel, arg) == TargetParameterName
        );
    }
}
