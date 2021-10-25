using System;

namespace SumTypeOO
{
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

    public abstract class ExprApply<A> : IExprElement<A>, IExprApply<A>
    {
        private class ApplyImpl<B> : ExprApply<A>
        {
            private IExprElement<Func<B,A>> _f;
            private IExprElement<B> _v;

            public ApplyImpl(
                IExprElement<Func<B,A>> f,
                IExprElement<B> v)
            {
                _f = f;
                _v = v;
            }

            public override R Accept<R>(
                IExprVisitor<A, R> visitor) =>
                visitor.Visit(this);

            public override R Accept<R>(
                IExprApplyVisitor<A, R> visitor) =>
                visitor.Visit(_f, _v);
        }

        public static ExprApply<A> Make<B>(
            IExprElement<Func<B,A>> f,
            IExprElement<B> v) =>
            new ApplyImpl<B>(f, v);

        public virtual R Accept<R>(IExprVisitor<A, R> visitor) =>
            throw new Exception("Unreachable");

        public abstract R Accept<R>(IExprApplyVisitor<A, R> visitor);
    }

    public static class ExprApply
    {
        public static ExprApply<A> Make<A, B>(
            IExprElement<Func<B, A>> f,
            IExprElement<B> v) =>
            ExprApply<A>.Make<B>(f,v);
    }
}