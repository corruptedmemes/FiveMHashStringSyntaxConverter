using System.Collections.Immutable;
using System.IO;
using CurrentNamespace;
using Loretta.CodeAnalysis;
using Loretta.CodeAnalysis.Lua;

namespace ShittyFiveMConverter
{
    internal class Program
    {
        public static void Main()
        {
            var syntaxTree = LuaSyntaxTree.ParseText(File.ReadAllText("a-input.lua"));
            var ast = new Script(ImmutableArray.Create(syntaxTree));
            File.WriteAllText("a-output.lua", new ClassName(ast.SyntaxTrees).Retrieve()[0].ToString());
        }
    }
}