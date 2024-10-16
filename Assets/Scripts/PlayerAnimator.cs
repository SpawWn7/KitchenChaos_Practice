using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking"; //This is the name of the condition we made on Unity for trantioning between idle and walking
    private Animator animator;
    [SerializeField] private Player player; // Obtain reference to player game object

    private void Awake()
    {
        animator = GetComponent<Animator>();  // Obtain reference to animator component from Unity
    }

    private void Update() //Reminder: Update runs on every frame update
    {
        animator.SetBool(IS_WALKING, player.IsWalking()); // Checkts to see if player is moving on every frame update
    }
}
