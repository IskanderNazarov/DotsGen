public interface ILevelCreator {
    public Level GenerateLevel(IFindable findable, Pair boardSize, int vertexCount, int levelNumber);
}