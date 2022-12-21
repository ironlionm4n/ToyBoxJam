using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBossLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerMovement = other.GetComponent<PlayerMovement>();
        if(playerMovement != null)
            SceneManager.LoadScene("BossBattle");
    }
}
