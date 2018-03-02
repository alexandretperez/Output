using Output.Extensions;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Output.Visitors
{
    public class NullCheckVisitor : ExpressionVisitor
    {
        private readonly Expression _expression;
        private Expression _condition;

        public NullCheckVisitor(Expression expression)
        {
            _expression = expression;
            _condition = Expression.Constant(false);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var n = node.Expression;
            if (n == null)
                return node;

            if (n.NodeType == ExpressionType.Parameter)
                return node;

            _condition = OrElse(n);
            Visit(n);
            return n;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var n = node.Object;
            if (n == null)
                return node;

            _condition = OrElse(n);
            Visit(n);
            return n;
        }

        private Expression OrElse(Expression node)
        {
            return Expression.OrElse(
                Expression.Equal(node, GetDefaultValue(node)),
                _condition
            );
        }

        public Expression Visit()
        {
            base.Visit(_expression);

            return _condition.NodeType == ExpressionType.Constant
                ? _expression
                : Expression.Condition(_condition, GetDefaultValue(_expression), _expression);
        }

        private Expression GetDefaultValue(Expression node)
        {
            if (node.Type.GetTypeInfo().IsValueType && !node.Type.IsNullable())
                return Expression.Constant(Activator.CreateInstance(node.Type));

            return node.Type.IsClass()
                ? Expression.Constant(null, node.Type)
                : Expression.Default(node.Type) as Expression;
        }
    }
}