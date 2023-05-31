public struct Pair {
    
    public int a { get; }
    public int b { get; }

    public Pair(int a, int b) {
        this.a = a;
        this.b = b;
    }

    public override string ToString() {
        return $"({a}:{b})";
    }
}