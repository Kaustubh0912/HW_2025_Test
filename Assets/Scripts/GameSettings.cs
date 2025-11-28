public enum GameMode
{
    Limited,
    Endless
}

public static class GameSettings
{
    public static GameMode CurrentGameMode = GameMode.Limited;
}
