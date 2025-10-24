using UnityEngine;

public class S_Board : MonoBehaviour
{
    [SerializeField] private S_Cell[] _cells;

    public S_Cell GetCellByNumber(int index)
    {
        return _cells[index];
    }

    public int GetNextCellToMove(int cellNumber)
    {
        return cellNumber % _cells.Length;
    }
}
