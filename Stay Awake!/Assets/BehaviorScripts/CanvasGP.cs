using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGP : MonoBehaviour
{
    [Header("Object Refs")]
    public GameObject[] gameplayElements;
    void Awake() {
        ResetGPElements();
    }

    void ResetGPElements() {
        foreach (GameObject obj in gameplayElements) {
            obj.SetActive(false);
        }
    }

    public void InitializeGPElements() {
        foreach (GameObject obj in gameplayElements) {
            obj.SetActive(true);
        }
    }
}
