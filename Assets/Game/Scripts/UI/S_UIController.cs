using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_UIController : MonoBehaviour
{
    [SerializeField] private S_DialogueComponent _dialogueComponent;
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private GameObject _choicePanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] public GameObject _victoryPanel;
    [SerializeField] private TMP_Text _characterNameText;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private Button _skipButton;
    [SerializeField] private Button _rollButton;
    
    public Image[] _heart;
    private S_AnimalComponent _animalComponent;

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
        _rollButton.interactable = true;
    }

    public void UpdateHeart(int health)
    {
        foreach (Image img in _heart)
        {
            img.color = Color.white;
        }
        for (int i = 0; i < health; i++)
        {
            _heart[i].color = Color.red;
        }
    }

    public void UIMakeChoice(S_AnimalComponent _animal)
    {
        _animalComponent = _animal;
        Debug.Log(_animalComponent);
        _choicePanel.SetActive(true);
        _skipButton.interactable = false;
    }

    public void TrapChoice()
    {
        _skipButton.interactable = true;
        _choicePanel.SetActive(false);
        EndDialogue();
        _animalComponent.TrapDestruction();
    }

    public void HunterChoice()
    {
        _skipButton.interactable = true;
        _choicePanel.SetActive(false);
        EndDialogue();
        _animalComponent.DamageHunter();
    }

    public void GameOverUI()
    {
        _gameOverPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
