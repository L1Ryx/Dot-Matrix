using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasLoss : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] public List<GameObject> lossObjects;

    void Awake() {
        HideLossScreen();
    }

    public void ShowLossScreen(float delay) {
        StartCoroutine(DelayAndShowLossScreen(delay));
    }

    private IEnumerator DelayAndShowLossScreen(float delay) {
        yield return new WaitForSeconds(delay);
        foreach (GameObject obj in lossObjects) {
            obj.SetActive(true);
        }
    }

    public void HideLossScreen() {
        foreach (GameObject obj in lossObjects) {
            obj.SetActive(false);
        }
    }
}
