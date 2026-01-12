using UnityEngine;
using System.Collections;

public class S_Hunter : MonoBehaviour
{
    [SerializeField] public S_Board _board;
    [SerializeField] public SO_PlayerDatas _hunterDatas;
    [SerializeField] private GameObject _trapCellPrefab;
    [SerializeField] public S_UIController _UIController;

    [SerializeField] private float _moveDuration = 1f;
    private Coroutine _moveCoroutine;

    public int _heath = 5;

    private void MoveToCell()
    {
        Transform target = _board
            .GetCellByNumber(_hunterDatas._cellIndex)
            .PlayerTransform;

        // Si un déplacement est déjà en cours, on l’arrête
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MoveRoutine(target));
    }

    private IEnumerator MoveRoutine(Transform target)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float elapsed = 0f;

        // --- PHASE 1 : déplacement + regarder la direction du mouvement ---
        while (elapsed < _moveDuration)
        {
            float t = elapsed / _moveDuration;

            Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
            Vector3 moveDir = currentPos - transform.position;

            transform.position = currentPos;

            // Regarde la direction du déplacement (si on bouge)
            if (moveDir.sqrMagnitude > 0.0001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(moveDir.normalized);
                transform.rotation = lookRotation;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Sécurité : position finale exacte
        transform.position = endPos;

        // --- PHASE 2 : rotation finale vers la direction cible ---
        float rotateElapsed = 0f;
        float rotateDuration = 0.2f; // ajustable

        Quaternion currentRot = transform.rotation;

        while (rotateElapsed < rotateDuration)
        {
            float t = rotateElapsed / rotateDuration;
            transform.rotation = Quaternion.Slerp(currentRot, endRot, t);

            rotateElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
    }

    public void Move(int value)
    {
        _hunterDatas._cellIndex = _board.GetNextCellToMove(_hunterDatas._cellIndex+value);
        MoveToCell();
        TrapCell();
    }

    private void TrapCell()
    {
        int index = _hunterDatas._cellIndex;
        S_Cell cell = _board.GetCellByNumber(index);

        // Sécurité
        if (cell == null)
        {
            return;
        }

        // Si la cellule est déjà une TrapCell, on ne fait rien
        if (cell is S_TrapCell)
            return;

        // Sauvegarde position / rotation / parent
        Transform cellTransform = cell.transform;
        Vector3 position = cellTransform.position;
        Quaternion rotation = cellTransform.rotation;
        Transform parent = cellTransform.parent;

        // Supprimer l’ancienne cellule
        Destroy(cell.gameObject);

        // Instancier la TrapCell
        GameObject newCellGO = Instantiate(
            _trapCellPrefab,
            position,
            rotation,
            parent
        );

        // Sécurité CRUCIALE : vérifier le script
        S_TrapCell trapCell = newCellGO.GetComponent<S_TrapCell>();
        if (trapCell == null)
        {
            Destroy(newCellGO);
            return;
        }

        // Synchroniser le board (source de vérité)
        _board.ReplaceCell(index, trapCell);
    }

    public void Victory()
    {
        _UIController._victoryPanel.SetActive(true);
    }
}
