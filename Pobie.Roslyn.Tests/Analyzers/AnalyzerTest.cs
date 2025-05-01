using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing.XUnit;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace Pobie.Roslyn.Tests.Analyzers;

public abstract class AnalyzerTest<T>
    where T : DiagnosticAnalyzer, new()
{
    private CSharpAnalyzerTest<T, XUnitVerifier> Analyzer(
        string diagnosticId,
        int line,
        int column,
        string sources,
        DiagnosticSeverity severity
    )
    {
        CSharpAnalyzerTest<T, XUnitVerifier> analyzer = Analyzer(sources);
        analyzer.ExpectedDiagnostics.Add(
            AnalyzerVerifier<T>
                .Diagnostic(diagnosticId)
                .WithLocation(line, column)
                .WithSeverity(severity)
        );
        return analyzer;
    }

    private CSharpAnalyzerTest<T, XUnitVerifier> Analyzer(string sources)
    {
        return new CSharpAnalyzerTest<T, XUnitVerifier>
        {
            ReferenceAssemblies = ReferenceAssemblies.Default.AddPackages(
                [
                    new PackageIdentity("xunit", "2.4.2"),
                    new PackageIdentity("FluentAssertions", "8.2.0"),
                ]
            ),
            TestState = { Sources = { sources } },
        };
    }

    protected Task ShouldHaveDiagnostic(
        string diagnosticId,
        int line,
        int column,
        string sources,
        DiagnosticSeverity severity
    )
    {
        return Analyzer(diagnosticId, line, column, sources, severity).RunAsync();
    }

    protected Task ShouldNotHaveDiagnostic(string sources)
    {
        return Analyzer(sources).RunAsync();
    }
}
