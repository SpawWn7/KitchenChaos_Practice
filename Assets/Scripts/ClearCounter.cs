using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    //[SerializeField] private Transform tomatoPrefab; // Transform and GameObjects are almost identical. From here one can edit the tomoatoPrefab object on Unity's editor since the variable is serialized.

    public override void Interact(Player player)
    {
       
    }

}
