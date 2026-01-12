using UnityEngine;

public class S_DialogueComponent : MonoBehaviour, S_Actionnable
{
    [SerializeField] private SO_DialogueDatas _dialogueData;
    private DialogueRow _currentRow;
    private int _currentRowIndex = 0;
    private S_UIController _dialogueController;
    [SerializeField] private GameObject _self;

    private void Awake()
    {
        // 🔍 Recherche automatique du UIController dans la scène
        _dialogueController = FindFirstObjectByType<S_UIController>();

        if (_dialogueController == null)
        {
            Debug.LogError(
                $"[S_DialogueComponent] Aucun S_UIController trouvé dans la scène",
                this
            );
        }
    }

    public void Action(S_Pawn CurrentPawn)
    {
        _currentRowIndex = 0;
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
            if (_currentRow.nextRowNumber == -1 && _self.GetComponent<S_AnimalComponent>() != null)
            {
                var _animalComponentRef = _self.GetComponent<S_AnimalComponent>();
                _animalComponentRef.MakeChoice();
            }
        }
    }
}
