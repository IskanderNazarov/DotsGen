using System;
using System.Collections.Generic;

public class BoardGenerator {
    //------------------------------

    public int[][] GenerateBoard(Pair boardSize, int blocksCount, int startVertex) {
        var b = new int[boardSize.a][];
        for (var i = 0; i < b.Length; i++) {
            b[i] = new int[boardSize.b];
            for (var j = 0; j < b[i].Length; j++) {
                b[i][j] = 1;
            }
        }
        

        b[startVertex / boardSize.b][startVertex % boardSize.b] = 2;
        

        //first get on edge place tiles indices
        var edgeIndices = GetEdgeIndices(b);
        edgeIndices.Shuffle();
        


        var innerIndices = GetInnerBlocksIndices(b, edgeIndices);
        innerIndices.Shuffle();

        
        var edgeBlocksCount = (int) (blocksCount * (0.2f + new Random().NextDouble() * 0.2f));
        var innerBlocksCount = blocksCount - edgeBlocksCount;

        //first place edge indices...
        PlaceBlocks(edgeBlocksCount, boardSize, edgeIndices, b);
        
        //then place inner indices...
        PlaceBlocks(innerBlocksCount, boardSize, innerIndices, b);
        
        //todo save board to database
        
        
        
        
        return b;
    }

    //------------------------------

    private void PlaceBlocks(int blocksCount, Pair boardSize, List<int> availableIndices, int[][] b) {
        for (var k = 0; k < blocksCount; k++) {
            foreach (var edgeIndex in availableIndices) {
                var i = edgeIndex / boardSize.b;
                var j = edgeIndex % boardSize.b;

                if (b[i][j] == 0 || b[i][j] == 2) {
                    continue; //if this index is already occupied by a block
                }

                b[i][j] = 0;
                var isNotValid = !IsBoardValid(b);
                if (isNotValid) {
                    //this is not valid place for the block, let's find another
                    b[i][j] = 1;
                }
                else {
                    //ok, we've found a place fot the block
                    break;
                }
            }
        }

        //Utils.PrintBoard(b);
    }

    //------------------------------

    public bool IsBoardValid(int[][] b) {
        return !HasIsolatedDots(b) &&
               !HasMoreThan_1_SurroundedDot(b) && !HasIsolatedAreas(b) && !HasBlocksAreas(b) && !HasIsolatedAreaByStartVertex(b);
               //&& !HasMany1x2Blocks(b);//only for small levels
    }
    
    //------------------------------

    public bool HasIsolatedDots(int[][] board) {
        for (var i = 0; i < board.Length; i++) {
            for (var j = 0; j < board[i].Length; j++) {
                if (board[i][j] != 0 &&
                    (i - 1 < 0 || board[i - 1][j] == 0) &&
                    (i + 1 >= board.Length || board[i + 1][j] == 0) &&
                    (j - 1 < 0 || board[i][j - 1] == 0) &&
                    (j + 1 >= board[0].Length || board[i][j + 1] == 0)) {
                    return true; //fully isolated dot is found
                }
            }
        }

        return false;
    }

    //------------------------------
    
    public bool HasIsolatedAreaByStartVertex(int[][] board) {
        
        /*
         3 3 2 1 1
         3 0 1 1 1
         0 1 1 1 1
         1 1 1 1 1
         */
        //333 - is isolated area by 2


        var startI = -1;
        var startJ = -1;
        for (var i = 0; i < board.Length; i++) {
            for (var j = 0; j < board[i].Length; j++) {
                if (board[i][j] == 2) {
                    startI = i;
                    startJ = j;
                    break;
                }
            }

            if (startI > 0) {
                break;
            }
        }

        board[startI][startJ] = 0;
        var hasIsolatedArea = HasIsolatedAreas(board);
        board[startI][startJ] = 2;

        return hasIsolatedArea;
    }
    //------------------------------

