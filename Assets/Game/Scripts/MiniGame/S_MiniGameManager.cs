using UnityEngine;
using UnityEngine.SceneManagement;

public class S_MiniGameManager : MonoBehaviour
{
    public void WinGame()
    {
        Debug.Log("Mini Game Won!");
        S_GlobalData.WonMiniGame = true;
        S_GlobalData.ReturningFromMiniGame = true;
        LoadBoardScene();
    }

    public void LoseGame()
    {
        Debug.Log("Mini Game Lost!");
        S_GlobalData.WonMiniGame = false;
        S_GlobalData.ReturningFromMiniGame = true;
        LoadBoardScene();
    }

    private void LoadBoardScene()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
