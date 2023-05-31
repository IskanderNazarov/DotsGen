using System.Collections.Generic;

public interface IFindable {
        List<int> FindPath();
        string FinderName();
        void Initialize(int[][] board, int startI, int startJ);
}