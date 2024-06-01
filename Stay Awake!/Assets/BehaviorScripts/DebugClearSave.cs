using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugClearSave : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnityEvent clearSave;

    public void ClearSave() {
        clearSave.Invoke();
    }
}
