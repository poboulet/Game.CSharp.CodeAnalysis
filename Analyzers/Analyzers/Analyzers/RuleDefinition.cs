using Microsoft.CodeAnalysis;

namespace Analyzers.Analyzers;

public class RuleDefinition(
    string id,
    string title,
    string message,
    string description,
    DiagnosticSeverity severity,
    DiagnosticCategory category,
    bool isEnabledByDefault = true
)
{
    private LocalizableResourceString Title { get; } =
        new(title, Resources.ResourceManager, typeof(Resources));

    private LocalizableResourceString Message { get; } =
        new(message, Resources.ResourceManager, typeof(Resources));

    private LocalizableResourceString Description { get; } =
        new(description, Resources.ResourceManager, typeof(Resources));

    public DiagnosticDescriptor ToDiagnosticDescriptor()
    {
        return new DiagnosticDescriptor(
            id,
            Title,
            Message,
            category.ToString(),
            severity,
            isEnabledByDefault,
            Description
        );
    }
}
