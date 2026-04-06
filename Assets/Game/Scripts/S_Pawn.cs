using UnityEngine;
using System.Collections;
using System;

public class S_Pawn : MonoBehaviour
{
    [SerializeField] public S_Board _board;
    [SerializeField] public SO_PlayerDatas _playerDatas;
    [SerializeField] public S_UIController _UIController;
    [SerializeField] private GameObject _cellPrefab;

    [SerializeField] private float _moveDuration = 0.5f;

    public int _heath = 5;
    public int _reBuildMap = 0;

    private Coroutine _moveCoroutine;
    public event Action<S_Cell> OnMoveFinished;

    private void Start()
    {
        if (!S_GlobalData.ReturningFromMiniGame)
        {
            _playerDatas._cellIndex = 0;
            MoveInstantToCell();
            ActivateCell();
            if (_UIController != null) _UIController.UpdateHeart(_heath);
        }
    }

    // Placement instantané (au start uniquement)
    public void MoveInstantToCell()
    {
        Transform cell = _board
            .GetCellByNumber(_playerDatas._cellIndex)
            .PlayerTransform;

        transform.position = cell.position;
        transform.rotation = cell.rotation;
    }

    public void Move(int value)
    {
        _reBuildMap = _reBuildMap + 1;
        if (_reBuildMap == 5)
        {
            _reBuildMap = 0;
            _board.GenerateSpecialCells();
        }
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MoveStepByStep(value));
    }

    private IEnumerator MoveStepByStep(int value)
    {
        int startIndex = _playerDatas._cellIndex;
        int targetIndex = _board.GetNextCellToMove(startIndex + value);

        int currentIndex = startIndex;

        while (currentIndex != targetIndex)
        {
            int nextIndex = _board.GetNextCellToMove(currentIndex + 1);
            Transform nextCell = _board.GetCellByNumber(nextIndex).PlayerTransform;

            yield return StartCoroutine(MoveRoutine(nextCell));

            currentIndex = nextIndex;
            _playerDatas._cellIndex = currentIndex;
        }

        ActivateCell();

        S_Cell finalCell = _board.GetCellByNumber(_playerDatas._cellIndex);
        OnMoveFinished?.Invoke(finalCell);
    }

    private IEnumerator MoveRoutine(Transform target)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float elapsed = 0f;

        // PHASE 1 : déplacement + regarder direction
        while (elapsed < _moveDuration)
        {
            float t = elapsed / _moveDuration;

            Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
            Vector3 moveDir = currentPos - transform.position;

            transform.position = currentPos;

            if (moveDir.sqrMagnitude > 0.0001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(moveDir.normalized);
                transform.rotation = lookRotation;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        // PHASE 2 : rotation finale
        float rotateElapsed = 0f;
        float rotateDuration = 0.2f;

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

    public void UnTrapCell()
    {
        int index = _playerDatas._cellIndex;
        S_Cell cell = _board.GetCellByNumber(index);

        // Sauvegarde position / rotation
        Transform cellTransform = cell.transform;
        Vector3 position = cellTransform.position;
        Quaternion rotation = cellTransform.rotation;
        Transform parent = cellTransform.parent;

        // Supprimer l’ancienne cellule
        Destroy(cell.gameObject);

        // Instancier la TrapCell
        GameObject newCellGO = Instantiate(
            _cellPrefab,
            position,
            rotation,
            parent
        );

        // Récupérer le script
        S_Cell newCell = newCellGO.GetComponent<S_Cell>();

        // Remplacer dans le board
        _board.ReplaceCell(index, newCell);
    }

    private void ActivateCell()
    {
        S_Cell cell = _board.GetCellByNumber(_playerDatas._cellIndex);
        cell.Activate(this);
    }

    public void GameOver()
    {
        _playerDatas._cellIndex = 0;
        _UIController.GameOverUI();
    }
}
