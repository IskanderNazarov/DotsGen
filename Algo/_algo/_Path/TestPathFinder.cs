using System;
using System.Collections.Generic;

public class Node {
    public int LowerBound { get; set; }
    public List<int> Path { get; set; }
}

public class TestPathFinder : IFindable {
    int m, n;
    int startV;
    public bool[] visited;
    private int pathLength;

    private int[][] adjMatrix;

    public void Initialize(int[][] board, int startI, int startJ) {
        m = board[0].Length;
        n = board.Length;
        startV = startI * m + startJ;
        visited = new bool[m * n];
        for (int i = 0; i < visited.Length; i++) {
            visited[i] = false;
        }


        var count = m * n;
        adjMatrix = new int[count][];
        for (var i = 0; i < adjMatrix.Length; i++) {
            adjMatrix[i] = new int[count];
        }


        //creating graph adjacent matrix
        for (var i = 0; i < board.Length; i++) {
            for (var j = 0; j < board[i].Length; j++) {
                if (board[i][j] != 1 && j + 1 < board[0].Length && board[i][j + 1] != 1) {
                    var v = i * m + j;
                    var u = v + 1;
                    adjMatrix[v][u] = 1;
                    adjMatrix[u][v] = 1;
                }

                if (board[i][j] != 1 && i + 1 < board.Length && board[i + 1][j] != 1) {
                    var v = i * m + j;
                    var u = (i + 1) * m + j;
                    adjMatrix[v][u] = 1;
                    adjMatrix[u][v] = 1;
                }

                if (board[i][j] != 1) {
                    pathLength++;
                }
            }
        }
        
        var s = "";
        foreach (var aa in adjMatrix) {
            foreach (var a in aa) {
                s += a + " ";
            }

            s += "\n";
        }
        
        Console.WriteLine("Adjacency matrix:\n" + s);
        
        
    }


    public List<int> FindPath() {
        return BranchAndBounds(adjMatrix);
    }

    public string FinderName() {
        return "Test finder";
    }


public class Node
{
    public int LowerBound { get; set; }
    public List<int> Path { get; set; }
}

public static List<int> BranchAndBounds(int[][] adjMatrix)
{
    int n = adjMatrix.GetLength(0);
    var pq = new PriorityQueue<Node>();
    pq.Enqueue(new Node { LowerBound = 0, Path = new List<int>() }); // (lower bound, path)
    List<int> bestPath = null;
    while (pq.Count > 0) {
        var d = pq.Dequeue();
        var (lowerBound, path) = (d.LowerBound, d.Path);
        if (path.Count == n)
        {
            bestPath = path;
            break;
        }
        int? lastVertex = path.Count > 0 ? path[path.Count-1] : (int?)null;
        for (int i = 0; i < n; i++)
        {
            if (!path.Contains(i) && adjMatrix[lastVertex.GetValueOrDefault()][ i] == 1)
            {
                var childPath = new List<int>(path) { i };
                var childBound = ComputeBound(childPath, adjMatrix);
                if (childBound < lowerBound)
                {
                    pq.Enqueue(new Node { LowerBound = childBound, Path = childPath });
                }
            }
        }
    }
    return bestPath;
}

public static int ComputeBound(List<int> path, int[][] adjMatrix)
{
    int n = adjMatrix.GetLength(0);
    var visited = new HashSet<int>(path);
    int bound = path.Count - 1;
    return bound;
}

public class PriorityQueue<T>
{
    private readonly List<T> _heap;
    private readonly IComparer<T> _comparer;

    public PriorityQueue(IComparer<T> comparer = null)
    {
        _heap = new List<T>();
        _comparer = comparer ?? Comparer<T>.Default;
    }

    public int Count => _heap.Count;

    public void Enqueue(T item)
    {
        _heap.Add(item);
        int i = _heap.Count - 1;
        while (i > 0)
        {
            int j = (i - 1) / 2;
            if (_comparer.Compare(_heap[j], item) <= 0) break;
            _heap[i] = _heap[j];
            i = j;
        }
        _heap[i] = item;
    }

    public T Dequeue()
    {
        int lastIndex = _heap.Count - 1;
        T root = _heap[0];
        _heap[0] = _heap[lastIndex];
        _heap.RemoveAt(lastIndex);
        lastIndex--;
        int i = 0;
        while (true)
        {
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            if (left > lastIndex) break;
            int minChild = left;
            if (right <= lastIndex && _comparer.Compare(_heap[right], _heap[left]) < 0) minChild = right;
            if (_comparer.Compare(_heap[i], _heap[minChild]) <= 0) break;
            T temp = _heap[i];
            _heap[i] = _heap[minChild];
            _heap[minChild] = temp;
            i = minChild;
        }
        return root;
    }
}

}