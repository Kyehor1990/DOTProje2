using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<GameObject> tutorialSteps;

    bool anyDeactivated = false;


    public void NextStep()
    {
        anyDeactivated = false;
        // 1. İlk aktif olanı bul ve kapat
        foreach (GameObject step in tutorialSteps)
        {
            if (step.activeSelf)
            {
                step.SetActive(false);
                anyDeactivated = true;
                break; // Sadece birini kapat
            }
        }

        // 2. Eğer tümü kapalıysa (yani artık hiçbiri aktif değilse), hepsini geri aç
        if (!IsAnyActive())
        {
            foreach (GameObject step in tutorialSteps)
            {
                step.SetActive(true);
            }
        }
    }

    private bool IsAnyActive()
    {
        foreach (GameObject step in tutorialSteps)
        {
            if (step.activeSelf)
                return true;
        }
        return false;
    }

    public void OpenStartMenu()
    {
        StartCoroutine(DelayedStartMenu());
    }

    IEnumerator DelayedStartMenu()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("StartMenu");
    }
}
