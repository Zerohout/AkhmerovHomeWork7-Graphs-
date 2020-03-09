namespace AkhmerovHomeWork7
{
    using static FuncLibrary.FuncLibrary;
    using static System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            GetGraph(@"Graphs/graph.txt", out var graph);
            //GetGraph(@"Graphs/graph1.txt", out var graph);
            //GetGraph(@"Graphs/graph2.txt", out var graph);

            //BypassGraphInWidth(graph, 0);
            //BypassGraphInWidthRecursive(graph, new int[graph.GetLength(0)], new Stack<int>(graph.GetLength(0)), 0);
            //BypassGraphInDeep(graph,0);
            //BypassGraphInDeepRecursive(graph, new int[graph.GetLength(0)], new Queue<int>(graph.GetLength(0)), 0);

            //TreeSearchShortWay(graph);

            var cc = new CalculationComplexity();
            var weightMatrix = new int[graph.GetLength(0), graph.GetLength(1)];

            for (var i = 0; i < graph.GetLength(0); i++)
            {
                var weight = DijkstraSerchShortWay(graph, i, ref cc);

                for (var j = 0; j < weight.Length; j++)
                {
                    weightMatrix[i, j] = weight[j];
                    Write($"{weight[j]} ");
                }

                WriteLine();
            }

            WriteLine($"\nРазмер матрицы: {graph.GetLength(0)}x{graph.GetLength(1)}.");
            WriteLine($"Количество итераций алгоритма: {graph.GetLength(0)}.");
            PrintComplecityAlgorithm(cc);

            ReadKey();
        }
    }
}
