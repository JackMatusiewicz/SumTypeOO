namespace SumTypeOO
{
    public record ExprValue<A>(A Value) : IExprElement<A>
    {
        public R Accept<R>(IExprVisitor<A, R> visitor) =>
            visitor.Visit(this);
    }
}