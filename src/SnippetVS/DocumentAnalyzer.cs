using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SnippetVS
{
    internal class DocumentAnalyzer
    {
        internal string FindContainedMethods(string filePath, int startPosition, int endPosition)
        {
            var currentSolution = SolutionManager.CurrentSolution;
            var project = currentSolution.Projects.Where(n => n.Documents.Any(m => m.FilePath == filePath)).FirstOrDefault();
            var document = project.Documents.Where(n => n.FilePath == filePath).Single();

            var root = document.GetSyntaxRootAsync().Result;
            var token1 = root.FindToken(startPosition);
            var token2 = root.FindToken(endPosition);

            // Find the containing method or property for this token
            var member1 = token1.Parent.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().FirstOrDefault();
            var member2 = token2.Parent.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().FirstOrDefault();

            var commonParent = token1.Parent.FirstAncestorOrSelf<SyntaxNode>(ancestor => token2.Parent.AncestorsAndSelf().Contains(ancestor));
            var commonMethod = commonParent.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().FirstOrDefault();

            var commonType = commonParent.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().FirstOrDefault();

            var affectedMethods = commonType.ChildNodes().OfType<MemberDeclarationSyntax>().Where(n => n == member1 || n.SpanStart > startPosition && n.SpanStart < endPosition);


            var compilation = CreateCompilation(root.SyntaxTree);
            var model = compilation.GetSemanticModel(root.SyntaxTree);


            var visitor1 = new DiscoveryWalker(startPosition, endPosition, model);
            visitor1.Visit(commonType);
            var visitor2 = new MethodWalker(20, 30, model, visitor1.DefinedOutside);
            foreach (var method in affectedMethods)
            {
                visitor2.Visit(method);
            }
            foreach (var method in affectedMethods)
            {
                visitor2.sb.Append(method.ToFullString());
            }
            return visitor2.sb.ToString();
        }

        internal static Compilation CreateCompilation(SyntaxTree tree)
        {
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var compilation = CSharpCompilation.Create("MyCompilation",
                syntaxTrees: new[] { tree }, references: new[] { mscorlib });
            return compilation;
        }
    }
}
