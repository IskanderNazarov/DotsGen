using System;
using System.Collections.Generic;

public class LevelCreator {

    public virtual Level GenerateLevel(IFindable findable, Pair boardSize, int vertexCount, int levelNumber) {
        var blocksCount = boardSize.a * boardSize.b - vertexCount;
        Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
        Console.WriteLine("Start time: " + DateTime.Now);
        Console.WriteLine("Level number: " + levelNumber);
        Console.WriteLine("BoardSize: " + boardSize);
        Console.WriteLine("AllCells: " + (boardSize.a * boardSize.b));
        Console.WriteLine("Vertex count: " + vertexCount);
        Console.WriteLine("Blocks count: " + blocksCount);

        var before = DateTime.Now;
        List<int> path = null;
        List<int> blockz = null;
        Pair startVertex;
        var count = 0;
        var timeForBoardGeneration = 0d;
        do {
            var bf = DateTime.Now;
            var (b, startI, startJ, blocks) = GenerateValidBoard(boardSize, blocksCount);
            var af = DateTime.Now;
            timeForBoardGeneration += (af - bf).TotalSeconds;
            
            blockz = blocks;
            startVertex = new Pair(startI, startJ);
            

            findable.Initialize(b, startI, startJ);
            path = findable.FindPath();
            count++;

        } while (path.Count == 0 /*&& count < 10*/);

        var after = DateTime.Now;
        var sec = (float)(after - before).TotalSeconds;
        Console.WriteLine("Time for board generation: " + Math.Round(timeForBoardGeneration, 1));



        var level = new Level {
            path = path.ToArray(), //ConvertPairsToIndices(path, boardSize).ToArray(),
            boardSize = boardSize,
            startVertex = startVertex.a * boardSize.b + startVertex.b,
            blocksPositions = blockz.ToArray(),
            timeForGeneration = sec
        };
        
        Console.WriteLine("Tested boards count: " + count);
        Console.WriteLine(level.GetTimeString());
        Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");


        return level;
    }

    protected virtual (int[][], int i, int j, List<int> blocks) GenerateValidBoard(Pair boardSize, int blocksCount) {
        (int[][] b, int startI, int startJ, List<int> blocks) t;
        int startV;
        bool isInDatabase;
        do {
            //startV = t.startI * boardSize.b + t.startJ;
            startV = GetRandomStartVertex(0, boardSize.a * boardSize.b - 1);
            t = GenerateRandomBoard(boardSize, blocksCount, startV);
            isInDatabase = BoardsDatabase.shared.HasBoard(t.blocks, startV);

            if (!isInDatabase) {
                BoardsDatabase.shared.Add(t.blocks, startV);
            }
            
        } while (isInDatabase);
        return t;
    }

    //------------------------------------------------------------

    protected virtual (int[][], int i, int j, List<int> blocks) GenerateRandomBoard(Pair boardSize, int blocksCount, int startVertex) {
        //boardSize.a - lines (height)
        //boardSize.b - columns (width)

        //randomly get start position
        var start_i = startVertex / boardSize.b;
        var start_j = startVertex % boardSize.b;


        var board = new BoardGenerator().GenerateBoard(boardSize, blocksCount, startVertex);
        var blocks = new List<int>();


        for (var i = 0; i < board.Length; i++) {
            for (var j = 0; j < board[i].Length; j++) {
                var v = board[i][j];
                if (v == 0) {
                    blocks.Add(i * board[i].Length + j);
                }
            }
        }


        return (board, start_i, start_j, blocks);
    }

    //------------------------------------------------------------

    /**
     * Generate a number in the range of [valueStart, valueEnd] both inclusive
     */
    protected virtual int GetRandomStartVertex(int valueStart, int valueEnd) {
        return new Random().Next(valueStart, valueEnd + 1);
    }


    //------------------------------------------------------------

    private static List<int> ConvertPairsToIndices(List<Pair> pairs, Pair boardSize) {
        var indices = new List<int>();

        foreach (var p in pairs) {
            var i = p.a;
            var j = p.b;
            indices.Add(i * boardSize.b + j);
        }

        return indices;
    }
}