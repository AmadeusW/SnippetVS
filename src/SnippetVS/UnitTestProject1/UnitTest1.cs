using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SnippetVS;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var tree = CSharpSyntaxTree.ParseText(@"
                class MyClass
                {
                    int test = 3;
                    int MyTest = {get; set;}
                    void MyMethod()
                    {
                        int x = test;
                        MyTest = x;
                    }
                }");

            var compilation = CreateCompilation(tree);
            var model = compilation.GetSemanticModel(tree);

            var visitor1 = new DiscoveryWalker(20, 30, model);
            visitor1.Visit(tree.GetRoot());
            var visitor2 = new MethodWalker(20, 30, model, visitor1.DefinedOutside);
            visitor2.Visit(tree.GetRoot());
            var test = visitor2.sb.ToString();
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
