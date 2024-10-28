using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject()) // If the player is holding an object, then we will destroy (despawn) the object
        {
            player.GetKitchenObject().DestroySelf();
        }
    }
}