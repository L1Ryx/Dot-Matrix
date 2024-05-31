using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CanvasTutorial : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] public List<GameObject> tutorialObjects;
    [Header("Datacubes")]
    [SerializeField] private PlayerState playerState;

    void Awake() {
        HideTutorialScreen();
    }

    public void ShowTutorialScreen(float duration) {
        playerState.hasSeenTutorial = ES3.Load<bool>("tutorialSeen", false);
        if (!playerState.hasSeenTutorial) {
            StartCoroutine(ShowTutorialScreenCoroutine(duration));
        }
    }

    private IEnumerator ShowTutorialScreenCoroutine(float duration) {
        foreach (GameObject obj in tutorialObjects) {
            obj.SetActive(true);
        }
        yield return new WaitForSeconds(duration);
        ES3.Save("tutorialSeen", true);
        HideTutorialScreen();
    }

    public void HideTutorialScreen() {
        foreach (GameObject obj in tutorialObjects) {
            obj.SetActive(false);
        }
    }
}
