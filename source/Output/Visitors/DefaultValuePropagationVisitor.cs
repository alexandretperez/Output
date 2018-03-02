using Output.Extensions;
using System.Linq.Expressions;

namespace Output.Internals
{
    public class DefaultValuePropagationVisitor : ExpressionVisitor
    {
        private Expression GetDefaultValue(Expression node)
        {
            if (node.Type.IsNullable() || node.Type.IsClass())
                return Expression.Constant(null, node.Type);

            return Expression.Default(node.Type);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression == null)
                return node;

            var n = Visit(node.Expression);
            if (n == null || n.NodeType == ExpressionType.Parameter)
                return base.VisitMember(node);

            return Expression.Condition(
                Expression.NotEqual(n, GetDefaultValue(n)),
                node,
                Expression.Default(node.Type)
            );
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Object == null)
                return node;

            var n = Visit(node.Object);
            if (n == null || n.NodeType == ExpressionType.Parameter)
                return base.VisitMethodCall(node);

            return Expression.Condition(
                Expression.NotEqual(n, GetDefaultValue(n)),
                node,
                Expression.Default(node.Type)
            );
        }
    }
}