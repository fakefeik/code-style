using System.Collections.Immutable;
using System.IO;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

using NUnit.Framework;

using RoslynAnalyzer;

namespace TestProject
{
    public class AnalyzerTest
    {
        [Test]
        public void Test1()
        {
            var file = $"{TestContext.CurrentContext.TestDirectory}/../../../UnitTest1.cs";
            var assemblies = new[] {typeof(object), typeof(Enumerable), typeof(TestAttribute)}.Select(x => x.Assembly.Location).ToArray();

            var project = new AdhocWorkspace().AddProject("test", LanguageNames.CSharp);
            project = project.AddDocument(Path.GetFileName(file), File.ReadAllText(file)).Project;
            project = project.AddMetadataReferences(assemblies.Select(x => MetadataReference.CreateFromFile(x)));

            var compilation = project.GetCompilationAsync().GetAwaiter().GetResult();
            var c = compilation.WithAnalyzers(ImmutableArray.Create((DiagnosticAnalyzer)new MakeConstAnalyzer()));
            var diagnostics = c.GetAnalyzerDiagnosticsAsync().GetAwaiter().GetResult();
        }
    }
}