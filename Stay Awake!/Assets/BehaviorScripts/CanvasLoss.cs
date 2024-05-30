using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CanvasLoss : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] public List<GameObject> lossObjects;

    [Header("Events")]
    [SerializeField] private UnityEvent scoresLogged;

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
        scoresLogged.Invoke();
    }

    public void HideLossScreen() {
        foreach (GameObject obj in lossObjects) {
            obj.SetActive(false);
        }
    }
}
