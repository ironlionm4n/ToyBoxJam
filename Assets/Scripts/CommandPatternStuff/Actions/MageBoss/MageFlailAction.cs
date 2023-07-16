using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public class MageFlailAction : MonoBehaviour, IAction
{
    public Transform mage { get; private set; }

    public GameObject projectile { get; private set; }

    public int numberOfBackForths { get; private set; }

    public Transform[] movePoints { get; private set; }

    public bool moveRight { get; private set; }

    public bool moving { get; private set; }

    public bool executing { get; private set; }

    public float moveTime { get; private set; }
    
    public float shootCooldown { get; private set; }

    public Action<MageFlailAction> FlailAttack { get; private set; }

    /// <summary>
    ///  Takes in the the mage boss transform, projectile the mage will throw, the number of time they move back and forth before the attack stops,
    ///  the points the mage moves between (with the first point being the left and the second point being the right), 
    ///  direction mage moves at first, how fast the mage moves, and how much time between shots during the attack.
    /// </summary>
    /// <param name="mage"></param>
    /// <param name="projectile"></param>
    /// <param name="numberOfBackForths"></param>
    /// <param name="movePoints"></param>
    /// <param name="moveRight"></param>
    /// <param name="moveTime"></param>
    /// <param name="shootCooldown"></param>
    public MageFlailAction(Transform mage, GameObject projectile, int numberOfBackForths, Transform[] movePoints, bool moveRight, float moveTime, float shootCooldown)
    {
        this.mage = mage;
        this.projectile = projectile;
        this.numberOfBackForths = numberOfBackForths;
        this.movePoints = movePoints;
        this.moveRight = moveRight;
        this.moveTime = moveTime;
        this.shootCooldown = shootCooldown;

        FlailAttack = Empty;
    }

    public void Execute()
    {
        executing = true;


        //StartCoroutine(StartMoving());
    }

    public void Stop()
    {

    }


    public void Empty(MageFlailAction action)
    {

    }
    
}
