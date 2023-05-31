using System;
using System.Collections.Generic;
using System.Text;

public class BoardsDatabase {
    public static BoardsDatabase shared = new BoardsDatabase();

    private HashSet<string> allData;
    private int counter;

    public BoardsDatabase() {
        allData = Utils.ReadUsedData();
    }

    public bool HasBoard(List<int> blocks, int startVertex) {
        return allData.Contains(GenerateKey(blocks, startVertex));
    }


    private string GenerateKey(List<int> blocks, int startVertex) {
        var sb = new StringBuilder();

        sb.Append(startVertex);
        foreach (var b in blocks) {
            sb.Append('_');
            sb.Append(b);
        }

        return sb.ToString();
    }

    public void Add(List<int> blocks, int startVertex) {

        var key = GenerateKey(blocks, startVertex);
        allData.Add(key);


        counter++;
        if (counter >= 10000) {
            counter = 0;
            SaveToFile();
            Console.WriteLine("Data base entries count: " + allData.Count);
        }
    }

    public void SaveToFile() {
        var sb = new StringBuilder();

        foreach (var s in allData) {
            sb.Append(s);
            sb.Append('\n');
        }

        Utils.WriteUsedData(sb.ToString());
    }
}