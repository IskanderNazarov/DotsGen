using System;
using System.Text;

public class Level {
    public int levelNumber;
    public int complexity;
    public Pair boardSize;
    public int startNumber;
    public int[] path;
    public int[] blocksPositions;
    public float timeForGeneration;


    public string GenerateDataForFile() {
        var sb = new StringBuilder();
        sb.Append(boardSize.a);
        sb.Append(' ');
        sb.Append(boardSize.b);
        sb.Append('\n');
        sb.Append(startNumber);
        sb.Append('\n');
        
        foreach (var p in path) {
            sb.Append(p);
            sb.Append(' ');
        }
        
        sb.Append('\n');
        sb.Append(CreateBoardString());
        sb.Append('\n');
        sb.Append("Complexity: ");
        sb.Append(complexity);
        sb.Append('\n');
        sb.Append(GetTimeString());
        
        //create matrix

        return sb.ToString();
    }

    public string GetTimeString() {
        return $"Time: {Math.Round(timeForGeneration), 2} seconds or {Math.Round(timeForGeneration / 60, 2)} minutes or {Math.Round(timeForGeneration / 3600, 2)} hours";
    }

    public override string ToString() {
        var p = "";
        foreach (var pp in path) {
            var i = pp / boardSize.b;
            var j = pp % boardSize.b;
            p += $"({i}, {j}) ";
        }


        


        var s = "";
        s += $"Level number: {levelNumber}\n" +
            $"BoardSize: {boardSize}\n" +
             $"Start number: {startNumber}\n" +
             $"Matrix: \n{CreateBoardString()}\n" +
             $"Path: {p}\n" +
            $"-----------------------------"
            ;
        return s;
    }

    private string CreateBoardString() {
        var b = new int[boardSize.a][];
        for (var i = 0; i < boardSize.a; i++) {
            b[i] = new int[boardSize.b];
            for (var j = 0; j < boardSize.b; j++) {
                b[i][j] = 1;
            }
        }

        var startI = startNumber / boardSize.b;
        var startJ = startNumber % boardSize.b;
        b[startI][startJ] = 2;
        foreach (var pp in blocksPositions) {
            var i = pp / boardSize.b;
            var j = pp % boardSize.b;
            b[i][j] = 0;
        }

        var matrix = "";
        for (var i = 0; i < boardSize.a; i++) {
            //b[i] = new int[boardSize.b];
            for (var j = 0; j < boardSize.b; j++) {
                matrix += b[i][j] + " ";
            }

            matrix += '\n';
        }
        
        return matrix;
    }
}