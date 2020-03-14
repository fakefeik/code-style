using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RoslynAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AssertEqualsAnalyzer : DiagnosticAnalyzer
    {
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var expression = (InvocationExpressionSyntax)context.Node;
            if (!(expression.Expression is MemberAccessExpressionSyntax memberAccess))
                return;

            var type = context.SemanticModel.GetTypeInfo(memberAccess.Expression).Type;
            if (type == null || type.ContainingNamespace?.ToString() != "NUnit.Framework" || type.Name != "Assert")
                return;

            if (memberAccess.Name.ToString() != "AreEqual")
                return;

            context.ReportDiagnostic(Diagnostic.Create(rule, context.Node.GetLocation()));
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(rule);

        public const string DiagnosticId = "NoNUnitAssert";

        private const string AssertName = "NUnit.Framework.Assert";

        private readonly DiagnosticDescriptor rule = new DiagnosticDescriptor(
            "NoNUnitAssert", "AnalyzerTitle", "Следует использовать FluentAssertions", "TestAnalyzers",
            DiagnosticSeverity.Warning, isEnabledByDefault : true, description : "AnalyzerDescription");
    }
}