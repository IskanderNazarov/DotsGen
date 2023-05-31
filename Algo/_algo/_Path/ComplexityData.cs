using System;

public class ComplexityData {

    public int levelsCount;
    public int complexity;
    public Pair vertexCount;
    public (int n, int m)[] boardSizes;

    public ComplexityData() {
        
    }

    public int GetRandomVertexCount() {
        return new Random().Next(vertexCount.a, vertexCount.b + 1);
    }
    
    public Pair GetRandomBoardSize() {
        var (n, m) = boardSizes[new Random().Next(0, boardSizes.Length)];
        return new Pair(n, m);
    }

    /*public Pair GetRandomBoardSize() {
        var r = new Random();
        return new Pair(r.Next(minBoardSize.a, maxBoardSize.a + 1), r.Next(minBoardSize.b, maxBoardSize.b + 1));
    }*/

    /*public Pair GetBoardSizeByVerticesCount(int verticesCount) {
        var t = (verticesCount - (float)vertexCount) / verticesCount;
        return Utils.Lerp(minBoardSize, maxBoardSize, t);
    }*/
    
}
    
        

    