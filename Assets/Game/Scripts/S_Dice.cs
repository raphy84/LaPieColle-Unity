using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class S_Dice : MonoBehaviour
{
    [SerializeField] private S_Pawn _pawn;
    [SerializeField] private S_Hunter _hunter;
    [SerializeField] private Button _rollButton;
    [SerializeField] private TMP_Text _rollNumber;

    private void Awake()
    {
        _pawn.OnMoveFinished += OnPlayerMoveFinished;
    }

    private void OnDestroy()
    {
        _pawn.OnMoveFinished -= OnPlayerMoveFinished;
    }

    public void RollTheDice()
    {
        _rollButton.interactable = false;

        int valuePlayer = Random.Range(1, 7);
        _pawn.Move(valuePlayer);
        _rollNumber.text = valuePlayer.ToString();

        int valueHunter = Random.Range(1, 20);
        _hunter.Move(valueHunter);
    }

    private void OnPlayerMoveFinished(S_Cell cell)
    {
        // Si la cellule contient un dialogue, on ne réactive pas le bouton
        if (cell.GetComponent<S_DialogueComponent>() != null)
            return;

        // Sinon on peut relancer le dé
        _rollButton.interactable = true;
    }
}
