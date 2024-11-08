using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) // We check to see if the player is holding a plate to turn in at the delivery counter
            {
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
