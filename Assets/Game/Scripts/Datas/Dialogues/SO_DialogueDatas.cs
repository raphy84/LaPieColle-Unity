using UnityEngine;

[System.Serializable]
public struct DialogueRow
{
    public int rowNumber;
    public string characterName;
    public string LongDialogue;
    public int nextRowNumber;
}

[CreateAssetMenu(fileName = "SO_DialogueDatas", menuName = "Scriptable Objects/SO_DialogueDatas")]
public class SO_DialogueDatas : ScriptableObject
{
    public DialogueRow[] rows;
}
