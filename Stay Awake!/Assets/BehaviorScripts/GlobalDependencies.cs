using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDependencies : MonoBehaviour
{
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
