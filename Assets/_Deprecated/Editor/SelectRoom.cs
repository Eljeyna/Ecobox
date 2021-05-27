public static class SelectRoom
{
    public static string GetNextRoom()
    {
        Game.GetRandom();
        int random = (int)(Game.random * 6f);
        return random.ToString();
    }
}
