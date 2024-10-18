using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabObject;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) // If kitchenObject does not exist, aka null, then we spawn it and then assign it. Makes it so that we do not spawn an infinite amount of objects just from interacting with the counters multiple times if the object exists.
        {
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab); // Create or spawn an object of the tomato prefab using the position of an empty game object (which should be set somewhere desirable for the tomato to spawn) 
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player); // We set the clear counter and the visual for the kitchen object on it (Further reference in KitchenObject.cs)

            OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
        }
    }

}
