public enum GameResult
{
    UNKNOWN,
    DRAW,
    XWINS,
    OWINS
}

public static class GameResultExtentions
{
    public static bool HasEnded(this GameResult result)
    {
        return result != GameResult.UNKNOWN;
    }

    public static bool IsWin(this GameResult result)
    {
        return result == GameResult.OWINS || result == GameResult.XWINS;
    }

    public static bool IsMyWin(this GameResult myResult, GameResult targetResult)
    {
        return myResult.IsWin() && myResult == targetResult;
    }

    public static bool IsOponentWin(this GameResult myResult, GameResult targetResult)
    {
        if (myResult == GameResult.UNKNOWN || myResult == GameResult.DRAW)
        {
            return false;
        }

        return !myResult.IsMyWin(targetResult);
    }
}