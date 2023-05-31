using System;

class PathFinderTester {
    /*int[][] board = new int[][] {
            new [] { 2, 0, 1, 1, 0, 0, 0},
            new [] { 1, 0, 1, 1, 1, 1, 1},
            new [] { 1, 0, 1, 0, 1, 1, 0},
            new [] { 1, 1, 1, 0, 1, 1, 1},
            new [] { 1, 1, 1, 0, 1, 1, 1},
            new [] { 1, 1, 1, 0, 1, 1, 0},
            new [] { 1, 1, 0, 0, 1, 1, 1},
            new [] { 1, 1, 0, 0, 1, 1, 1},
            new [] { 1, 1, 0, 0, 1, 1, 0},
            new [] { 1, 1, 0, 0, 1, 1, 0},
            new [] { 1, 1, 0, 0, 1, 1, 0},
            new [] { 1, 1, 0, 0, 1, 1, 0},
        };*/

    int[][] board = new int[][] {
        new[] {2, 1, 1, 1},
        new[] {0, 0, 1, 1},
        new[] {1, 0, 1, 1},
        new[] {1, 1, 1, 1},
        
    };
    
    int[][] adjMatrix = new int[][] {
        new[] {0, 1, 1, 1},
        new[] {1, 1, 1, 1},
        new[] {1, 1, 1, 1},
        new[] {1, 1, 1, 1},
        
    };

    public void Test() {
        //FindPath(new HamiltonianPath(board, startI, startJ));
        //FindPath(new HamiltonianPathInGraph(board, startI, startJ));
        //FindPath(new HamiltonianPathInGraphListAdj(board, startI, startJ));
        //FindPath(new HamiltonianPathBranches(board, startI, startJ));
        
        /*var solver = new HamiltonianPathInGraphListAdj_TryingImprove();
        solver.Initialize(board, 0, 0);
        //solver.Initialize(adjMatrix, 0);
        var p = solver.FindPath();
        Utils.PrintList(p);*/

        const int boardCount = 1500;
        var timer1 = 0d;
        var timer2 = 0d;
        DateTime before;
        DateTime after;
        for (var i = 0; i < boardCount; i++) {
            board = new BoardGenerator().GenerateBoard(new Pair(9, 9), 20, 0);

            before = DateTime.Now;
            IFindable f = new HamiltonianPathInGraphListAdj();
            f.Initialize(board, 0, 0);
            //FindPath(f);
            f.FindPath();
            after = DateTime.Now;
            timer1 += (after - before).TotalSeconds;

            
            before = DateTime.Now;
            f = new HamiltonianPathInGraphListAdj_TryingImprove();
            f.Initialize(board, 0, 0);
            //FindPath(f);
            f.FindPath();
            after = DateTime.Now;
            timer2 += (after - before).TotalSeconds;
        }

        Console.WriteLine("Time 1: " + Math.Round(timer1, 2));
        Console.WriteLine("Time 2: " + Math.Round(timer2, 2));

        ////------------
    }

    private void FindPath(IFindable findable) {
        Console.WriteLine("-------------------");
        Console.WriteLine("Finder: " + findable.FinderName());
        var before = DateTime.Now;

        var path = findable.FindPath();
        if (path.Count > 0) {
            Console.WriteLine("Length: " + path.Count);
            foreach (var c in path) {
                var i = c / board[0].Length;
                var j = c % board[0].Length;
                Console.Write($"({i},{j})");
            }
        }
        else {
            Console.WriteLine("No path found :(");
        }

        Console.WriteLine();

        var time = (DateTime.Now - before).TotalSeconds;
        Console.WriteLine("Time: " + time);
    }
}