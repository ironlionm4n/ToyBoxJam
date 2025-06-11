using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JaggedBoneBlock : Lowerable
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
