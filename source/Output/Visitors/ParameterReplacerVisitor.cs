using System.Linq.Expressions;

namespace Output.Internals
{
    public class ParameterReplacerVisitor : ExpressionVisitor
    {
        private readonly Expression _parameterReplacer;

        public ParameterReplacerVisitor(Expression parameterReplacer)
        {
            _parameterReplacer = parameterReplacer;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _parameterReplacer;
        }
    }
}