using System;
using System.Collections.Generic;
using System.Linq;

namespace SumTypeOO
{
    public record Node(int Id, string Value);

    public sealed record Edge(Node Left, Node Right);

    public sealed record Digraph(HashSet<Node> Nodes, HashSet<Edge> Edges)
    {
        public string ToDotFileContents()
        {
            var nodes = Nodes.Select(n => $"{n.Id} [label = \"{n.Value}\"]");
            var edges = Edges.Select(e => $"{e.Left.Id} -> {e.Right.Id}");

            return $"strict digraph {{ {string.Join("\n", nodes)} \n\n {string.Join("\n", edges)} }}";
        }

        public Digraph Merge(Digraph other)
        {
            Nodes.UnionWith(other.Nodes);
            Edges.UnionWith(other.Edges);
            return new Digraph(Nodes, Edges);
        }
    }

    public sealed class GraphVizGenerator : IExprCata<(Digraph, Node)>
    {
        int NodeIdCounter = 0;
        public (Digraph, Node) VisitValue<A>(A Value)
        {
            var node = new Node(NodeIdCounter++, Value.ToString());

            return
                (new Digraph(
                    new HashSet<Node> { node },
                    new HashSet<Edge>()),
                node);
        }

        public (Digraph, Node) VisitMap<A, B>(
            Func<B, A> func,
            (Digraph, Node) value)
        {
            var functionNode =
                new Node(
                    NodeIdCounter++,
                    $"{typeof(B).ToString()} -> {typeof(A).ToString()}");

            var resultNode =
                new Node(
                    NodeIdCounter++,
                    $"{typeof(A).ToString()}");
            var edge = new Edge(functionNode, resultNode);
            var edge2 = new Edge(value.Item2, resultNode);

            value.Item1.Nodes.Add(functionNode);
            value.Item1.Nodes.Add(resultNode);
            value.Item1.Edges.Add(edge);
            value.Item1.Edges.Add(edge2);

            return
                (new Digraph(value.Item1.Nodes, value.Item1.Edges),
                resultNode);
        }

        public (Digraph, Node) VisitApply<A, B>(
            (Digraph, Node) funcRet,
            (Digraph, Node) valueRet)
        {
            var resultNode =
                new Node(
                    NodeIdCounter++,
                    $"{typeof(A).ToString()}");
            var edge = new Edge(funcRet.Item2, resultNode);
            var edge2 = new Edge(valueRet.Item2, resultNode);

            var graph = funcRet.Item1.Merge(valueRet.Item1);

            graph.Nodes.Add(resultNode);
            graph.Edges.Add(edge);
            graph.Edges.Add(edge2);

            return (graph, resultNode);
        }
    }

}