using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private TMP_Text skipCutscene;
    [SerializeField] private GameObject coinUI;
    [SerializeField] private GameObject healthUI;

    [Header("Variables")]
    [SerializeField] private float fadeSpeed = 5f;
    [SerializeField] private float playerMoveTime = 3f;
    [SerializeField] private float elapsedTime = 0f;
    [SerializeField] private bool cutsceneInProgress = false;
    [SerializeField] private bool playerWalking = true;
    [SerializeField] private float healthIncreaseSpeed = 10f;
    Vector3 playerStartingPosition;

    [Header("Game Over")]
    [SerializeField] private SpriteRenderer gameOverBackground;
    [SerializeField] private GameObject bossDeathLocation;
    [SerializeField] private float bossFallSpeed = 2f; //Smaller == faster
    [SerializeField] private Image[] YouWin;
    [SerializeField] private bool bossDying = false;
    [SerializeField] private Button QuitButton;
    [SerializeField] private AudioSource winMusic;
    [SerializeField] private AudioSource levelMusic;

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
        coinUI.SetActive(false);
        healthUI.SetActive(false);

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

        yield return new WaitUntil(()=> !playerWalking);

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

    public IEnumerator BossDefeated()
    {
        levelMusic.Stop();
        cutsceneInProgress = true;
        bossDying = true;
        paim.enabled = false;
        pivotPoint.SetActive(false);
        coinUI.SetActive(false);
        healthUI.SetActive(false);

        pstats.MakeInvincible();
        pstats.InCutscene = true;
        pmove.StopSounds();
        prb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        pmove.InCutscene = true;
        banim.InCutscene = true;
        bstats.InCutscene = true;
        bcamera.InCutscene = true;

        playerStartingPosition = player.transform.position;

        boss.GetComponent<Animator>().SetBool("Dead", true);
        bcamera.BossDead();

        yield return new WaitForSeconds(0.1f);

        while (gameOverBackground.color.a < 1)
        {
            Color color = gameOverBackground.color;
            float fadeAmount = color.a + (fadeSpeed/10 * Time.deltaTime);

            color = new Color(gameOverBackground.color.r, gameOverBackground.color.g, gameOverBackground.color.b, fadeAmount);
            gameOverBackground.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(0.8f);

        boss.GetComponent<Animator>().SetBool("Dead", false);
        boss.GetComponent<Animator>().SetBool("Sleep", true);

        elapsedTime = 0;
        Vector3 bossStartLocation = boss.transform.position;

        while(boss.transform.position.y > bossDeathLocation.transform.position.y)
        {
            elapsedTime += Time.deltaTime;
            float percentComplete = elapsedTime / bossFallSpeed;

            boss.transform.position = Vector3.Lerp(bossStartLocation, bossDeathLocation.transform.position, percentComplete);
            yield return null;
        }

        //Move back to player and show victory text
        winMusic.Play();

        yield return new WaitForSeconds(1f);

        bcamera.MoveToPlayer();

        yield return new WaitUntil(() => bcamera.transform.position.x == player.transform.position.x);

        bossHealthBar.SetActive(false);

        for(int i = 0; i < YouWin.Length; i++)
        {
            while (YouWin[i].color.a < 1)
            {
                Color color = YouWin[i].color;
                float fadeAmount = color.a + (fadeSpeed * Time.deltaTime);

                color = new Color(YouWin[i].color.r, YouWin[i].color.g, YouWin[i].color.b, fadeAmount);
                YouWin[i].color = color;
                yield return null;
            }

            yield return null;
        }

        //Enable quit button

        QuitButton.enabled = true;

        while(QuitButton.image.color.a < 1)
        {
            Color color = QuitButton.image.color;
            float fadeAmount = color.a + (fadeSpeed * Time.deltaTime);

            color = new Color(QuitButton.image.color.r, QuitButton.image.color.g, QuitButton.image.color.b, fadeAmount);
            QuitButton.image.color = color;
        }

        yield return null;
    }

    public void Update()
    {
        if(!cutsceneInProgress || bossDying) { return; }

        if (skipCutscene.alpha > 0)
        {
            skipCutscene.alpha -= 0.5f * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && cutsceneInProgress)
        {
            SkipCutscene();
        }

        elapsedTime += Time.deltaTime;
        float percentComplete = elapsedTime / playerMoveTime;

        player.transform.position = Vector3.Lerp(playerStartingPosition, playerStopPosition.transform.position, percentComplete);

        if(player.transform.position.x >= playerStopPosition.transform.position.x)
        {
            playerWalking= false;
        }
    }

    public void CutsceneOver()
    {
        cutsceneInProgress= false;
        paim.enabled = true;
        pivotPoint.SetActive(true);
        coinUI.SetActive(true);
        healthUI.SetActive(true);

        pstats.InCutscene = false;
        pmove.SetIsJumping(false);
        pmove.InCutscene = false;
        banim.InCutscene = false;
        bstats.InCutscene = false;
        bcamera.InCutscene = false;
    }

    public void SkipCutscene()
    {
        StopAllCoroutines();
        skipCutscene.alpha = 0;
        fadeImage.enabled = false;
        player.GetComponent<Animator>().SetBool("Cutscene", false);
        boss.GetComponent<Animator>().SetBool("Sleep", false);
        boss.GetComponent<Animator>().SetBool("Surprised", false);
        bossHealthBar.SetActive(true);

        bstats.UpdateHealth(100);
        CutsceneOver();
    }

    public void BossDead()
    {
            StartCoroutine(BossDefeated());
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
