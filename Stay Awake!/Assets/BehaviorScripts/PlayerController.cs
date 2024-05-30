using System;
using ES3Types;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private DefaultControls controls;
    
    [Header("Datacube Refs")]
    [SerializeField] private PlayerState playerState;
    [SerializeField] private GridPositions gridPositions;

    [Header("GameEvent Refs")]
    [SerializeField] private UnityEvent playerMoved;
    [SerializeField] private UnityEvent updateEnergy;
    [SerializeField] private UnityEvent playerDied;
    [SerializeField] private UnityEvent scoresLogged;

    [Header("GameObject Refs")] 
    public GameObject thisPlayer;

    private void Awake()
    {
        // Initialize the input controls
        controls = new DefaultControls();
    }

    private void OnEnable()
    {
        EnableControls();
    }

    private void OnDisable()
    {
        DisableControls();
    }

    public void SetInactive() {
        playerState.isActive = false;
    }

    public void AddPoints(int num) {
        playerState.currentPoints += num;
    }

    public void CheckForDeath() {
        if (playerState.currentEnergy <= 0 && playerState.isActive) {
            playerState.isActive = false;  // Set player state to inactive
            DisableControls();  // Ensure controls are disabled upon death
            playerDied.Invoke();  // Assumes this event will handle necessary clean-up
            // Debug.Log("Player died. Disabling controls.");
        }
    }

    public void UpdateScoresAndDeletePlayerObj() {
        playerState.lastEarnedPoints = playerState.currentPoints;
        if (playerState.lastEarnedPoints > playerState.highScore) {
            ES3.Save("HighScore", playerState.lastEarnedPoints);
        }
        if (ES3.KeyExists("HighScore")) {
            playerState.highScore = ES3.Load<int>("HighScore");
        }
        
        playerState.currentPoints = 0;

        Destroy(thisPlayer);
    }

    private void EnableControls()
    {
        if (controls == null)
            controls = new DefaultControls();

        // Enable the input controls
        controls.Enable();

        // Subscribe to the 'Move' action events
        controls.PlayerControls.Move.performed += OnMovePerformed;
        controls.PlayerControls.Move.canceled += OnMoveCanceled;
    }

    private void DisableControls()
    {
        if (controls != null)
        {
            // Unsubscribe from the 'Move' action events
            controls.PlayerControls.Move.performed -= OnMovePerformed;
            controls.PlayerControls.Move.canceled -= OnMoveCanceled;

            // Disable the input controls
            controls.Disable();
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        try
        {
            // Check if the player is active before processing any movement
            if (!playerState.isActive)
            {
                Debug.Log("Move performed but player is inactive.");
                return;  // Exit the function if the player is not active
            }
            
            // Check if thisPlayer is not null
            if (thisPlayer == null)
            {
                Debug.LogError("thisPlayer is null.");
                return;
            }

            // Debug.Log($"Processing move input for active player at Position: {thisPlayer.transform.position}");

            // Read the input vector from the input system
            Vector2 inputVector = context.ReadValue<Vector2>();

            // Handle horizontal movement
            if (inputVector.x == 1 && playerState.onCol != gridPositions.maxCol) {
                playerState.onCol++;
            } else if (inputVector.x == -1 && playerState.onCol != 1) {
                playerState.onCol--;
            }

            // Handle vertical movement
            if (inputVector.y == 1 && playerState.onRow != 1) {
                playerState.onRow--;
            } else if (inputVector.y == -1 && playerState.onRow != gridPositions.maxRow) {
                playerState.onRow++;
            }

            // Debug.Log($"Updated position to Row: {playerState.onRow}, Col: {playerState.onCol}");

            // Update the player's position on the grid
            thisPlayer.transform.position = gridPositions.GetSquarePos(playerState.onRow, playerState.onCol);

            // Invoke events related to movement
            if (playerState.isActive)
            {
                try
                {
                    playerMoved.Invoke();
                    updateEnergy.Invoke();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Debug.LogError($"ArgumentOutOfRangeException caught during event invocation: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Debug.LogError($"ArgumentOutOfRangeException caught in OnMovePerformed: {ex.Message}\n{ex.StackTrace}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Unexpected exception caught in OnMovePerformed: {ex.Message}\n{ex.StackTrace}");
        }
    }




    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Do something when the move keys are released
    }
}
