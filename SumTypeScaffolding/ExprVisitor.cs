namespace SumTypeOO
{
    // All cases of the SumType have a Visit method inside the visitor.
    public interface IExprVisitor<A, R>
    {
        R Visit(ExprValue<A> value);
        R Visit(ExprMap<A> value);
        R Visit(ExprApply<A> value);
    }
}