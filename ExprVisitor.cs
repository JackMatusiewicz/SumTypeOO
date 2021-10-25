namespace SumTypeOO
{
    public interface IExprVisitor<A, R>
    {
        R Visit(ExprValue<A> value);
        R Visit(ExprMap<A> value);
        R Visit(ExprApply<A> value);
    }
}