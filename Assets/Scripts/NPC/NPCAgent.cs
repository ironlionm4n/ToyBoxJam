using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAgent : MonoBehaviour
{
    private Transform player;

    private NPCFollow followCommands;
    private NPCSlowdown slowdownCommands;
    private NPCJump jumpCommands;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;

        followCommands = GetComponent<NPCFollow>();
        slowdownCommands = GetComponent<NPCSlowdown>();
        jumpCommands = GetComponent<NPCJump>();
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
      return followCommands.CheckIfNeedFollow(player);
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
}
