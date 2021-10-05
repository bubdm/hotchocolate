using System.Collections.Generic;
using HotChocolate.Language;
using HotChocolate.Language.Visitors;

namespace HotChocolate.Stitching.SchemaBuilding
{
    public class InspectSchemaVisitor : SyntaxVisitor<ISchemaRewriterContext>
    {
        private readonly static HashSet<SyntaxKind> _types = new()
        {
            SyntaxKind.EnumTypeDefinition,
            SyntaxKind.EnumTypeExtension,
            SyntaxKind.ObjectTypeDefinition,
            SyntaxKind.ObjectTypeExtension,
            SyntaxKind.InterfaceTypeDefinition,
            SyntaxKind.InterfaceTypeExtension,
            SyntaxKind.UnionTypeDefinition,
            SyntaxKind.UnionTypeExtension,
            SyntaxKind.ScalarTypeDefinition,
            SyntaxKind.ScalarTypeExtension,
            SyntaxKind.InputObjectTypeDefinition,
            SyntaxKind.InputObjectTypeExtension,
            SyntaxKind.SchemaDefinition,
            SyntaxKind.SchemaExtension
        };

        private readonly static HashSet<SyntaxKind> _visit = new()
        {
            SyntaxKind.Document,
            SyntaxKind.EnumTypeDefinition,
            SyntaxKind.EnumTypeExtension,
            SyntaxKind.ObjectTypeDefinition,
            SyntaxKind.ObjectTypeExtension,
            SyntaxKind.InterfaceTypeDefinition,
            SyntaxKind.InterfaceTypeExtension,
            SyntaxKind.UnionTypeDefinition,
            SyntaxKind.UnionTypeExtension,
            SyntaxKind.ScalarTypeDefinition,
            SyntaxKind.ScalarTypeExtension,
            SyntaxKind.InputObjectTypeDefinition,
            SyntaxKind.InputObjectTypeExtension,
            SyntaxKind.SchemaDefinition,
            SyntaxKind.SchemaExtension
        };

        protected override ISyntaxVisitorAction Enter(
            ISyntaxNode node,
            ISchemaRewriterContext context)
        {
            context.Path.Push(node);

            if (_types.Contains(node.Kind) &&
                node is IHasDirectives type)
            {
                foreach (ISchemaRewriter rewriter in context.Rewriters)
                {
                    foreach (DirectiveNode directive in type.Directives)
                    {
                        rewriter.Inspect(directive, context);
                    }
                }
            }

            if (_visit.Contains(node.Kind))
            {
                return Continue;
            }

            return base.Enter(node, context);
        }

        protected override ISyntaxVisitorAction Leave(
            ISyntaxNode node,
            ISchemaRewriterContext context)
        {
            context.Path.Pop();
            return Continue;
        }
    }
}
