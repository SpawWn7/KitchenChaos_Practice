using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetKitchenObjectFollowTransform();


    public void SetKitchenObject(KitchenObject kitchenObject); // We want to be able to transfor the proper kitchenObject to the new parent clear counter

    public KitchenObject GetKitchenObject();


    public void ClearKitchenObject();


    public bool HasKitchenObject(); // We want to check if a clear counter has a kitchen object on top of it
 
}
