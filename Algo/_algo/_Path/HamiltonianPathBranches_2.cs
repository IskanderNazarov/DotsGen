using System.Collections.Generic;
using System.Linq;

public class HamiltonianPathBranches_2 : IFindable {
    int startV;
    int m, n;
    public bool[] visited;
    private int pathLength;
    private int[][] graphph;

    public void Initialize(int[][] board, int startI, int startJ) {
        m = board[0].Length;
        n = board.Length;
        startV = startI * m + startJ;
        visited = new bool[m * n];
        for (int i = 0; i < visited.Length; i++) {
            visited[i] = false;
        }


        var count = m * n;
        graphph = new int[count][];
        for (var i = 0; i < graphph.Length; i++) {
            graphph[i] = new int[count];
        }

        //creating graph adjacent matrix
        for (var i = 0; i < board.Length; i++) {
            for (var j = 0; j < board[i].Length; j++) {
                if (board[i][j] != 1 && j + 1 < board[0].Length && board[i][j + 1] != 1) {
                    var v = i * m + j;
                    var u = v + 1;
                    graphph[v][u] = 1;
                    graphph[u][v] = 1;
                }

                if (board[i][j] != 1 && i + 1 < board.Length && board[i + 1][j] != 1) {
                    var v = i * m + j;
                    var u = (i + 1) * m + j;
                    graphph[v][u] = 1;
                    graphph[u][v] = 1;
                }

                if (board[i][j] != 1) {
                    pathLength++;
                }
            }
        }
    }


    public List<int> FindPath() {
        var p = FindHamiltonianPath(graphph);
        return new List<int>(p);
    }

    public string FinderName() {
        return "Branches and bounds";
    }


    public static int[] FindHamiltonianPath(int[][] graph) {
        int N = graph.Length;
        int[] path = new int[N];
        bool[] visited = new bool[N];
        for (int i = 0; i < N; i++) {
            path[i] = -1;
            visited[i] = false;
        }

        path[0] = 0;
        visited[0] = true;
        int level = 1;
        int[] bestPath = null;
        int minCost = int.MaxValue;
        BranchAndBound(graph, path, visited, level, ref bestPath, ref minCost);
        return bestPath;
    }

    private static void BranchAndBound(int[][] graph, int[] path, bool[] visited, int level, ref int[] bestPath, ref int minCost) {
        int n = graph.Length;
        if (level == n) {
            int cost = GetPathCost(graph, path);
            if (cost < minCost) {
                minCost = cost;
                bestPath = path.ToArray();
            }

            return;
        }

        int currentNode = path[level - 1];
        for (int i = 0; i < n; i++) {
            if (graph[currentNode][i] != 0 && !visited[i]) {
                int[] newPath = path.ToArray();
                newPath[level] = i;
                bool[] newVisited = visited.ToArray();
                newVisited[i] = true;
                int lowerBound = GetLowerBound(graph, newPath, newVisited);
                if (lowerBound < minCost) {
                    BranchAndBound(graph, newPath, newVisited, level + 1, ref bestPath, ref minCost);
                }
            }
        }
    }

    private static int GetLowerBound(int[][] graph, int[] path, bool[] visited) {
        int n = graph.Length;
        int lowerBound = GetPathCost(graph, path);
        for (int i = 0; i < n; i++) {
            if (!visited[i]) {
                int minCost = int.MaxValue;
                for (int j = 0; j < n; j++) {
                    if (visited[j] && graph[j][i] != 0 && (minCost == int.MaxValue || graph[j][i] < minCost)) {
                        minCost = graph[j][i];
                    }
                }

                if (minCost != int.MaxValue) {
                    lowerBound += minCost;
                }
            }
        }

        return lowerBound;
    }

    private static int GetPathCost(int[][] graph, int[] path) {
        int cost = 0;
        int n = graph.Length;
        for (int i = 1; i < n; i++) {
            int u = path[i - 1];
            int v = path[i];
            cost += graph[u][v];
        }

        return cost;
    }
}