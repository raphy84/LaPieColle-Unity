using UnityEngine;

public class S_TrapCell : S_Cell, S_Actionnable
{
    public void Action(S_Pawn CurrentPawn)
    {
        CurrentPawn._heath = CurrentPawn._heath - 1;
        CurrentPawn._UIController.UpdateHeart(CurrentPawn._heath);
        CurrentPawn.UnTrapCell();
        if (CurrentPawn._heath <= 0)
        {
            CurrentPawn.GameOver();
        }
    }
}