using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    private CharacterManager character;

    public float horizontal;
    public float vertical;
    
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    
    public void UpdateAnimatorMovementParameters(float horizontal, float vertical)
    {
        character.animator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);
    }
}
