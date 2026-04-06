using UnityEngine;
using UnityEngine.SceneManagement;

public class S_TrapCell : S_Cell, S_Actionnable
{
    public void Action(S_Pawn CurrentPawn)
    {
        // On sauvegarde l'tat actuel du joueur
        S_GlobalData.PlayerHealth = CurrentPawn._heath;
        S_GlobalData.PlayerCellIndex = CurrentPawn._playerDatas._cellIndex;

        // On sauvegarde l'tat du Hunter
        S_Hunter hunter = Object.FindAnyObjectByType<S_Hunter>();
        if (hunter != null && hunter._hunterDatas != null)
        {
            S_GlobalData.HunterCellIndex = hunter._hunterDatas._cellIndex;
        }

        // On sauvegarde l'emplacement de tous les piges
        S_GlobalData.TrapCellIndices.Clear();
        S_Board board = CurrentPawn._board;
        if (board != null)
        {
            for (int i = 0; i < board._cells.Length; i++)
            {
                if (board._cells[i] is S_TrapCell)
                {
                    S_GlobalData.TrapCellIndices.Add(i);
                }
            }
        }

        // Transition hard vers le MiniGame
        SceneManager.LoadScene("MiniGameScene", LoadSceneMode.Single);
    }
}