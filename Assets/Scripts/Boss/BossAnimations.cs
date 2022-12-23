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

    [Header("Variables")]
    [SerializeField] private bool attacking = false;

    [Header("Cutscene Management")]
    [SerializeField] private bool inCutscene = false;

    public bool InCutscene { get { return inCutscene; } set { inCutscene = value; } }

    // Start is called before the first frame update
    void OnEnable()
    {
        //StartCoroutine(TestAnimations());
    }

    // Update is called once per frame
    void Update()
    {
        if (inCutscene) { return; }

        if (player.transform.position.x < transform.position.x)
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
        attacking = true;
        StartCoroutine(Vibrate());

        yield return new WaitForSeconds(5f);

        attacking = false;
        bossAnimator.SetBool("Attacking", false);

        StartCoroutine(TestAnimations());
    }

    public void Attack()
    {
        bossAnimator.SetBool("Attacking", true);
        StartCoroutine(Vibrate());
    }

    public void Dead()
    {

    }

    public void SwitchAttacking()
    {
        attacking = !attacking;
    }

    public IEnumerator Vibrate()
    {
        while (attacking)
        {
            transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, 0f);

            yield return new WaitForSeconds(0.1f);

            transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y, 0f);

            yield return new WaitForSeconds(0.1f);

            transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y, 0f);

            yield return new WaitForSeconds(0.1f);

            transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, 0f);

        }

        bossAnimator.SetBool("Attacking", false);
    }
}
