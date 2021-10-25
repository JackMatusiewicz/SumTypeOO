using System;

namespace SumTypeOO
{
    public interface IMapVisitor<A, R>
    {
        R Visit<B>(Func<B, A> f, IExprElement<B> v);
    }

    public interface IMap<A>
    {
        public R Accept<R>(IMapVisitor<A, R> visitor);
    }

    public abstract class ExprMap<A> : IExprElement<A>, IMap<A>
    {
        private sealed class MapImpl<B> : ExprMap<A>
        {
            private Func<B, A> _f;
            private IExprElement<B> _v;

            public MapImpl(
                Func<B, A> f,
                IExprElement<B> v)
            {
                _f = f;
                _v = v;
            }

            public override R Accept<R>(IExprVisitor<A, R> visitor) =>
                visitor.Visit(this);

            public override R Accept<R>(IMapVisitor<A, R> visitor) =>
                visitor.Visit(_f, _v);
        }

        public static ExprMap<A> Make<B>(
            Func<B,A> f,
            IExprElement<B> v) =>
            new MapImpl<B>(f, v);

        public virtual R Accept<R>(IExprVisitor<A, R> visitor) =>
            throw new Exception("Impossible");

        public abstract R Accept<R>(IMapVisitor<A, R> visitor);
    }
}