using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;

namespace RNGui.BuildInfo
{
    [Generator(LanguageNames.CSharp)]
    public sealed class BuildDateSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var targets =
                context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    "RNGui.BuildInfo.CompilationTimeStringAttribute",
                    static (node, _) => node is FieldDeclarationSyntax { Parent: ClassDeclarationSyntax, AttributeLists.Count: > 0 },
                    static (context, token) => { }
                );
                .//Where(static item => item.Hier ;

            //
            IncrementalValueProvider<(Compilation, ImmutableArray<FieldDeclarationSyntax>)> combined
                = context.CompilationProvider.Combine(targets.Collect());

            // Generate combined source
            context.RegisterSourceOutput(combined,
                static (spc, source) => Execute(source.Item1, source.Item2, spc));
        }
    }
}
