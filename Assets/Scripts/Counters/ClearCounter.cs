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
            if (player.HasKitchenObject()) // Player is carrying something
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) // Check to see if the player is carrying a plate (kitchen object)
                {
     
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) // Add the current ingredient on the counter to the plate
                    {
                        GetKitchenObject().DestroySelf(); // After adding the ingredient to the plate, remove the ingredient from the counter itself
                    }
                }
                else // Player is carrying something else other than a plate
                {
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) // A plate is on the counter
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else // Player is not carrying anything
            {
                GetKitchenObject().SetKitchenObjectParent(player); // Player picks up kitchen object from counter
            }
        }
    }

}
