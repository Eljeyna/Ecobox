public static class SelectRoom
{
    public static string GetNextRoom()
    {
        StaticGameVariables.GetRandom();
        int random = (int)(StaticGameVariables.random * 6f);
        return random.ToString();
    }
}
