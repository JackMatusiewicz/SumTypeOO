using System;

namespace SumTypeOO
{
    // Writing operations for the sum type requires implementing the visitor to operate on each
    // case.
    public sealed class ExprEvaluator<A> : IExprVisitor<A, A>
    {
        private record ExprMapEvaluator()
            : IMapVisitor<A, A>
        {
            // TODO - could cache based on type rather than repeatedly making objects.
            public A Visit<B>(Func<B, A> f, IExprElement<B> v)
            {
                var childEvaluator = new ExprEvaluator<B>();
                var b = v.Accept(childEvaluator);
                return f(b);
            }
        }

        private record ExprApplyEvaluator()
            : IExprApplyVisitor<A, A>
        {
            public A Visit<B> (
                IExprElement<Func<B,A>> f,
                IExprElement<B> v)
                {
                    var func = f.Accept(new ExprEvaluator<Func<B, A>>());
                    var value = v.Accept(new ExprEvaluator<B>());
                    return func(value);
                }
        }

        public A Visit(ExprValue<A> v) => v.Value;

        public A Visit(ExprMap<A> v) =>
            v.Accept(new ExprMapEvaluator());

        public A Visit(ExprApply<A> v) =>
            v.Accept(new ExprApplyEvaluator());
    }
}