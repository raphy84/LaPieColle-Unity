using UnityEngine;
using System.Collections.Generic;

public class S_Board : MonoBehaviour
{
    [SerializeField] public S_Cell[] _cells;
    [SerializeField] private GameObject _animalCellPrefab;
    [SerializeField] private GameObject _foodCellPrefab;

    public void Start()
    {
        GenerateSpecialCells();

        if (S_GlobalData.ReturningFromMiniGame)
        {
            // On gère l'ordre de restauration ici pour éviter les bugs !
            S_Hunter hunter = Object.FindAnyObjectByType<S_Hunter>();
            S_Pawn player = Object.FindAnyObjectByType<S_Pawn>();

            // 1. Restaurer le Hunter et les pièges
            if (hunter != null)
            {
                hunter._hunterDatas._cellIndex = S_GlobalData.HunterCellIndex;
                hunter.MoveToCellInstant();
                foreach (int trapIndex in S_GlobalData.TrapCellIndices)
                {
                    hunter.TrapCellAt(trapIndex);
                }
            }

            // 2. Restaurer le Joueur et son résultat
            if (player != null)
            {
                player._heath = S_GlobalData.PlayerHealth;
                player._playerDatas._cellIndex = S_GlobalData.PlayerCellIndex;

                if (S_GlobalData.WonMiniGame)
                {
                    Debug.Log("WON ! Piège neutralisé.");
                    player.UnTrapCell();
                }
                else
                {
                    Debug.Log("LOST ! Dégât subit.");
                    player._heath -= 1;
                    if (player._heath <= 0) player.GameOver();
                }
                
                player.MoveInstantToCell();
                if (player._UIController != null) player._UIController.UpdateHeart(player._heath);
            }

            S_GlobalData.ResetData();
        }
    }

    public S_Cell GetCellByNumber(int index)
    {
        return _cells[index];
    }

    public int GetNextCellToMove(int cellNumber)
    {
        return cellNumber % _cells.Length;
    }

    public void ReplaceCell(int index, S_Cell newCell)
    {
        _cells[index] = newCell;
    }

    public void GenerateSpecialCells()
    {
        int animalCount = Random.Range(4, 7);
        int foodCount = Random.Range(10, 15);

        List<int> normalCellIndices = GetNormalCellIndices();

        if (animalCount + foodCount > normalCellIndices.Count)
        {
            Debug.LogWarning("Pas assez de cellules normales pour la génération.");
            return;
        }

        for (int i = 0; i < animalCount; i++)
        {
            int index = GetRandomIndex(normalCellIndices);
            ReplaceWithPrefab(index, _animalCellPrefab);
            normalCellIndices.Remove(index);
        }

        for (int i = 0; i < foodCount; i++)
        {
            int index = GetRandomIndex(normalCellIndices);
            ReplaceWithPrefab(index, _foodCellPrefab);
            normalCellIndices.Remove(index);
        }
    }

    private int GetRandomIndex(List<int> indices)
    {
        int randomListIndex = Random.Range(0, indices.Count);
        return indices[randomListIndex];
    }

    private List<int> GetNormalCellIndices()
    {
        List<int> indices = new List<int>();

        for (int i = 0; i < _cells.Length; i++)
        {
            if (_cells[i] != null && _cells[i].GetType() == typeof(S_Cell))
            {
                indices.Add(i);
            }
        }

        return indices;
    }

    private void ReplaceWithPrefab(int index, GameObject prefab)
    {
        S_Cell cell = GetCellByNumber(index);
        if (cell == null)
            return;

        if (cell.GetType() != typeof(S_Cell))
            return;

        Transform t = cell.transform;
        Vector3 position = t.position;
        Quaternion rotation = t.rotation;
        Transform parent = t.parent;

        Destroy(cell.gameObject);

        GameObject newCellGO = Instantiate(prefab, position, rotation, parent);

        S_Cell newCell = newCellGO.GetComponent<S_Cell>();
        if (newCell == null)
        {
            Destroy(newCellGO);
            return;
        }

        ReplaceCell(index, newCell);
    }
}
