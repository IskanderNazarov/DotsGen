using System;
using System.Collections.Generic;
using System.Linq;

class HamiltonianPathListAdjWarnsdorf : IFindable {
    int m, n;
    int startV;
    public bool[] visited;
    private int pathLength;

    private List<int>[] adj;

    public void Initialize(int[][] board, int startI, int startJ) {
        m = board[0].Length;
        n = board.Length;
        startV = startI * m + startJ;
        visited = new bool[m * n];
        for (int i = 0; i < visited.Length; i++) {
            visited[i] = false;
        }


        var count = m * n;
        adj = new List<int>[count];
        for (var i = 0; i < adj.Length; i++) {
            adj[i] = new List<int>();
        }

        //creating graph adjacent matrix
        pathLength = 0;
        for (var i = 0; i < board.Length; i++) {
            for (var j = 0; j < board[i].Length; j++) {
                if (board[i][j] != 0 && j + 1 < board[0].Length && board[i][j + 1] != 0) {
                    var v = i * m + j;
                    var u = v + 1;
                    adj[v].Add(u);
                    adj[u].Add(v);
                }

                if (board[i][j] != 0 && i + 1 < board.Length && board[i + 1][j] != 0) {
                    var v = i * m + j;
                    var u = (i + 1) * m + j;
                    adj[v].Add(u);
                    adj[u].Add(v);
                }

                if (board[i][j] != 0) {
                    pathLength++;
                }
            }
        }
    }

    public string FinderName() {
        return "Warndorf";
    }
    
    public List<int> FindPath()
    {
        var path = new List<int>();
        visited[startV] = true;
        path.Add(startV);

        if (BacktrackingWithFC(startV, 1, path))
        {
            return path;
        }

        // No Hamiltonian path found, reset visited and path
        visited[startV] = false;
        path.Clear();
        return path;
    }

    private bool BacktrackingWithFC(int startV, int count, List<int> path)
    {
        if (count == pathLength)
        {
            return true;
        }


        foreach (var u in adj[startV])
        {
            if (!visited[u])
            {
                visited[u] = true;
                path.Add(u);

                var remainingLegalValues = ForwardChecking(u);

                if (remainingLegalValues.Count > 0)
                {
                    if (BacktrackingWithFC(u, count + 1, path))
                    {
                        return true;
                    }
                }

                visited[u] = false;
                path.RemoveAt(path.Count - 1);
                RestoreLegalValues(remainingLegalValues);
            }
        }

        return false;
    }

    private List<int> ForwardChecking(int vertex)
    {
        var remainingLegalValues = new List<int>();

        foreach (var neighbor in adj[vertex])
        {
            if (!visited[neighbor])
            {
                visited[neighbor] = true;
                remainingLegalValues.AddRange(adj[neighbor]);
                remainingLegalValues.RemoveAll(v => visited[v]);
                visited[neighbor] = false;
            }
        }

        return remainingLegalValues.Distinct().ToList();
    }

    private void RestoreLegalValues(List<int> legalValues)
    {
        foreach (var value in legalValues)
        {
            visited[value] = false;
        }
    }

}