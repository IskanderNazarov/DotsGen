using System;
using System.Collections.Generic;
using System.IO;

public static class Utils {
    public static float Lerp(float a, float b, float t) {
        return a + (b - a) * t;
    }

    public static Pair Lerp(Pair p1, Pair p2, float t) {
        return new Pair((int) Lerp(p1.a, p2.a, t), (int) Lerp(p1.b, p2.b, t));
    }

    public static void Shuffle<T>(this List<T> list) {
        var rng = new Random();
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void PrintBoard(int[][] board) {
        foreach (var line in board) {
            foreach (var v in line) {
                Console.Write(v + " ");
            }

            Console.WriteLine();
        }
    }

    public static void PrintList<T>(List<T> list) {
        foreach (var v in list) {
            Console.Write(v + " ");
        }

        Console.WriteLine();
    }
    
    
    public static void WriteLevelToFile(string data, int levelNumber, int complexity, string namePrefix = "") {
        var fileName = levelNumber + ".txt";
        WriteDataToFile(@"../../_levels/" + fileName, data);
    }

    public static string ReadDataFromFile(string filePath) {
        var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

        using var reader = new StreamReader(fileStream);
        var data = reader.ReadToEnd();
        reader.Close();

        return data;
    }

    public static void WriteDataToFile(string filePath, string data) {
        //string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);

        using var writer = new StreamWriter(fileStream);
        writer.Write(data);
        writer.Flush();
    }

    public static HashSet<string> ReadUsedData() {
        var dataFile = ReadDataFromFile("../../Not_Valid_Boards_Database.txt");
        var arr = dataFile.Split('\n');
        return new HashSet<string>(arr);
    }
    
    public static void WriteUsedData(string data) {
        WriteDataToFile("../../Not_Valid_Boards_Database.txt", data);
    }
}