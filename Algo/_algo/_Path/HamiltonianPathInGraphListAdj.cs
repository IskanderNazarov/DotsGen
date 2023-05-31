using System;
using System.Collections.Generic;

class HamiltonianPathInGraphListAdj : IFindable {
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


        //sort lists
        var countArr = new int[adj.Length];
        for (var i = 0; i < adj.Length; i++) {
            var list = adj[i];
            countArr[i] = list.Count;
        }

        foreach (var list in adj) {
            list.Sort(delegate(int o1, int o2) {
                if (countArr[o1] < countArr[o2]) return 1;
                if (countArr[o1] > countArr[o2]) return -1;
                return 0;
            });
        }
    }

    public string FinderName() {
        return "Vertices lists  DFS";
    }

    public List<int> FindPath() {
        var path = new List<int>();
        DFS(startV, 1, path);
        return path;
    }

    private bool DFS(int startV, int count, List<int> path) {
        visited[startV] = true;
        path.Add(startV);
        if (count == pathLength) {
            return true;
        }

        foreach (var u in adj[startV]) {
            if (!visited[u]) {
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