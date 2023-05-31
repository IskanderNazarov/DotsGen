using System.Collections.Generic;

class HamiltonianPathInGraph:IFindable {
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
    }

    public List<int> FindPath() {
        var path = new List<int>();
        DFS(startV, 1, path);
        return path;
    }
    
    public string FinderName() {
        return "Adj matrix";
    }

    private bool DFS(int startV, int count, List<int> path) {
        visited[startV] = true;
        path.Add(startV);
        if (count == pathLength) {
            return true;
        }
        
        for (var u = 0; u < adjMatrix[startV].Length; u++) {
            //Console.WriteLine($"visited[{u}] = {visited[u]}");
            if (!visited[u] && adjMatrix[startV][u] == 1) {
                if (DFS(u, count + 1, path)) {
                    return true;
                }
            }
        }
        
        visited[startV] = false;
        path.RemoveAt(path.Count - 1);
        
        return false;
    }
}