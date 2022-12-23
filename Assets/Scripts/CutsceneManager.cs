using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private GameObject player;
    private PlayerStats pstats;
    private PlayerMovement pmove;
    private Aim paim;
    private Rigidbody2D prb;

    [SerializeField] private GameObject pivotPoint;

    [SerializeField] private GameObject boss;
    private BossAnimations banim;
    private BossStats bstats;

    [SerializeField] private BossCamera bcamera;
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject playerStopPosition;

    [SerializeField] private GameObject bossSurprised;
    [SerializeField] private GameObject bossHealthBar;

    [Header("Variables")]
    [SerializeField] private float fadeSpeed = 5f;
    [SerializeField] private float playerMoveTime = 3f;
    [SerializeField] private float elapsedTime = 0f;
    [SerializeField] private bool cutsceneInProgress = false;
    [SerializeField] private float healthIncreaseSpeed = 10f;
    Vector3 playerStartingPosition;

    // Start is called before the first frame update
    void OnEnable()
    {
        fadeImage.gameObject.SetActive(true);
        pstats = player.GetComponent<PlayerStats>();
        pmove= player.GetComponent<PlayerMovement>();
        paim = player.GetComponent<Aim>();
        prb = player.GetComponent<Rigidbody2D>();
        banim = boss.GetComponent<BossAnimations>();
        bstats = boss.GetComponent<BossStats>();

        paim.enabled = false;
        pivotPoint.SetActive(false);

        pstats.InCutscene= true;
        pmove.InCutscene= true;
        banim.InCutscene= true;
        bstats.InCutscene= true;
        bcamera.InCutscene= true;

        playerStartingPosition = player.transform.position;
        cutsceneInProgress = true;

        StartCoroutine(PlayCutscene());
    }

   public IEnumerator PlayCutscene()
    {
        player.GetComponent<Animator>().SetBool("Cutscene", true);
        boss.GetComponent<Animator>().SetBool("Sleep", true);

        yield return new WaitForSeconds(0.5f);

        while(fadeImage.color.a > 0)
        {
            Color color= fadeImage.color;
            float fadeAmount = color.a - (fadeSpeed * Time.deltaTime);

            color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeAmount);
            fadeImage.color = color;

            yield return null;
        }

        yield return new WaitUntil(()=> !cutsceneInProgress);

        player.GetComponent<Animator>().SetBool("Cutscene", false);

        yield return new WaitForSeconds(1f);

        boss.GetComponent<SpriteRenderer>().flipX = true;

        yield return new WaitForSeconds(0.7f);

        boss.GetComponent<Animator>().SetBool("Sleep", false);
        bossSurprised.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        bossSurprised.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        banim.SwitchAttacking();
        banim.Attack();

        bossHealthBar.SetActive(true);

        while(bstats.GetCurrentHealth() < 100)
        {
            bstats.UpdateHealth(healthIncreaseSpeed * Time.deltaTime);
            yield return null;
        }

        banim.SwitchAttacking();
        boss.GetComponent<Animator>().SetBool("Surprised", false) ;

        yield return new WaitForSeconds(0.5f);

        bcamera.CutSceneEnding();

        yield return new WaitUntil(() => !bcamera.GetCutsceneEnding());

        CutsceneOver();

        yield return null;
    }

    public void Update()
    {
        if(!cutsceneInProgress) { return; }

        elapsedTime += Time.deltaTime;
        float percentComplete = elapsedTime / playerMoveTime;

        player.transform.position = Vector3.Lerp(playerStartingPosition, playerStopPosition.transform.position, percentComplete);

        if(player.transform.position.x >= playerStopPosition.transform.position.x)
        {
            cutsceneInProgress= false;
        }
    }

    public void CutsceneOver()
    {
        cutsceneInProgress= false;
        paim.enabled = true;
        pivotPoint.SetActive(true);

        pstats.InCutscene = false;
        pmove.SetIsJumping(false);
        pmove.InCutscene = false;
        banim.InCutscene = false;
        bstats.InCutscene = false;
        bcamera.InCutscene = false;
    }
}
