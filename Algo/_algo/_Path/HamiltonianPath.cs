using System;
using System.Collections.Generic;
using System.Linq;

class HamiltonianPath:IFindable {
    int[][] board;
    int n, m;
    int startI, startJ;
    int[] di = {0, 0, 1, -1};
    int[] dj = {1, -1, 0, 0};
    public bool[][] visited;
    private int pathLength;

    public void Initialize(int[][] board, int startI, int startJ) {
        this.board = board;
        this.n = board.Length;
        this.m = board[0].Length;
        this.startI = startI;
        this.startJ = startJ;
        this.visited = new bool[n][];
        for (int i = 0; i < n; i++) {
            this.visited[i] = new bool[m];
        }

        for (var i = 0; i < board.Length; i++) {
            for (var j = 0; j < board[i].Length; j++) {
                if (board[i][j] == 0 || board[i][j] == 2) {
                    pathLength++;
                }
            }
        }
    }


    public List<int> FindPath() {
        var path = new List<Pair>();
        DFS(startI, startJ, 1, path);

        var res = new List<int>();
        foreach (var pp in path) {
            res.Add(pp.a * m + pp.b);
        }
        
        return res;
    }

    public string FinderName() {
        return "Matrix searching";
    }

    private bool DFS(int i, int j, int count, List<Pair> path) {
        visited[i][j] = true;
        path.Add(new Pair(i, j));
        if (count == pathLength) {
            return true;
        }

        for (int k = 0; k < di.Length; k++) {
            var ni = i + di[k];
            var nj = j + dj[k];
            if (ni >= 0 && ni < n && nj >= 0 && nj < m && !visited[ni][nj] && board[ni][nj] == 0) {
                if (DFS(ni, nj, count + 1, path)) {
                    return true;
                }
            }
        }

        visited[i][j] = false;
        path.RemoveAt(path.Count - 1);
        return false;
    }
}