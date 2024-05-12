using System;
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

    public void AddPoints(int num) {
        playerState.currentPoints += num;
    }

    private void EnableControls()
    {
        // Enable the input controls
        controls.Enable();

        // Subscribe to the 'Move' action events
        controls.PlayerControls.Move.performed += OnMovePerformed;
        controls.PlayerControls.Move.canceled += OnMoveCanceled;
    }

    private void DisableControls()
    {
        // Unsubscribe from the 'Move' action events
        controls.PlayerControls.Move.performed -= OnMovePerformed;
        controls.PlayerControls.Move.canceled -= OnMoveCanceled;

        // Disable the input controls
        controls.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        
        Vector2 inputVector = context.ReadValue<Vector2>();
        if(inputVector.x == 1 && playerState.onCol != gridPositions.maxCol) {
            playerState.onCol++;
        } else if (inputVector.x == -1 && playerState.onCol != 1) {
            playerState.onCol--;
        } else if (inputVector.y == 1 && playerState.onRow != 1) {
            playerState.onRow--;
        } else if (inputVector.y == -1 && playerState.onRow != gridPositions.maxRow) {
            playerState.onRow++;
        }
        
        thisPlayer.transform.position = gridPositions.GetSquarePos(playerState.onRow, playerState.onCol);
        playerMoved.Invoke();
        updateEnergy.Invoke();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Do something when the move keys are released
    }

    // public void AddEnergy(int num) {
    //     playerState.currentEnergy += num;
    //     if (playerState.currentEnergy > playerState.maxEnergySlots) {
    //         playerState.currentEnergy = playerState.maxEnergySlots;
    //     }

    //     updateEnergy.Invoke();
    // }

    // public void MinusEnergy(int num) {
    //     playerState.currentEnergy -= num;
    //     if (playerState.currentEnergy < 0) {
    //         playerState.currentEnergy = 0;
    //     }

    //     updateEnergy.Invoke();
    // }
}
