using UnityEngine;

public class S_DialogueComponent : MonoBehaviour, S_Actionnable
{
    [SerializeField] private SO_DialogueDatas _dialogueData;
    private DialogueRow _currentRow;
    private int _currentRowIndex = 0;
    [SerializeField] private S_UIController _dialogueController; 

    public void Action(S_Pawn CurrentPawn)
    {
        _currentRow = GetDialogueRow();
        _dialogueController.StartDialogue(this);
    }

    public DialogueRow GetDialogueRow()
    {
        return _dialogueData.rows[_currentRowIndex];
    }

    public string GetDialogueText()
    {
        return _currentRow.LongDialogue;
    }

    public string GetCharacterName()
    {
        return _currentRow.characterName;
    }

    public void GetNextRow()
    {
        if(_currentRow.nextRowNumber == -1)
        {
            _dialogueController.EndDialogue();
        }
        else
        {
            _currentRowIndex = _currentRow.nextRowNumber;
            _currentRow = GetDialogueRow();
            _dialogueController.UpdateText();
        }
    }
}
