using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; } // Valid C# property

    public event EventHandler OnPickedSomthing;
    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangeEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f; // This is a private variable that can still be accessed through the Unity editor
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask; // This is for raycasting. We specify what object the raycast should work. 
    [SerializeField] private Transform KitchenObjectHoldPoint;

    private Vector3 lastInteractDir;
    private bool isWalking;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if(Instance !=null)
        {
            Debug.LogError("There is more than one player instance!");
        }
        Instance = this;
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction; // Set up lsitening for event
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update() //This is for player movement. Function is called per frame update.
    {
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y); // Must explicitly cast inputVector into Vector3 since transform.position is a vector of 3 items, which are x, y, z. In a 3D environment, Y is for up and down, X  is for left and right, and Z is for forward and backwards. This will allow for forward, backward, left, and right movement (NOT UP AND DOWN!)

        if (moveDir != Vector3.zero) // We are checking if the last known movement is still faceing an object for interaction purposes. So if the player stopped moving then this will not execute, which will have the last known moveDir. Otherwise keep updating as the player moves.
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) // We define a raycast from point of origin pointing to a direction. The ray's distance is defined in interactDistance. rayCastHit variable is a reference to an object that is first hit by the raycast.
        {
            //Debug.Log(raycastHit.transform);
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) // Check to see if the component we are getting is a ClearCounter object. This is how we check for certain components.
            {
                // Has ClearCounter
                //clearCounter.Interact(); // Call the interact function in ClearCounter.cs
                if (baseCounter != selectedCounter) // If there is a counter in front of the player then it will set it. Else it will not set it at all.
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }

        Debug.Log(selectedCounter);
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y); // Must explicitly cast inputVector into Vector3 since transform.position is a vector of 3 items, which are x, y, z. In a 3D environment, Y is for up and down, X  is for left and right, and Z is for forward and backwards. This will allow for forward, backward, left, and right movement (NOT UP AND DOWN!)

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f; // This defines the player size for collision detection purposes
        float playerHeight = 2f;
        //bool canMove = !Physics.Raycast(transform.position, movDir, playerSize); // Bool operation that returns true if point of origin makes contact with something else, false if nothing is in the way  (Uses rays or lasers casted from point of origin to see if it reaches or touches another object)
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance); // CapsuleCast is better than RayCast for this case because it uses the shape of a capsule for collision detection rather than using a  thin laser line from center of player for collision detection

        if (!canMove)
        {
            //Cannot  move  towards this direction

            //Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // Can move only on the X axis
                moveDir = moveDirX;
            }
            else
            {
                // Cannot  move only in X axis

                //Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // Can only move  on Z axis
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannnot  move in any direction
                }

            }
        }

        if (canMove) //Collision detection; Player's position will not be allowed to change if it's colliding with someting or if something is in the way
        {
            transform.position += moveDir * moveDistance; // This will allow actual movement of game object or the Player in this case. Time.deltaTime incorporates the time between frames. The faster the framerate, the smaller the number. Essentially this makes it so that the object moves at the same speed, indpendent or regardless of the framerate
        }

        isWalking = (moveDir != Vector3.zero); // True if player is moving (movDir is not 0,0,0 on the x,y,z axis

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed); // This makes it so that the player model will turn and face whereever the position's magnitude or direction is in a smooth fashion. The rotateSpeed adds a multiplier to the speed of rotation (this would make it faster)
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangeEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return KitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) // We want to be able to transfor the proper kitchenObject to the new parent clear counter
    {
        this.kitchenObject = kitchenObject;

        if  (kitchenObject != null) 
        {
            OnPickedSomthing?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject() // We want to check if a clear counter has a kitchen object on top of it
    {
        return kitchenObject != null;
    }
}
