using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAgent : MonoBehaviour
{
    private Transform player;

    private NPCFollow followCommands;
    private NPCSlowdown slowdownCommands;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;

        followCommands = GetComponent<NPCFollow>();
        slowdownCommands = GetComponent<NPCSlowdown>();
    }

    public void StartFollowing()
    {
        bool needToMove = CheckIfNeedFollow();

        if (needToMove)
        {
            followCommands.StartFollowing(player);
        }
    }

    public void StopFollowing()
    {
        followCommands.StopFollowing();
    }

    public bool CheckIfNeedFollow()
    {
      return followCommands.CheckIfNeedFollow(player);
    }

    public void StartSlowing()
    {
        slowdownCommands.StartSlowingdown();
    }

    public void StopSlowing()
    {
        slowdownCommands.StopSlowingdown();
    }

    public bool CheckIfNeedSlowdown()
    {
        //Debug.Log(followCommands.FollowDistance);
        return slowdownCommands.CheckIfNeedSlowdown(player, followCommands.FollowDistance);
    }
}
