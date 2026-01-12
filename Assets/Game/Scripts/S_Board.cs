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
        int animalCount = Random.Range(4, 7);   // 4 à 6
        int foodCount = Random.Range(10, 15); // 10 à 14

        // Récupère UNIQUEMENT les cellules normales
        List<int> normalCellIndices = GetNormalCellIndices();

        // Sécurité
        if (animalCount + foodCount > normalCellIndices.Count)
        {
            Debug.LogWarning("Pas assez de cellules normales pour la génération.");
            return;
        }

        // AnimalCells
        for (int i = 0; i < animalCount; i++)
        {
            int index = GetRandomIndex(normalCellIndices);
            ReplaceWithPrefab(index, _animalCellPrefab);
            normalCellIndices.Remove(index);
        }

        // FoodCells
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
            // Seulement les cellules de base
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

        // Sécurité absolue
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
            Debug.LogError("Prefab sans S_Cell !");
            Destroy(newCellGO);
            return;
        }

        ReplaceCell(index, newCell);
    }
}