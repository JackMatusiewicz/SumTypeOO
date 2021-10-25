using System;

namespace SumTypeOO
{
    // A simple static class with a helper function to construct each case of the
    // sum type. This aids with type inference.
    public static class Expr
    {
        public static ExprValue<A> Value<A>(A v) =>
            new ExprValue<A>(v);

        public static ExprMap<A> Map<A, B>(Func<B, A> f, IExprElement<B> v) =>
            ExprMap<A>.Make<B>(f, v);

        public static ExprApply<A> Apply<A, B>(
            IExprElement<Func<B, A>> f,
            IExprElement<B> v) =>
            ExprApply<A>.Make<B>(f,v);
    }
}