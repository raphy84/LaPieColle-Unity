using UnityEngine;

public class S_FoodComponent : S_Cell, S_Actionnable
{
    [SerializeField] public S_DialogueComponent _dialogue;
    public void Action(S_Pawn CurrentPawn)
    {
        _dialogue.Action(CurrentPawn);
        if (CurrentPawn._heath + 1 < 6)
        {
            CurrentPawn._heath = CurrentPawn._heath + 1;
            CurrentPawn._UIController.UpdateHeart(CurrentPawn._heath);
            CurrentPawn.UnTrapCell();
        }
    }
}