    public bool HasMoreThan_1_SurroundedDot(int[][] board) {
        //find surrounded from 3 sides dots count
        var surroundedCount = 0;
        for (var i = 0; i < board.Length; i++) {
            for (var j = 0; j < board[i].Length; j++) {
                if (board[i][j] == 0) continue;
                var closedSidesCount = 0;
                if (i - 1 < 0 || board[i - 1][j] == 0) closedSidesCount++;
                if (i + 1 >= board.Length || board[i + 1][j] == 0) closedSidesCount++;
                if (j - 1 < 0 || board[i][j - 1] == 0) closedSidesCount++;
                if (j + 1 >= board[0].Length || board[i][j + 1] == 0) closedSidesCount++;

                if (closedSidesCount >= 3 && board[i][j] != 2) {
                    // startVertex (2) can be surrounded
                    surroundedCount++;
                    if (surroundedCount > 1) {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    //------------------------------

    private bool[] visited;

    public bool HasIsolatedAreas(int[][] b) {
        visited = new bool[b.Length * b[0].Length];

        var areasCounter = 0;
        var w = b[0].Length;
        for (var i = 0; i < b.Length; i++) {
            for (var j = 0; j < b[i].Length; j++) {
                if (b[i][j] != 0 && !visited[i * w + j]) {
                    var area = new List<int>();
                    StartBFS(b, i * w + j, area, new Queue<int>(), false);
                    areasCounter++;
                }
            }
        }


        return areasCounter > 1;
    }

    //------------------------------

    private void StartBFS(int[][] b, int startV, List<int> area, Queue<int> queue, bool searchingBlocks) {
        var w = b[0].Length;
        var i = startV / w;
        var j = startV % w;

//if visited - do nothing
        if (visited[startV]) return;

        area.Add(startV);
        visited[startV] = true;


        //if (i - 1 >= 0 && b[i - 1][j] == 0) {
        if (i - 1 >= 0 && (searchingBlocks ? b[i - 1][j] == 0 : b[i - 1][j] != 0)) {
            queue.Enqueue((i - 1) * w + j);
        }

        if (i + 1 < b.Length && (searchingBlocks ? b[i + 1][j] == 0 : b[i + 1][j] != 0)) {
            queue.Enqueue((i + 1) * w + j);
        }

        if (j - 1 >= 0 && (searchingBlocks ? b[i][j - 1] == 0 : b[i][j - 1] != 0)) {
            queue.Enqueue(i * w + j - 1);
        }

        if (j + 1 < b[0].Length && (searchingBlocks ? b[i][j + 1] == 0 : b[i][j + 1] != 0)) {
            queue.Enqueue(i * w + j + 1);
        }


        while (queue.Count != 0) {
            StartBFS(b, queue.Dequeue(), area, queue, searchingBlocks);
        }
    }

    //------------------------------

    private List<int> GetEdgeIndices(int[][] b) {
        var edgeIndices = new List<int>();

        var w = b[0].Length;
        var edgeJ = w - 1;

        //left and right
        for (var i = 0; i < b.Length; i++) {
            edgeIndices.Add(i * w);
            edgeIndices.Add(i * w + edgeJ);
        }

        //top and bottom
        for (var j = 0; j < w; j++) {
            edgeIndices.Add(j);
            edgeIndices.Add((b.Length - 1) * w + j);
        }

        return edgeIndices;
    }

    //---------------

    public bool HasBlocksAreas(int[][] b) {
        visited = new bool[b.Length * b[0].Length];

        var w = b[0].Length;
        for (var i = 0; i < b.Length; i++) {
            for (var j = 0; j < b[i].Length; j++) {
                if (b[i][j] == 0 && !visited[i * w + j]) {
                    var area = new List<int>();
                    StartBFS(b, i * w + j, area, new Queue<int>(), true);
                    if (area.Count > 2) {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    //---------------
    
    public bool HasMany1x2Blocks(int[][] b) { //1x2 blocks are acceptable, but if there are many such blocks levels are not very
        visited = new bool[b.Length * b[0].Length];

        var areas = new List<List<int>>();
        var w = b[0].Length;
        var blocksInArea_1x2_Count = 0;
        var totalBlocksCount = 0;
        for (var i = 0; i < b.Length; i++) {
            for (var j = 0; j < b[i].Length; j++) {
                if (b[i][j] == 0 && !visited[i * w + j]) {
                    var area = new List<int>();
                    StartBFS(b, i * w + j, area, new Queue<int>(), true);
                    if (area.Count == 2) {
                        blocksInArea_1x2_Count += 2;
                        // return true;
                    }
                }
                
                if (b[i][j] == 0) {
                    totalBlocksCount++;
                }
            }
        }

        return blocksInArea_1x2_Count > totalBlocksCount * 0.5f;
    }
    
    //---------------

    private List<int> GetInnerBlocksIndices(int[][] b, List<int> edgeIndices) {
        var availableIndices = new List<int>();
        for (var i = 0; i < b.Length; i++) {
            for (var j = 0; j < b[0].Length; j++) {
                var index = i * b[i].Length + j;
                if (!edgeIndices.Contains(index)) {
                    availableIndices.Add(index);
                }
            }
        }

        return availableIndices;
    }

    //---------------
}