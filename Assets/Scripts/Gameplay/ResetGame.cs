using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private string sceneName;

    void Update()
    {
        if (playerMovement.IsDead)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
