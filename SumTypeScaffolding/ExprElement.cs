namespace SumTypeOO
{
    // All cases of the SumType must implement this interface.
    public interface IExprElement<A>
    {
        public R Accept<R>(IExprVisitor<A, R> visitor);
    }
}