using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class RestartGame : MonoBehaviour
{
    public void RestartCurrentScene()
    {
        StartCoroutine(DelayedRestart());
    }

    IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(0.2f); // Sesin süresi kadar bekle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}