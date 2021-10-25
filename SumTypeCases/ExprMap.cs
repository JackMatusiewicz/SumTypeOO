using System;

namespace SumTypeOO
{
    // These two interfaces encode the existential type for the Map case.
    // That is, for an ExprMap<A> there exists some type B such that we have
    // a Func<B,A> and an IExprElement<B>.
    // However, we don't know what that type B is when we interact with it
    // so we must provide a universally quantified function.
    public interface IMapVisitor<A, R>
    {
        R Visit<B>(Func<B, A> f, IExprElement<B> v);
    }

    public interface IMap<A>
    {
        public R Accept<R>(IMapVisitor<A, R> visitor);
    }

    public abstract record ExprMap<A> : IExprElement<A>, IMap<A>
    {
        private sealed record MapImpl<B>(
            Func<B,A> F,
            IExprElement<B> V)
            : ExprMap<A>
        {
            public override R Accept<R>(IExprVisitor<A, R> visitor) =>
                visitor.Visit(this);

            public override R Accept<R>(IMapVisitor<A, R> visitor) =>
                visitor.Visit(F, V);
        }

        public static ExprMap<A> Make<B>(
            Func<B,A> f,
            IExprElement<B> v) =>
            new MapImpl<B>(f, v);

        public virtual R Accept<R>(IExprVisitor<A, R> visitor) =>
            throw new Exception("Unreachable");

        public abstract R Accept<R>(IMapVisitor<A, R> visitor);
    }
}
