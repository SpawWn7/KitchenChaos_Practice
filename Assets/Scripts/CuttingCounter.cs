using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject()) // Check if counter does not have a kitchen object (tomato, items, etc)
        {
            if (player.HasKitchenObject()) // Player is carrying something
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) // We check if the player is attempting to drop a valid cuttable recipe to be cut
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
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

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))  // If the cutting board has something on it AND it is cuttable then we may cut
        {
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO()); 
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) 
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return true;
            }
        }
        return false;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) 
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }
}
