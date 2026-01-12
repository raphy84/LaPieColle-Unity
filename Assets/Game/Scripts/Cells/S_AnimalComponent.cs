using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class S_AnimalComponent : S_Cell, S_Actionnable
{
    [SerializeField] public S_DialogueComponent _dialogue;
    private S_UIController _dialogueController;
    public S_AnimalComponent _self;
    private S_Board _board;
    private S_Hunter _hunter;
    [SerializeField] private GameObject _cellPrefab;

    private S_Pawn _pawn;

    private void Awake()
    {
        // Recherche automatique du UIController dans la scène
        _dialogueController = FindFirstObjectByType<S_UIController>();
        _board = FindFirstObjectByType<S_Board>();
        _hunter = FindFirstObjectByType<S_Hunter>();
    }

    public void Action(S_Pawn CurrentPawn)
    {
        CurrentPawn = _pawn;
        _dialogue.Action(CurrentPawn);
    }

    public void MakeChoice()
    {
        _dialogueController.UIMakeChoice(_self);
    }

    public void DamageHunter()
    {
        _hunter._heath = _hunter._heath - 1;
        if (_hunter._heath <= 0)
        {
            _hunter.Victory();
        }
    }

    public void TrapDestruction()
    {
        int destroyedCount = 0;

        for (int i = 0; i < _board._cells.Length; i++)
        {
            if (_board._cells[i] is not S_TrapCell)
                continue;

            S_Cell cell = _board._cells[i];

            // Sauvegarde position / rotation / parent
            Transform cellTransform = cell.transform;
            Vector3 position = cellTransform.position;
            Quaternion rotation = cellTransform.rotation;
            Transform parent = cellTransform.parent;

            // Supprimer l’ancienne cellule
            Destroy(cell.gameObject);

            // Instancier la cellule normale
            GameObject newCellGO = Instantiate(
                _cellPrefab,
                position,
                rotation,
                parent
            );

            // Récupérer le script
            S_Cell newCell = newCellGO.GetComponent<S_Cell>();

            // Remplacer dans le board
            _board.ReplaceCell(i, newCell);

            destroyedCount++;

            // On s’arrête après 5
            if (destroyedCount >= 5)
                break;
        }
    }
}
