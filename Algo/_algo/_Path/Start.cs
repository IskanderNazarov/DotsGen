using System;

public class Start {
    private ComplexityData[] data = {
        new ComplexityData {levelsCount = 0, complexity = 1, vertexCount = new Pair(18, 19), boardSizes = new []{(7, 4)}}, //30-32, blocks 10, 7
        new ComplexityData {levelsCount = 0, complexity = 1, vertexCount = new Pair(21, 23), boardSizes = new []{(5, 6), (6, 5)}}, //30-32, blocks 10, 7
        new ComplexityData {levelsCount = 0, complexity = 1, vertexCount = new Pair(21, 23), boardSizes = new []{(8, 4)}}, //30-32, blocks 10, 7
        new ComplexityData {levelsCount = 0, complexity = 1, vertexCount = new Pair(27, 30), boardSizes = new []{(6, 7), (7, 6), (8, 5)}}, //42-40, blocks 13, 12, 15, 
        
        new ComplexityData {levelsCount = 0, complexity = 2, vertexCount = new Pair(40, 44), boardSizes = new []{(8, 7), (10, 6), (9, 7)}}, //56-60-63, 16, 12, 20, 23, 19 ...
        new ComplexityData {levelsCount = 0, complexity = 3, vertexCount = new Pair(52, 55), boardSizes = new []{(10, 7), (9, 8)}}, //70-72, blocks 13-15
        new ComplexityData {levelsCount = 51, complexity = 4, vertexCount = new Pair(56, 63), boardSizes = new []{(8, 10), (10, 8), (9, 9), (11, 8), (10, 9), (9, 10)}}, //80-81-88-90-99, blocks 20-22
        new ComplexityData {levelsCount = 0, complexity = 5, vertexCount = new Pair(64, 70), boardSizes = new []{(11, 9),(10, 10), (13, 8), (12, 9)}} //99-100-104-108, blocks 28-33
    };
    private int levelNumber = 449;
    
    //U2 F2 R2 U' L2 D B R' B R' B R' D' L2 U'.
    //U' L2 F2 D' L' D U2 R U' R' U2 R2 U F' L' U R'.
    //U L2 D F D' B' U L' B2 U2 F U' F' U2 B' U'
    
    public static void Main(string[] args) {
        
        IFindable findable = new HamiltonianPathInGraphListAdj();
        new Start().Generate(findable);
        
        
        /*var pf = new PathFinderTester();
        pf.Test();*/

        var t = new int[][] {
            new[] {1, 1, 2, 1, 1, 1},
            new[] {1, 1, 0, 1, 1, 1},
            new[] {0, 0, 1, 1, 1, 1},
            new[] {1, 1, 1, 1, 1, 1},
            new[] {1, 1, 0, 0, 1, 1},
            new[] {0, 0, 1, 1, 0, 1},
            new[] {1, 1, 1, 1, 1, 1},
        };
        /*var path = new HamiltonianPathInGraphListAdj(t, 5, 1).FindPath();
        Utils.PrintList(path);*/
        /*var b = new BoardGenerator();
        Console.WriteLine("HasAtLeast_2_DeadLockArea: " + b.HasAtLeast_2_DeadLockArea(t, 0, 2));*/
    }

    //---------------------------------------------------------------------------------

    private void Generate(IFindable findable) {
        var levelCreator = new LevelCreator();
        var totalTime = 0f;
        foreach (var d in data) {
            for (var i = 0; i < d.levelsCount; i++) {

                var level = levelCreator.GenerateLevel(findable, d.GetRandomBoardSize(), d.GetRandomVertexCount(), levelNumber);
                level.levelNumber = levelNumber;
                level.complexity = d.complexity;
                
                Utils.WriteLevelToFile(level.GenerateDataForFile(), levelNumber, d.complexity);
                levelNumber++;
            }
        }
        
        //DO NOT forget to save board database
        BoardsDatabase.shared.SaveToFile();
        
    }

    //---------------------------------------------------------------------------------

    
}