namespace SumTypeOO
{
    public record ExprValue<A>(A Value) : IExprElement<A>
    {
        public R Accept<R>(IExprVisitor<A, R> visitor) =>
            visitor.Visit(this);
    }

    public static class ExprValue
    {
        public static ExprValue<A> Make<A>(A v) =>
            new ExprValue<A>(v);
    }
}