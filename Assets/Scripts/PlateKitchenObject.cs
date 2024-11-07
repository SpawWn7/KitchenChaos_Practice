using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList; // We are enforcing only valid ingredients to be added on to the plate
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) // The ingredient attempting to be added to the plate is not a valid ingredient
        {
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) // Already has the kitchen type or ingredient. At most one of each type of ingredient can be added to a plate. For exmaple we don't want to have 2 tomato slices or 2 lettuces on the plate.
        {
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            return true;
        }
    }
}
