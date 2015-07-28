using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SnippetVS
{
    public class DiscoveryWalker : CSharpSyntaxWalker
    {
        SemanticModel _model;
        int _start, _end;

        public Dictionary<string, string> DefinedOutside { get; private set; }

        public DiscoveryWalker(int start, int end, SemanticModel model)
        {
            _model = model;
            _start = start;
            _end = end;
            DefinedOutside = new Dictionary<string, string>();
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            base.VisitFieldDeclaration(node);
            foreach (var variable in node.Declaration.Variables)
            {
                var test = variable.Identifier;
                DefinedOutside.Add(variable.Identifier.ToString(), variable.ToFullString());
            }
            var t = DefinedOutside;
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            base.VisitPropertyDeclaration(node);
            DefinedOutside.Add(node.Identifier.ToString(), node.ToFullString());
        }

    }
}
