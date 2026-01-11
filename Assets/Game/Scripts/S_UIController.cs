using TMPro;
using UnityEngine;

public class S_UIController : MonoBehaviour
{
    [SerializeField] private S_DialogueComponent _dialogueComponent;
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TMP_Text _characterNameText;
    [SerializeField] private TMP_Text _dialogueText;

    public void StartDialogue(S_DialogueComponent DialogueComponent)
    {
        _dialogueComponent = DialogueComponent;
        UpdateText();
        _dialoguePanel.SetActive(true);
    }

    public void ChageRow()
    {
        _dialogueComponent.GetNextRow();
    }

    public void UpdateText()
    {
        _characterNameText.text = _dialogueComponent.GetCharacterName();
        _dialogueText.text = _dialogueComponent.GetDialogueText();
    }

    public void EndDialogue()
    {
        _dialoguePanel.SetActive(false);
    }
}
