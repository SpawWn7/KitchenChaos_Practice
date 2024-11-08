using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;


    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0f) // We will begin to grab a new recipe from the recipeSOList every 4 seconds to then add to the waiting recipe orders
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax) // We only want to add a new recipe to the waiting list as long as there are less than  4 recipes/orders waiting
            {
                RecipeSO recipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(recipeSO.recipeName);
                waitingRecipeSOList.Add(recipeSO);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) // We will valdiate the plate being delivered
    {
        for (int i=0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) // We check to see if the plate has the same number of ingredients as the waiting recipe/order
            {
                bool plateContentsMatchesRecipe = true;

                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) // We are cycling through all teh ingredients of the recipe
                {
                    bool ingredientFound = false;

                    foreach(KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) // We are cycling through all the ingredients on the plate
                    {
                        if (recipeKitchenObjectSO == plateKitchenObjectSO) // Ingredients match from the recipe and plate
                        {
                            ingredientFound=true;
                            break;
                        }
                    }
                    if (!ingredientFound)  // This recipe ingredient was not found on the plate
                    {
                        plateContentsMatchesRecipe = false;
                    }
                }
                if (plateContentsMatchesRecipe) // Player delivered the correct recipe
                {
                    Debug.Log("Player delivered the correct recipe");
                    waitingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }
        //Player failed to deliver the correct recipe
        Debug.Log("Player did not deliver the correct recipe");

    }
}
