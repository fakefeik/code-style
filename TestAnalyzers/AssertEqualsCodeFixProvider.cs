using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace TestAnalyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp)]
    public class AssertEqualsCodeFixProvider : CodeFixProvider
    {
        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.Single();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var invocation = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<InvocationExpressionSyntax>().First();
            context.RegisterCodeFix(
                CodeAction.Create(
                    title : "Use FluentAssertions",
                    createChangedDocument : c => MakeConstAsync(context.Document, invocation, c),
                    equivalenceKey : "Use FluentAssertions"),
                diagnostic);
        }

        private static async Task<Document> MakeConstAsync(Document document, InvocationExpressionSyntax invocationExpression, CancellationToken cancellationToken)
        {
            var shouldInvocation = SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                                     invocationExpression.ArgumentList.Arguments[1].Expression,
                                                     SyntaxFactory.IdentifierName("Should")));

            var beArgs = new List<ArgumentSyntax> {SyntaxFactory.Argument(invocationExpression.ArgumentList.Arguments[0].Expression)};
            beArgs.AddRange(invocationExpression.ArgumentList.Arguments.Skip(2));

            var beInvocation = SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, shouldInvocation, SyntaxFactory.IdentifierName("Be")),
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(beArgs)));

            var formattedInvocation = beInvocation.WithAdditionalAnnotations(Formatter.Annotation);

            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = oldRoot.ReplaceNode(invocationExpression, formattedInvocation);

            return document.WithSyntaxRoot(newRoot);
        }

        public override sealed FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AssertEqualsAnalyzer.DiagnosticId);
    }
}