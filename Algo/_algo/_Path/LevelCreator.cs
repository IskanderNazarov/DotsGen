using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class LevelCreator {

    public Level GenerateLevel(IFindable findable, Pair boardSize, int vertexCount, int levelNumber) {
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
            //var pathFinder = new HamiltonianPathListAdjWarnsdorf(b, startI, startJ);
            

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
            startNumber = startVertex.a * boardSize.b + startVertex.b,
            blocksPositions = blockz.ToArray(),
            timeForGeneration = sec
        };
        
        Console.WriteLine("Tested boards count: " + count);
        Console.WriteLine(level.GetTimeString());
        Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");


        return level;
    }

    private (int[][], int i, int j, List<int> blocks) GenerateValidBoard(Pair boardSize, int blocksCount) {
        (int[][] b, int startI, int startJ, List<int> blocks) t;
        int startV;
        bool isInDatabase;
        do {
            t = GenerateRandomBoard(boardSize, blocksCount);
            startV = t.startI * boardSize.b + t.startJ;
            isInDatabase = BoardsDatabase.shared.HasBoard(t.blocks, startV);

            if (!isInDatabase) {
                BoardsDatabase.shared.Add(t.blocks, startV);
            }
            
        } while (isInDatabase);
        return t;
    }

    //------------------------------------------------------------

    private (int[][], int i, int j, List<int> blocks) GenerateRandomBoard(Pair boardSize, int blocksCount) {
        //boardSize.a - lines (height)
        //boardSize.b - columns (width)
        var randList = new List<int>();
        for (var i = 0; i < boardSize.a * boardSize.b; i++) randList.Add(i);
        randList.Shuffle();

        //randomly get start position
        var startVertex = new Random().Next(0, boardSize.a * boardSize.b);
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