using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Prime31.StateKit;

public class GameManager : MonoBehaviour {
    [SerializeField] GameObject              m_playerObject;
    [SerializeField] SpriteRenderer          m_playerObjectSprite;
    [SerializeField] PlayerController        m_playerController;
    [SerializeField] GameObject              m_musicPlayerObject;
    [SerializeField] MusicPlayer             m_musicPlayer;

    [SerializeField] SpawnerEventHandler     m_meteorSpawner1;
    [SerializeField] SpawnerEventHandler     m_meteorSpawner2;
    [SerializeField] SpawnerEventHandler     m_collectableSpawner;
    [SerializeField] AudioManager            m_audioManager;
    [SerializeField] CameraManager           m_cameraManager;
    [SerializeField] UIManager               m_UIManager;

    public AudioSource                       musicAudioSource;

    public int                               playerScore;
    public int                               previousPlayerScore;
    public int                               playerHealth;

    public bool                              playerIsDead;
    public bool                              playerIsHealed;
    public bool                              isWaitingPlayer;

    private SKStateMachine<GameManager> m_stateMachine;

    void Awake() {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            m_meteorSpawner1 = GameObject.Find("MeteorSpawner1").GetComponent<SpawnerEventHandler>();
            m_meteorSpawner2 = GameObject.Find("MeteorSpawner2").GetComponent<SpawnerEventHandler>();
            m_collectableSpawner = GameObject.Find("CollectSpawner1").GetComponent<SpawnerEventHandler>();

            m_playerObject = GameObject.FindWithTag("Player");
            m_playerController = m_playerObject.GetComponent<PlayerController>();
            m_playerObjectSprite = m_playerObject.GetComponent<SpriteRenderer>();

            m_cameraManager = Camera.main.GetComponent<CameraManager>();
            m_audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
            m_UIManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();

            m_musicPlayerObject = GameObject.FindWithTag("MusicPlayer");
        }

        if (m_musicPlayerObject != null)
        {
            m_musicPlayer = m_musicPlayerObject.GetComponent<MusicPlayer>();
            musicAudioSource =  m_musicPlayerObject.GetComponent<AudioSource>();
        }

        isWaitingPlayer = false;
    }

    void Start () {
        if (m_musicPlayerObject != null && !musicAudioSource.isPlaying)
            m_musicPlayer.UnPauseMusic();

        playerScore  = 0;
        previousPlayerScore = playerScore;
        playerHealth = 3;

        playerIsHealed      = false;
        playerIsDead        = false;
        isWaitingPlayer     = false;

        m_stateMachine = new SKStateMachine<GameManager>(this, new PlayingState() );
        m_stateMachine.addState(new PauseState());
        m_stateMachine.addState(new GameOverState());
    }

    void Update () {
        m_stateMachine.update(Time.deltaTime);
    }

    IEnumerator RestartLevel() {
        playerIsDead = true;

        if (m_musicPlayerObject != null)
            m_musicPlayer.PauseMusic();

        m_meteorSpawner1.enabled     = false;
        m_meteorSpawner2.enabled     = false;
        m_collectableSpawner.enabled = false;

        m_playerObjectSprite.enabled = false;

        m_UIManager.DeathFlash();

        m_audioManager.Play(Random.Range(0, 4));
        m_cameraManager.ScreenShake(CameraManager.ShakeStrength.Big);

        m_playerController.EnableCollider(false);
        m_playerController.InitShipParticles();

        yield return new WaitForSeconds(2);

        m_playerController.enabled = false;
        m_playerObject.SetActive(false);

        yield return new WaitForSeconds (3);

        m_UIManager.EnableClosurePanel(true);
        isWaitingPlayer = true;

        yield return new WaitForSeconds (120);

        SceneManager.LoadScene(0);
    }


    IEnumerator ResetHeal() {
        yield return new WaitForSeconds (1);
        playerIsHealed = false;
    }

    public void HitPlayer(int hit) {
        playerHealth -= 1;
        m_playerController.PlayHitFlash();
    }

    public void HealPlayer(int heal) {
        playerHealth += 1;
        playerIsHealed = true;
        m_audioManager.Play(17);
        StartCoroutine("ResetHeal");
    }

    public void AddPoints(int points) {
        playerScore += points;
    }
}
