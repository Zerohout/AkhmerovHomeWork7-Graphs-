namespace FuncLibrary
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using static System.Console;

    public class FuncLibrary
    {
        #region Подсчет сложности алгоритма

        /// <summary>
        /// Структура подсчета сложности алгоритма
        /// </summary>

        public struct CalculationComplexity
        {
            public int operations;
            public int repeat;
            public int middleNumVertex;
            public int square;
            public int cubic;
            public int tentative;
            public int generalOper;
            public int generalSquare;
            public int generalCubic;
            public int generalTentative;
        }

        /// <summary>
        /// Расчет сложности алгоритма
        /// </summary>
        /// <param name="cc">Структура подсчета сложности алгоритма</param>
        /// <param name="size">Размер матрицы смежности</param>
        /// <param name="print">Флаг вывода информации в консоль</param>
        public static void CalculatingComplexityAlgorithm(ref CalculationComplexity cc, int size, bool print)
        {
            cc.middleNumVertex = cc.middleNumVertex / size + 1;
            cc.square = (size - 1) * (size - 1);
            cc.cubic = (size - 1) * (size - 1) * (size - 1);
            cc.tentative = ((size) * (size - 1)) + (cc.repeat * cc.middleNumVertex);
            cc.generalOper += cc.operations;
            cc.generalSquare += cc.square;
            cc.generalCubic += cc.cubic;
            cc.generalTentative += cc.tentative;

            if (print)
            {
                WriteLine($"Размер матрицы {size}x{size}\nОпераций: {cc.operations}.\nКвадратная сложность: {cc.square}\nКубическая сложность: {cc.cubic}\nПримерная сложность алгоритма: {cc.tentative}\n");
            }
        }

        /// <summary>
        /// Вывод на консоль информации о подсчете сложности алгоритма
        /// </summary>
        /// <param name="cc">Структура подсчета сложности алгоритма</param>
        public static void PrintComplecityAlgorithm(CalculationComplexity cc)
        {
            WriteLine($"Общее количество операций (ОКП): {cc.generalOper}.");
            WriteLine($"ОКП при квадратичной сложности: {cc.generalSquare}.");
            WriteLine($"ОКП при кубической сложности: {cc.generalCubic}.");
            WriteLine($"ОКП при примерно рассчитанной сложности: {cc.generalTentative}");
            WriteLine("\nИтоговая примерная сложность алгоритма: O((N^2) - N + (R*V))\nГде R - количество повторных операций, а V среднее количество путей из вершины");
        }

        #endregion

        #region Поиск минимальных весов путей матрицы смежности

        /// <summary>
        /// Вспомогательная структура для поиска дерева коротких путей матрицы смежности
        /// </summary>
        struct TreeSearchHelper
        {
            public int vertexA;
            public int vertexB;
            public int min;
            public bool[] status;
            public bool start;
        }

        /// <summary>
        /// Дерево минимальных весов путей матрицы смежности
        /// </summary>
        /// <param name="graph">Матрица смежности</param>
        public static void TreeSearchShortWay(int[,] graph)
        {
            var tsh = new TreeSearchHelper
            {
                status = new bool[graph.GetLength(0)],
                start = false
            };

            for (var k = 0; k < graph.GetLength(0) - 1; k++)
            {
                tsh.min = int.MaxValue;
                for (var i = 0; i < graph.GetLength(0); i++)
                {
                    if (tsh.start && !tsh.status[i]) continue;

                    for (var j = 0; j < graph.GetLength(1); j++)
                    {
                        if (tsh.start && tsh.status[j]) continue;

                        if (i != j && graph[i, j] > 0 && graph[i, j] < tsh.min)
                        {
                            tsh.vertexA = i;
                            tsh.vertexB = j;
                            tsh.min = graph[i, j];
                        }
                    }
                }

                WriteLine($"{tsh.vertexA} --({tsh.min})-- {tsh.vertexB}");

                if (!tsh.start) tsh.start = true;
                if (!tsh.status[tsh.vertexA]) tsh.status[tsh.vertexA] = true;
                if (!tsh.status[tsh.vertexB]) tsh.status[tsh.vertexB] = true;
            }
        }

        /// <summary>
        /// Алгоритм Дейкстры по поиску минимальных весов путей вершин
        /// </summary>
        /// <param name="graph">Матрица смежности</param>
        /// <param name="i">Индекс начальной вершины</param>
        /// <param name="cc">Структура подсчета сложности алгоритма</param>
        /// <returns></returns>
        public static int[] DijkstraSerchShortWay(int[,] graph, int i, ref CalculationComplexity cc)
        {
            var size = graph.GetLength(0);
            var status = new int[size];
            var weight = CreateWayWeight(size);
            var queue = new Queue<int>();

            cc.operations = 0;
            cc.repeat = 0;
            cc.middleNumVertex = 0;

            status[i] = 1;
            weight[i] = 0;

            while (true)
            {
                //cc.operations++;
                var temp = 0;
                for (var j = 0; j < size; j++)
                {
                    cc.operations++;
                    if (graph[i, j] == 0) continue;
                    if (status[j] == 0) status[j] = 1;
                    temp++;
                    if (weight[i] >= weight[j]) continue;

                    if (weight[i] + graph[i, j] == weight[j])
                    {
                        if (status[j] == 2) status[j] = 1;
                        cc.operations++;

                        if (queue.Contains(j)) cc.repeat++;

                        queue.Enqueue(j);
                        continue;
                    }

                    if (weight[i] + graph[i, j] < weight[j])
                    {
                        if (queue.Contains(j)) cc.repeat++;

                        queue.Enqueue(j);
                        weight[j] = weight[i] + graph[i, j];
                        cc.operations++;
                    }
                }

                cc.middleNumVertex += temp;

                if (queue.Count > 0)
                {
                    if (!queue.Contains(i)) status[i] = 2;
                    i = queue.Dequeue();
                }
                else
                {
                    CalculatingComplexityAlgorithm(ref cc, size, false);
                    return weight;
                }
            }
        }

        /// <summary>
        /// Создание массива с весами вершин.
        /// </summary>
        /// <param name="size">размер матрицы смежности</param>
        /// <returns></returns>
        static int[] CreateWayWeight(int size)
        {
            var temp = new int[size];

            for (var i = 0; i < size; i++)
            {
                temp[i] = int.MaxValue;
            }

            return temp;
        }

        #endregion

        #region Вспомогательные методы для графов

        /// <summary>
        /// Чтение матрицы смежности из файла
        /// </summary>
        /// <param name="graphPath">Путь к матрице смежности</param>
        /// <returns></returns>
        public static void GetGraph(string graphPath, out int[,] graph)
        {
            try
            {
                int size;
                string[] nums;

                using (var sr = new StreamReader(graphPath))
                {
                    size = int.Parse(sr.ReadLine());
                    nums = sr.ReadToEnd().Split(new[] { ",", "\r", "\n", " " }, StringSplitOptions.RemoveEmptyEntries);
                }

                graph = new int[size, size];

                for (int i = 0, j = 0, k = 0; i < nums.Length; i++, k++)
                {
                    graph[j, k] = int.Parse(nums[i]);

                    if (k != size - 1) continue;

                    j++;
                    k = -1;
                }
            }
            catch (Exception e)
            {
                WriteLine(e);
                graph = null;
            }
        }

        /// <summary>
        /// Вывести двумерный массив с матрицой смежности на экран
        /// </summary>
        /// <param name="graph">двумерный массив с матрицой смежности</param>
        public static void PrintGraph(int[,] graph)
        {
            WriteLine();
            for (var i = 0; i < graph.GetLength(0); i++)
            {
                for (var j = 0; j < graph.GetLength(1); j++)
                {
                    Write($"{graph[i, j]} ");
                }
                WriteLine();
            }

            WriteLine();
        }

        #endregion

        #region Обходы матрицы смежности

        /// <summary>
        /// Обход матрицы смежности в ширину
        /// </summary>
        /// <param name="graph">Матрица смежности</param>
        /// <param name="i">Индекс начальной вершины</param>
        public static void BypassGraphInWidth(int[,] graph, int i)
        {
            var status = new int[graph.GetLength(0)];
            var stack = new Stack<int>();
            status[i] = 1;

            while (true)
            {
                for (var j = 0; j < graph.GetLength(0); j++)
                {
                    if (graph[i, j] == 0 || status[j] > 0) continue;

                    WriteLine($"{i} --> {j}");
                    stack.Push(j);
                    status[j] = 1;
                }

                status[i] = 2;

                if (stack.Count == 0) return;

                i = stack.Pop();
            }
        }

        /// <summary>
        /// Обход матрицы смежности в ширину рекурсивным методом
        /// </summary>
        /// <param name="graph">Матрица смежности</param>
        /// <param name="status">Массив статусов вершины</param>
        /// <param name="stack">Стек вершин</param>
        /// <param name="i">Индекс начальной вершины</param>
        public static void BypassGraphInWidthRecursive(int[,] graph, int[] status, Stack<int> stack, int i)
        {
            status[i] = 1;

            for (var j = 0; j < graph.GetLength(0); j++)
            {
                if (graph[i, j] == 0 || status[j] > 0) continue;

                WriteLine($"{i} --> {j}");
                stack.Push(j);
                status[j] = 1;
            }

            status[i] = 2;
            if (stack.Count > 0)
            {
                BypassGraphInWidthRecursive(graph, status, stack, stack.Pop());
            }
        }

        /// <summary>
        /// Обход матрицы смежности в глубину
        /// </summary>
        /// <param name="graph">Матрица смежности</param>
        /// <param name="i">Индекс начальной вершины</param>
        public static void BypassGraphInDeep(int[,] graph, int i)
        {
            var status = new int[graph.GetLength(0)];
            var queue = new Queue<int>();
            status[i] = 1;

            while (true)
            {
                for (var j = 0; j < graph.GetLength(0); j++)
                {
                    if (graph[i, j] == 0 || status[j] > 0) continue;

                    WriteLine($"{i} --> {j}");
                    queue.Enqueue(j);
                    status[j] = 1;
                }

                status[i] = 2;

                if (queue.Count == 0) return;

                i = queue.Dequeue();
            }
        }

        /// <summary>
        /// Обход матрицы смежности в ширину рекурсивным методом
        /// </summary>
        /// <param name="graph">Матрица смежности</param>
        /// <param name="status">Массив статусов вершины</param>
        /// <param name="queue">Стек вершин</param>
        /// <param name="i">Индекс начальной вершины</param>
        public static void BypassGraphInDeepRecursive(int[,] graph, int[] status, Queue<int> queue, int i)
        {
            status[i] = 1;

            for (var j = 0; j < graph.GetLength(0); j++)
            {
                if (graph[i, j] == 0 || status[j] > 0) continue;

                WriteLine($"{i} --> {j}");
                queue.Enqueue(j);
                status[j] = 1;
            }

            status[i] = 2;
            if (queue.Count > 0)
            {
                BypassGraphInDeepRecursive(graph, status, queue, queue.Dequeue());
            }
        }

        #endregion
    }
}
