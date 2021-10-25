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
            Console.WriteLine("Hello World!");
        }
    }
}
