using System;

namespace SumTypeOO
{
    /*
        The premise of this application is to model the following algebra:

        type 'a Expr =
            | Value of 'a
            | Map of 'a Map
            | Apply of 'a Apply
        
        and 'a Map =
            abstract Accept<'r> : MapEval<'a, 'r> -> 'r
        and MapEval<'a, 'r> =
            abstract Eval<'b> : ('b -> 'a) -> 'b Expr -> 'r

        and 'a Apply =
            abstract Accept<'r> : ApplyEval<'a, 'r> -> 'r
        and ApplyEval<'a, 'r> =
            abstract Eval<'b> : ('b -> 'a) Expr -> 'b Expr -> 'r
    */

    class Program
    {
        static void Main(string[] args)
        {
            Func<int, Func<int, Func<int, int>>> func =
                a => b => c => a + b + c;

            var a = Expr.Value(2);
            var b = Expr.Map(func, a);
            var c = Expr.Apply(b, Expr.Value(3));
            var d = Expr.Apply(c, Expr.Value(4));
            var result = d.Accept(new ExprEvaluator<int>());
            Console.WriteLine(result);

            return;
        }
    }
}
