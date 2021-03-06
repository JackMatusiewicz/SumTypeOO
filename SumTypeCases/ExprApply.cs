using System;

namespace SumTypeOO
{
    // These two interfaces encode the existential type for the Apply case.
    // That is, for an ExprApply<A> there exists some type B such that we have
    // an IExprElement<Func<B,A>> and an IExprElement<B>.
    // However, we don't know what that type B is when we interact with it
    // so we must provide a universally quantified function.
    public interface IExprApplyVisitor<A, R>
    {
        R Visit<B>(
            IExprElement<Func<B, A>> f,
            IExprElement<B> v);
    }

    public interface IExprApply<A>
    {
        public R Accept<R>(IExprApplyVisitor<A, R> visitor);
    }

    public abstract record ExprApply<A> : IExprElement<A>, IExprApply<A>
    {
        // This inner type is required to hide the B from the outer type.
        // If we didn't do this then we'd have to know about B inside
        // the visitor.
        private record ApplyImpl<B>(
            IExprElement<Func<B, A>> F,
            IExprElement<B> V)
            : ExprApply<A>
        {
            public override R Accept<R>(
                IExprVisitor<A, R> visitor) =>
                visitor.Visit(this);

            public override R Accept<R>(
                IExprApplyVisitor<A, R> visitor) =>
                visitor.Visit(F, V);
        }

        public static ExprApply<A> Make<B>(
            IExprElement<Func<B,A>> f,
            IExprElement<B> v) =>
            new ApplyImpl<B>(f, v);

        public virtual R Accept<R>(IExprVisitor<A, R> visitor) =>
            throw new Exception("Unreachable");

        public abstract R Accept<R>(IExprApplyVisitor<A, R> visitor);
    }
}
