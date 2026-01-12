using UnityEngine;

public class S_Cell : MonoBehaviour,S_CellActivable
{
    public Transform PlayerTransform;

    public virtual void Activate(S_Pawn CurrentPawn)
    {
        if(GetComponent<S_Actionnable>() != null)
        {
            GetComponent<S_Actionnable>().Action(CurrentPawn);
        }
    }
}
