namespace SumTypeOO
{
    public interface IExprElement<A>
    {
        public R Accept<R>(IExprVisitor<A, R> visitor);
    }
}