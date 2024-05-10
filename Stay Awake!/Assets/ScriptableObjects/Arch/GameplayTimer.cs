using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameplayTimer : ScriptableObject
{
    [SerializeField] private float timeElapsed;

    public float getTimeElapsed() {
        return timeElapsed;
    }

    public float setTimeElapsed(float newTimeElapsed) {
        return timeElapsed = newTimeElapsed;
    }

    public float addTimeElapsed(float timeToAdd) {
        return timeElapsed += timeToAdd;
    }

    public float minusTimeElapsed(float timeToMinus) {
        return timeElapsed -= timeToMinus;
    }
}
