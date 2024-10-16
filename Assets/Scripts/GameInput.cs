using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions; // Obtain access to PlayerInputAction in Unity
    public event EventHandler OnInteractAction; // This is a publisher event, which will need subrcibers or listeners for it to work properly
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable(); //Activate action map we created in Unity

        playerInputActions.Player.Interact.performed += Interact_performed; 
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) // When a player starts an interact action, it will start this function
    {
        //Debug.Log(obj);
        // If it is not null, then there are subcribers or listerners for this event, the below code will execute fully
        OnInteractAction?.Invoke(this, EventArgs.Empty); // This fires the event handler with a refernce from itself and with no added arguments attached (which is what EventArgs.Empty is, no added extra arguments)
        //throw new System.NotImplementedException();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        //Vector2 inputVector = new Vector2(0, 0); // This is a vector that represents 2 axises, namely x and y axis
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();  
        /*
        if (Input.GetKey(KeyCode.W)) // Returns true so long as key specified in parameter is pressed and held down
        {
            inputVector.y = +1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }
        */
        inputVector = inputVector.normalized; // This will normalize the input vector, which means that diagnol and euclidian movement should behave the same (Should have a magnitude of 1 in diagnol and horizontal/vertical movement in this example and not be greater or less)
        return inputVector;
    }
}
