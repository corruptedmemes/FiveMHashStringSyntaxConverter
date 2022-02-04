using System;
using System.Collections.Immutable;
using System.Linq;
using Loretta.CodeAnalysis;
using Loretta.CodeAnalysis.Lua;
using Loretta.CodeAnalysis.Lua.Syntax;

namespace CurrentNamespace
{
    public class ClassName
    {
        private static SyntaxNode GetInnerExpression(SyntaxNode node) =>
            node.IsKind(SyntaxKind.ParenthesizedExpression)
                ? GetInnerExpression(((ParenthesizedExpressionSyntax) node).Expression)
                : node;

        private static T GetValue<T>(SyntaxNode node)
        {
            return (T) ((LiteralExpressionSyntax) GetInnerExpression(node)).Token.Value;
        }
        
        private class RewriterClass : LuaSyntaxRewriter
        {
            public override SyntaxNode VisitLiteralExpression(LiteralExpressionSyntax node)
            {
                return node.Kind() == SyntaxKind.HashStringLiteralExpression ? SyntaxFactory.LiteralExpression(
                    SyntaxKind.NumericalLiteralExpression, SyntaxFactory.Literal(GetValue<uint>(node))) : node;
            }
        }

        private readonly ImmutableArray<SyntaxTree> _ast;
        
        public ClassName(ImmutableArray<SyntaxTree> ast)
        {
            _ast = ast.Select(tree => tree.WithRootAndOptions(
                new RewriterClass().Visit(tree.GetRoot()), tree.Options)).ToImmutableArray();
        }

        public ImmutableArray<SyntaxTree> Retrieve()
        {
            return _ast;
        }
    }
}