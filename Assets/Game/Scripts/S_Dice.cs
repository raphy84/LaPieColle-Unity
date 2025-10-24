using UnityEngine;

public class S_Dice : MonoBehaviour
{
    [SerializeField] private S_Pawn _pawn;

    public void RollTheDice()
    {
        int value = Random.Range(1, 7);
        _pawn.Move(value);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
