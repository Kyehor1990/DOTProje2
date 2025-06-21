using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public float delay = 0.2f;
    public void StartGame()
    {
        StartCoroutine(DelayedSceneLoad());
    }

    public void ExitGame()
    {
        StartCoroutine(DelayedExit());
    }

    public void OpenTutorial()
    {
        StartCoroutine(DelayedTutorial());
    }

    IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForSeconds(delay); // Sesin süresi kadar bekle
        SceneManager.LoadScene("Gameplay");

    }

    IEnumerator DelayedExit()
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Oyun kapatılıyor...");
        Application.Quit();
    }
    
    IEnumerator DelayedTutorial()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Tutorial");
    }
}