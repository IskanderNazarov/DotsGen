using System;
using System.Collections.Generic;

public class LevelCreator_Hard:LevelCreator {

    public override Level GenerateLevel(IFindable findable, Pair boardSize, int vertexCount, int levelNumber) {
        var before = DateTime.Now;
        
        var totalBlocksCount = boardSize.a * boardSize.b - vertexCount;
        var blocksCount_1 = totalBlocksCount / 2;
        var blocksCount_2 = totalBlocksCount - blocksCount_1;
        var board_1_size = new Pair(boardSize.a / 2, boardSize.b);
        var board_2_size = new Pair(boardSize.a - board_1_size.a, boardSize.b);

        //rand startV between indices on the  last line of the board (random on the bottom line)
        var startV_1 = GetRandomStartVertex((board_1_size.a - 1) * board_1_size.b, board_1_size.a * board_1_size.b - 1);
        var level_1 = GenerateLocalLevel(findable, board_1_size, blocksCount_1, startV_1);
        
        var startV_2 = GetRandomStartVertex(0, board_2_size.b - 1);
        var level_2 = GenerateLocalLevel(findable, board_2_size, blocksCount_2, startV_2);


        var level = new Level {
            boardSize = boardSize,
            levelNumber = levelNumber,
            startVertex = level_1.startVertex,

        };
        
        

        var after = DateTime.Now;

        return null;
    }

    private Level GenerateLocalLevel(IFindable findable, Pair boardSize, int blocksCount, int startV) {

        List<int> path;
        List<int> blockz;
        Pair startVertex;
        do {
            var (b, startI, startJ, blocks) = GenerateValidBoard(boardSize, blocksCount, startV);
            blockz = blocks;
            startVertex = new Pair(startI, startJ);

            findable.Initialize(b, startI, startJ);
            path = findable.FindPath();

        } while (path.Count == 0 /*&& count < 10*/);



        var level = new Level {
            path = path.ToArray(), //ConvertPairsToIndices(path, boardSize).ToArray(),
            boardSize = boardSize,
            startVertex = startVertex.a * boardSize.b + startVertex.b,
            blocksPositions = blockz.ToArray(),
        };


        return level;
    }
    
    protected (int[][], int i, int j, List<int> blocks) GenerateValidBoard(Pair boardSize, int blocksCount, int startV) {
        (int[][] b, int startI, int startJ, List<int> blocks) t;
        bool isInDatabase;
        do {
            t = GenerateRandomBoard(boardSize, blocksCount, startV);
            isInDatabase = BoardsDatabase.shared.HasBoard(t.blocks, startV);

            if (!isInDatabase) {
                BoardsDatabase.shared.Add(t.blocks, startV);
            }
            
        } while (isInDatabase);
        return t;
    }

  
}