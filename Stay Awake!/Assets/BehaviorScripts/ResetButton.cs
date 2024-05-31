using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResetButton : MonoBehaviour
{
    [Header("Datacubes")]
    [SerializeField] private PlayerState playerState;
    [Header("Objects")]
    [SerializeField] private GameObject gameplayInitializerPrefab;
    public GameObject gameplayInitializerObj;
    [Header("Events")]
    [SerializeField] private UnityEvent gameReset;

    public void ResetGame(float delay) {
        // SFXManager.Instance.PlaySound("ButtonPress");
        gameplayInitializerObj = Instantiate(gameplayInitializerPrefab);
        gameReset.Invoke();
    }
}
