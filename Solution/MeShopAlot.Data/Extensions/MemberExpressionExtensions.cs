using System.Linq.Expressions;

namespace MeShopAlot.Data.Extensions
{
    public static class MemberExpressionExtensions
    {
        public static IEnumerable<MemberExpression> GetPropertyAccesses<T, TResult>(
            this Expression<Func<T, TResult>> expression)
        {
            var visitor = new MemberAccesses(expression.Parameters[0]);
            visitor.Visit(expression);
            return visitor.Members;
        }
    }

    internal class MemberAccesses : ExpressionVisitor
    {
        private readonly ParameterExpression parameter;
        public HashSet<MemberExpression> Members { get; private set; }
        public MemberAccesses(ParameterExpression parameter)
        {
            this.parameter = parameter;
            this.Members = new HashSet<MemberExpression>();
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression == parameter)
            {
                Members.Add(node);
            }
            return base.VisitMember(node);
        }
    }
}
