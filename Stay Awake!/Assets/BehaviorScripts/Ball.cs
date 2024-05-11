using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public enum BallType {
    None,
    Energy,
    Enemy
}

public class Ball : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite anyBallSprite;
    [SerializeField] private Sprite energyBallSprite;
    [SerializeField] private Sprite enemyBallSprite;

    [Header("Properties")]
    public BallType ballType = BallType.None;

    [Header("Settings")]    
    [SerializeField] private float speed;
    [SerializeField] private float adjSpeed;
    [SerializeField] private int blinkCount;
    [SerializeField] private float durationFactor = 10;
    
    [Header("Components")]
    [SerializeField] private SpriteRenderer sr;

    [Header("Datacubes")]
    [SerializeField] private GameObject parentSquare;

    [Header("Events")]
    [SerializeField] private UnityEvent ballSpawn;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    public void HandleBall(BallType type, GameObject parentSq, float speedMultiplier) {
        ballType = type;
        parentSquare = parentSq;
        adjSpeed = speed * speedMultiplier;
        

        StartCoroutine(BallLifecycle());
    }

    private IEnumerator BallLifecycle() {
        // Initial blinking with anyBallSprite
        sr.sprite = anyBallSprite;
        yield return StartCoroutine(BlinkBall(blinkCount, 1 / adjSpeed));

        ballSpawn.Invoke();
        // Set the sprite and ballValue based on the ball type
        switch (ballType) {
            case BallType.Energy:
                sr.sprite = energyBallSprite;
                parentSquare.GetComponent<SquareController>().square.ballValue = 1;
                break;
            case BallType.Enemy:
                sr.sprite = enemyBallSprite;
                parentSquare.GetComponent<SquareController>().square.ballValue = -1;
                break;
            default:
                sr.sprite = anyBallSprite;
                break;
        }

        // Duration the ball should last
        yield return new WaitForSeconds(durationFactor / adjSpeed);

        // Blink again before destruction
        yield return StartCoroutine(BlinkBall(blinkCount, 1 / adjSpeed));

        // Destroy the ball object
        parentSquare.GetComponent<SquareController>().square.ballValue = 0;
        Destroy(gameObject);
    }

    private IEnumerator BlinkBall(int times, float interval) {
        Color originalColor = sr.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        for (int i = 0; i < times; i++) {
            sr.color = originalColor;
            yield return new WaitForSeconds(interval);
            sr.color = transparentColor;
            yield return new WaitForSeconds(interval);
        }

        // Ensure the ball is visible at the end of the blinking sequence
        sr.color = originalColor;
    }
}
