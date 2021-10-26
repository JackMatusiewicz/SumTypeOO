using System;

namespace SumTypeOO
{
    /// The interface that implements the catamorphism for the Expr sum type.
    public interface IExprCata<R>
    {
        public R VisitValue<A>(A value);

        public R VisitMap<A, B>(Func<B, A> func, R value);

        public R VisitApply<A, B>(R func, R value);
    }

    public static class ExprCata
    {
        internal static ExprCata<A, R> Make<A, R>(IExprCata<R> cata) =>
            new ExprCata<A, R>(cata);

        public static R Evaluate<A, R>(this IExprCata<R> cata, IExprElement<A> element)
        {
            var exprCata = Make<A, R>(cata);
            return element.Accept(exprCata);
        }
    }

    // The visitor that will recurse through the structure, applying the catamorphism.
    // N.B As with the evaluator, this isn't tail recursive like it would be in F#, so
    // it won't be feasible for larger structures.
    internal sealed record ExprCata<A, R>(IExprCata<R> Cata) : IExprVisitor<A, R>
    {
        private record ExprMapEvaluator(IExprCata<R> Cata)
            : IMapVisitor<A, R>
        {
            public R Visit<B>(Func<B, A> f, IExprElement<B> v)
            {
                var childCata = ExprCata.Make<B, R>(Cata);
                var b = v.Accept(childCata);
                return Cata.VisitMap(f, b);
            }
        }

        private record ExprApplyEvaluator(IExprCata<R> Cata)
            : IExprApplyVisitor<A, R>
        {
            public R Visit<B> (
                IExprElement<Func<B,A>> f,
                IExprElement<B> v)
                {
                    var func = f.Accept(ExprCata.Make<Func<B, A>, R>(Cata));
                    var value = v.Accept(ExprCata.Make<B, R>(Cata));
                    return Cata.VisitApply<A, B>(func, value);
                }
        }

        public R Visit(ExprValue<A> v) => Cata.VisitValue(v.Value);

        public R Visit(ExprMap<A> v) =>
            v.Accept(new ExprMapEvaluator(Cata));

        public R Visit(ExprApply<A> v) =>
            v.Accept(new ExprApplyEvaluator(Cata));
    }
}