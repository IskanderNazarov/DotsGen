using System;
using System.Collections.Generic;

public class BranchAndBoundHamiltonianPath
{
    private int[][] adjMatrix;
    private int numVertices;
    private bool[] visited;
    private List<int> currentPath;
    private List<int> bestPath;
    private int currentCost;
    private int bestCost;
    private int startVertex;

    
    int m, n;
    int startV;
    private int pathLength;


    private const int M = 10000;
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


        Console.WriteLine("Adj matrix:");
        for (var i = 0; i < adjMatrix.Length; i++) {
            for (var j = 0; j < adjMatrix[i].Length; j++) {
                if (i != j && adjMatrix[i][j] == 0) {
                    //adding new edges with very large weights
                    adjMatrix[i][j] = 1;
                }
                
                Console.Write(adjMatrix[i][j] + " ");
            }

            Console.WriteLine();
        }
    }

    public void Initialize(int[][] adj, int startV) {
        this.startV = startV;
        adjMatrix = adj;
    }




    public List<int> FindPath()
    {
        numVertices = adjMatrix.Length;
        visited = new bool[numVertices];
        currentPath = new List<int>();
        bestPath = new List<int>();
        currentCost = 0;
        bestCost = int.MaxValue;
        startVertex = startV;

        // Start with the specified vertex
        currentPath.Add(startVertex);
        visited[startVertex] = true;

        BranchAndBound(startVertex);

        return bestPath;
    }

    private void BranchAndBound(int currentNode)
    {
        if (currentPath.Count == numVertices)
        {
            // Found a Hamiltonian path
            int lastVertex = currentPath[currentPath.Count - 1];
            if (adjMatrix[lastVertex][startVertex] != 0)
            {
                currentCost += adjMatrix[lastVertex][startVertex];
                if (currentCost < bestCost)
                {
                    // Update best path
                    bestCost = currentCost;
                    bestPath = new List<int>(currentPath);
                }
                currentCost -= adjMatrix[lastVertex][startVertex];
            }
            return;
        }

        for (int nextNode = 0; nextNode < numVertices; nextNode++)
        {
            if (!visited[nextNode] && adjMatrix[currentNode][nextNode] != 0)
            {
                currentPath.Add(nextNode);
                visited[nextNode] = true;
                currentCost += adjMatrix[currentNode][nextNode];

                if (currentCost < bestCost)
                {
                    BranchAndBound(nextNode);
                }

                currentPath.RemoveAt(currentPath.Count - 1);
                visited[nextNode] = false;
                currentCost -= adjMatrix[currentNode][nextNode];
            }
        }
    }
}




