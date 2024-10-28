using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    //[SerializeField] private Transform tomatoPrefab; // Transform and GameObjects are almost identical. From here one can edit the tomoatoPrefab object on Unity's editor since the variable is serialized.

    public override void Interact(Player player)
    {
       if(!HasKitchenObject()) // Check if counter does not have a kitchen object (tomato, items, etc)
        {
            if (player.HasKitchenObject()) // Player is carrying something
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else // Player is not carrying anything
            {

            }
        }
       else // A kitchen object is already on the counter
        {
            if (player.HasKitchenObject()) // Player is carrying something so don't do anything. Don't attempt to pick up kitchen object when player's hands are full. 
            {
       
            }
            else // Player is not carrying anything
            {
                GetKitchenObject().SetKitchenObjectParent(player); // Player picks up kitchen object from counter
            }
        }
    }

}
