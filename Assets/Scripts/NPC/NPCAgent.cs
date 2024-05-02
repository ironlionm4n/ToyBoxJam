using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAgent : MonoBehaviour
{
    private Transform player;

    private NPCFollow followCommands;
    private NPCJump jumpCommands;
    private NPCAbility abilityCommands;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;

        followCommands = GetComponent<NPCFollow>();
        jumpCommands = GetComponent<NPCJump>();
        abilityCommands = GetComponent<NPCAbility>();
    }

    public void StartFollowing()
    {
        followCommands.StartFollowing(player);
    }

    public void StopFollowing()
    {
        followCommands.StopFollowing();
    }

    public bool CheckIfNeedFollow()
    {
      return followCommands.CheckIfNeedFollow(player, jumpCommands);
    }

    public bool CheckIfNeedToJump()
    {
        return jumpCommands.CheckIfNeedToJump(player);
    }

    public void StartJumping()
    {
        jumpCommands.StartJumping(player);
    }

    public void StopJumping()
    {
        jumpCommands.StopJumping();
    }

    public void UseAbility()
    {
        abilityCommands.UseAbility();
    }

    public void StopAbility()
    {
        abilityCommands.AbilityComplete();
    }
}
