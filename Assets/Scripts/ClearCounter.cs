using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    //[SerializeField] private Transform tomatoPrefab; // Transform and GameObjects are almost identical. From here one can edit the tomoatoPrefab object on Unity's editor since the variable is serialized.
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public void Interact(Player player)
    {
        if (kitchenObject == null) // If kitchenObject does not exist, aka null, then we spawn it and then assign it. Makes it so that we do not spawn an infinite amount of objects just from interacting with the counters multiple times if the object exists.
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint); // Create or spawn an object of the tomato prefab using the position of an empty game object (which should be set somewhere desirable for the tomato to spawn) 
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this); // We set the clear counter and the visual for the kitchen object on it (Further reference in KitchenObject.cs)
        }
        else // If the kitchen object is spawnned in or already  exists,  then we just look at which clear counter it is tied to or referencing
        {
            // Give object to the player
            kitchenObject.SetKitchenObjectParent(player);
            Debug.Log(kitchenObject.GetKitchenObjectParent()); 
        }
        //Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().GetKitchenObjectSO().objectName); // This calls the GetKitchentObjectSO method inside the KitchenObject Class, which in turn has a KitchenObjectSO field
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) // We want to be able to transfor the proper kitchenObject to the new parent clear counter
    {
        this.kitchenObject = kitchenObject;
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
