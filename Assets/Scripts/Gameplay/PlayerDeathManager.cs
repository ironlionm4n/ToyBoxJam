using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDeathManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;
    private PlayerStats pstats;
    private PlayerMovement pmove;
    private Aim paim;
    private Rigidbody2D prb;

    [SerializeField] private GameObject pivotPoint;
    [SerializeField] private bool waitingRespawn = false;
    [SerializeField] private GameObject coinUI;
    [SerializeField] private GameObject healthUI;

    [Header("Boss (Only Assign in Boss Fight Scene)")]
    [SerializeField] private GameObject boss;
    private BossAnimations banim;
    private BossStats bstats;
    [SerializeField] private GameObject bossHealthbar;

    [Header("Camera")]
    [SerializeField] private GameObject camera;
    [SerializeField] private float cameraMoveTime = 2f;
    [SerializeField] private float elapsedTime = 0f;

    [Header("Death Background")]
    [SerializeField] private SpriteRenderer gameOverBackground;
    [SerializeField] private float fadeSpeed = 5f;

    [Header("Checkpoints")]
    [SerializeField] private Vector3 currentCheckpoint;

    [Header("Text")]
    [SerializeField] private Image[] Rise;
    [SerializeField] private Image[] respawn;

    [Header("General")]
    [SerializeField] private bool inCutscene = false;
    [SerializeField] private AudioSource levelMusic;
    [SerializeField] private AudioSource deathMusic;

    // Start is called before the first frame update
    void OnEnable()
    {
        pstats = player.GetComponent<PlayerStats>();
        pmove= player.GetComponent<PlayerMovement>();
        paim = player.GetComponent<Aim>();
        prb = player.GetComponent<Rigidbody2D>();

        if(boss != null)
        {
            banim = boss.GetComponent<BossAnimations>();
            bstats = boss.GetComponent<BossStats>();
        }

        currentCheckpoint = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (waitingRespawn)
        {
            prb.velocity = new Vector2(0, 0);
            pmove.StopSounds();
        }

        if(Input.GetKeyDown(KeyCode.Space) && inCutscene)
        {
            inCutscene= false;
            StopAllCoroutines();

            StartCoroutine(PlayerRespawn());
        }
    }

    //When the player dies from low health
    public void PlayerDied()
    {
        levelMusic.Stop();
        paim.enabled = false;
        pivotPoint.SetActive(false);
        coinUI.SetActive(false);
        healthUI.SetActive(false);

        if(bossHealthbar!= null) {
            bossHealthbar.SetActive(false);
        }

        pstats.InCutscene = true;
        pmove.InCutscene = true;

        if (boss != null)
        {
            banim.InCutscene = true;
            bstats.InCutscene = true;
            if(camera.GetComponent<BossCamera>() != null)
            {
                camera.GetComponent<BossCamera>().PlayerDying = true;
            }
        }

        StartCoroutine(DeathCutscene());
    }

    public IEnumerator DeathCutscene()
    {
        inCutscene= true;
        waitingRespawn = true;
        pmove.StopSounds();

        //Makes player float
        prb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        prb.velocity = new Vector2(0, 0);

        player.GetComponent<Animator>().SetBool("Dead", true);
        gameOverBackground.sortingOrder = 11;

        while (gameOverBackground.color.a < 1)
        {
            Color color = gameOverBackground.color;
            float fadeAmount = color.a + (fadeSpeed / 10 * Time.deltaTime);

            color = new Color(gameOverBackground.color.r, gameOverBackground.color.g, gameOverBackground.color.b, fadeAmount);
            gameOverBackground.color = color;

            yield return null;
        }

        deathMusic.Play();

        yield return new WaitForSeconds(0.2f);

        while (respawn[0].color.a < 1)
        {
            for (int i = 0; i < respawn.Length; i++)
            {
                Color color = respawn[i].color;
                float fadeAmount = color.a + (fadeSpeed * Time.deltaTime);

                color = new Color(respawn[i].color.r, respawn[i].color.g, respawn[i].color.b, fadeAmount);
                respawn[i].color = color;

            }
            
            yield return null;

        }

        Debug.Log("Waiting respawn");

        while(waitingRespawn )
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, 0);

            yield return new WaitForSeconds(0.5f);

            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.5f, 0);

            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator PlayerRespawn()
    {
        for(int i = 0; i < respawn.Length; i++)
            {
            Color color = respawn[i].color;
            float fadeAmount = 0;

            color = new Color(respawn[i].color.r, respawn[i].color.g, respawn[i].color.b, fadeAmount);
            respawn[i].color = color;

        }

        yield return new WaitForSeconds(1f);

        waitingRespawn = false;

        for (int i = 0; i < Rise.Length; i++)
        {
            while (Rise[i].color.a < 1)
            {
                Color color = Rise[i].color;
                float fadeAmount = color.a + (fadeSpeed * Time.deltaTime);

                color = new Color(Rise[i].color.r, Rise[i].color.g, Rise[i].color.b, fadeAmount);
                Rise[i].color = color;
                yield return null;
            }

            yield return null;
        }

        deathMusic.Stop();

        yield return new WaitForSeconds(0.5f);

        if (boss != null)
        {
            SceneManager.LoadScene("BossBattle");
        }
        else
        {
            levelMusic.Play();
            RespawnAtLastCheckpoint();
           // prb.gravityScale = 1;

            yield return new WaitForSeconds(1f);

            CutsceneOver();

            while (gameOverBackground.color.a > 0)
            {
                Color color = gameOverBackground.color;
                float fadeAmount = color.a - (fadeSpeed / 10 * Time.deltaTime);

                color = new Color(gameOverBackground.color.r, gameOverBackground.color.g, gameOverBackground.color.b, fadeAmount);
                gameOverBackground.color = color;

                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < Rise.Length; i++)
            {
                while (Rise[i].color.a > 0)
                {
                    Color color = Rise[i].color;
                    float fadeAmount = color.a - (fadeSpeed * Time.deltaTime);

                    color = new Color(Rise[i].color.r, Rise[i].color.g, Rise[i].color.b, fadeAmount);
                    Rise[i].color = color;
                    yield return null;
                }

                yield return null;
            }

        }
    }

    public void RespawnAtLastCheckpoint()
    {
        elapsedTime = 0f;
        player.transform.position = currentCheckpoint;
        camera.transform.position = currentCheckpoint;
        player.GetComponent<Animator>().SetBool("Dead", false);
    }

    public void SetCurrentCheckpoint(GameObject checkpoint)
    {
        currentCheckpoint = checkpoint.transform.position;
    }

    public void CutsceneOver()
    {
        paim.enabled = true;
        pivotPoint.SetActive(true);
        coinUI.SetActive(true);
        healthUI.SetActive(true);

        prb.constraints = RigidbodyConstraints2D.FreezeRotation;

        pstats.InCutscene = false;
        pmove.SetIsJumping(false);
        pmove.Respawned();
        pstats.Respawned();
        pmove.InCutscene = false;
        waitingRespawn = false;

        if (boss != null)
        {
            banim.InCutscene = false;
            bstats.InCutscene = false;

            if (camera.GetComponent<BossCamera>() != null)
            {
                camera.GetComponent<BossCamera>().PlayerDying = false;
            }
        }

       
    }

    public void Respawn()
    {
        waitingRespawn = false;
    }
}
