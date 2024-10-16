using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null) // If the current parent has a reference to a ktichen object (something is on top of it) then we clear or remove the reference first as we are going to transfor the location of the kitchen object to a new clear counter parent
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent; // We get to set where the kitchen object is resting at or which clear counter it is referencing

        if(kitchenObjectParent.HasKitchenObject()) // This shouldn't happen as we are already clearing the new counter to recieve the new one. This is just in case any weird edge cases happen
        {
            Debug.LogError("IKitchenObjectParent already has a KitchenObject!");
        }
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform(); // We also must set where that kitchen object spawns relative to the clear counter's top point (This is just updating the visual)
        transform.localPosition = Vector3.zero; // Make sure we postion our object on top of counterTopPoint

    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
}
