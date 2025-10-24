using UnityEngine;

public class S_Pawn : MonoBehaviour
{
    [SerializeField] public S_Board _board;
    [SerializeField] public SO_PlayerDatas _playerDatas;

    private void Start()
    {
        MoveToCell();
    }

    private void MoveToCell()
    {
        Transform NewPos = _board.GetCellByNumber(_playerDatas._cellIndex)._PlayerTransform;
        transform.position = NewPos.position;
        transform.rotation = NewPos.rotation;
    }

    public void Move(int value)
    {
        _playerDatas._cellIndex = _board.GetNextCellToMove(_playerDatas._cellIndex+value);
        MoveToCell();
    }
}
