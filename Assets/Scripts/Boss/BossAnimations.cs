using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimations : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animation idle;
    [SerializeField] private Animation attack;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator bossAnimator;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(TestAnimations());
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX= true;
        }
        else if (player.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
    }

    public IEnumerator TestAnimations()
    {
        yield return new WaitForSeconds(5f);

        bossAnimator.SetBool("Attacking", true);

        yield return new WaitForSeconds(5f);

        bossAnimator.SetBool("Attacking", false);

        StartCoroutine(TestAnimations());
    }
}
