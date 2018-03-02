using System.Linq.Expressions;

namespace Output.Internals
{
    public class ExpressionReplacerVisitor : ExpressionVisitor
    {
        private readonly Expression _original;
        private readonly Expression _replacer;

        public ExpressionReplacerVisitor(Expression original, Expression replacer)
        {
            _original = original;
            _replacer = replacer;
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit((_original == node) ? _replacer : node);
        }
    }
}