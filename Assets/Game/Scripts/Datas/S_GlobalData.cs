using System.Collections.Generic;

public static class S_GlobalData
{
    public static bool ReturningFromMiniGame = false;
    public static bool WonMiniGame = false;

    public static int PlayerHealth = 5;
    public static int PlayerCellIndex = 0;

    public static int HunterCellIndex = 0;

    // The indices of the main board cells that are traps
    public static List<int> TrapCellIndices = new List<int>();

    public static void ResetData()
    {
        ReturningFromMiniGame = false;
        WonMiniGame = false;
        TrapCellIndices.Clear();
    }
}
