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
    public class MethodWalker : CSharpSyntaxWalker
    {
        SemanticModel _model;
        int _start, _end;

        public Dictionary<string, string> _DefinedOutside { get; private set; }
        public StringBuilder sb;

        public MethodWalker(int start, int end, SemanticModel model, Dictionary<string, string> definedOutside)
        {
            _model = model;
            _start = start;
            _end = end;
            _DefinedOutside = definedOutside;
            sb = new StringBuilder();
        }

        public override void Visit(SyntaxNode node)
        {

            var t = node.Kind();
            if (node.IsKind(SyntaxKind.IdentifierName))
            {
                var name = node.ToString();
                string syntax;
                if (_DefinedOutside.TryGetValue(name, out syntax))
                {
                    _DefinedOutside.Remove(name);
                    sb.Append(syntax);
                }
            }
            /*
            var symbol = _model.GetSymbolInfo(node).Symbol;
            var ca = _model.GetSymbolInfo(node).CandidateSymbols;
            var declaredSymbol = _model.GetDeclaredSymbol(node);


            if (symbol?.Kind == SymbolKind.Field)
            {
                var x = 3;
            }
            if (symbol != null)
            {
                var test = symbol.CanBeReferencedByName;
                var sources = symbol.DeclaringSyntaxReferences;

                if (sources.Any())
                {
                    foreach (var source in sources)
                    {
                        if (source.Span.Start > _end
                            || source.Span.End < _start)
                        {
                            //DefinedOutside.Add(source.GetSyntax().ToFullString());
                        }
                    }
                }
                var locations = symbol?.Locations;
                if (locations.HasValue)
                {
                    /*
                    foreach (var location in locations.Value)
                    {
                        if (location.SourceSpan.Start > _end
                            || location.SourceSpan.End < _start)
                        {
                            DefinedOutside.Add(symbol);
                        }
                    }

                }
            }*/
            base.Visit(node);
        }

    }
}
