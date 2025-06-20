using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ExitGame()
    {
        Debug.Log("Oyun kapatılıyor...");
        Application.Quit();
    }
}