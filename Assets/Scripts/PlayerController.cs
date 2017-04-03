using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31.StateKit;

public class PlayerController : MonoBehaviour {

    [SerializeField] Rigidbody2D      m_rigidbody2D;
    [SerializeField] Animator         m_animator;
    [SerializeField] AudioSource      m_laserSound;
    [SerializeField] SpriteRenderer   m_spriteRenderer;
    [SerializeField] BoxCollider2D    m_collider2D;

    [SerializeField] List<GameObject> m_shipParticles;
    [SerializeField] List<GameObject> m_cannonParticles;

    [SerializeField] AudioManager     m_audioManager;
    [SerializeField] CameraManager    m_cameraManager;
    [SerializeField] PoolManager      m_gameObjectPool;

    [SerializeField] GameObject       m_bullet;
    [SerializeField] GameObject       m_bulletSpawn;

    [SerializeField] float            m_velocity;
    [SerializeField] float            m_speed;

    public bool                       isShooting;
    public bool                       isTransfering;

    public enum PlayerMode { Ship, Cannon };
    public      PlayerMode playerMode;

    private SKMecanimStateMachine<PlayerController> m_stateMachine;

    void Start () {
        m_rigidbody2D      = GetComponent<Rigidbody2D>();
        m_animator         = GetComponent<Animator>();
        m_laserSound       = GetComponent<AudioSource>();
        m_spriteRenderer   = GetComponent<SpriteRenderer>();
        m_collider2D       = GetComponent<BoxCollider2D>();

        m_audioManager     = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        m_cameraManager    = Camera.main.GetComponent<CameraManager>();
        m_gameObjectPool   = GameObject.Find("PoolManager").GetComponent<PoolManager>();

        m_bulletSpawn      = GameObject.Find("BulletSpawn");

        var animator = GetComponent<Animator>();

        m_stateMachine = new SKMecanimStateMachine<PlayerController>(animator, this, new ShipIdleState());

        m_stateMachine.addState(new ShipMoveLeftState());
        m_stateMachine.addState(new ShipMoveRightState());
        m_stateMachine.addState(new CannonIdleState());
        m_stateMachine.addState(new CannonShootState());

        playerMode = PlayerMode.Ship;

        DisableParticles(ref m_shipParticles);
        DisableParticles(ref m_cannonParticles);
    }

    void Update() {
        m_stateMachine.update(Time.deltaTime);

        if (playerMode == PlayerMode.Ship)
            m_speed = 9.5f;
        else if (playerMode == PlayerMode.Cannon)
            m_speed = 7.5f;

        m_velocity = (Input.GetAxisRaw("Horizontal") * m_speed);

        m_rigidbody2D.velocity = new Vector2(m_velocity, 0.0f);

        m_bulletSpawn.transform.parent = transform;
        m_bulletSpawn.transform.localPosition = new Vector2(0.0f, 1.46f);
    }

    void DisableParticles(ref List<GameObject> m_particles) {
        for (int i = 0; i < m_particles.Count; i++)
            m_particles[i].SetActive(false);
    }

    IEnumerator ShootBullet() {
        yield return new WaitForSeconds(0.20f);
        m_bullet = m_gameObjectPool.GetPooledObject(PoolManager.PooledObject.Bullet);
        m_bullet.transform.position = m_bulletSpawn.transform.position;
        m_bullet.SetActive(true);
        m_laserSound.Play();
        m_cameraManager.ScreenShake(CameraManager.ShakeStrength.Small);
        isShooting = false;
    }

    IEnumerator TransferToCannon() {
        m_animator.SetBool("transformingToCannon", true);
        m_audioManager.Play(15);
        yield return new WaitForSeconds(0.25f);
        playerMode = PlayerMode.Cannon;
        m_animator.SetBool("transformingToCannon", false);
        isTransfering = false;
    }

    IEnumerator TransferToShip() {
        // Due to an animation cancelling bug we make sure here that we are not playing shooting animation before playing the transform animation
        m_animator.SetBool("shoot", false);
        m_animator.SetBool("isCannon", false);
        m_animator.SetBool("transformingToShip", true);
        m_audioManager.Play(16);
        yield return new WaitForSeconds(0.25f);
        playerMode = PlayerMode.Ship;
        m_animator.SetBool("transformingToShip", false);
        isTransfering = false;
    }

    IEnumerator HitFlash() {
        m_spriteRenderer.material.SetFloat("_FlashAmount", 1);
        yield return new WaitForSeconds ( 0.05f );
        m_spriteRenderer.material.SetFloat("_FlashAmount", 0);
    }

    public void EnableCollider(bool isEnabled) {
        if (isEnabled)
            m_collider2D.enabled = true;
        else if (!isEnabled)
            m_collider2D.enabled = false;
    }

    public void InitShipParticles() {
        if (playerMode == PlayerController.PlayerMode.Ship)
        {
            for (int i = 0; i < m_shipParticles.Count; i++)
            {
                m_shipParticles[i].transform.position = transform.position;
                m_shipParticles[i].SetActive(true);
            }
        }
        else if (playerMode == PlayerController.PlayerMode.Cannon)
        {
            for (int i = 0; i < m_cannonParticles.Count; i++)
            {
                m_cannonParticles[i].transform.position = transform.position;
                m_cannonParticles[i].SetActive(true);
            }
        }
    }

    public void PlayHitFlash() {
        StartCoroutine("HitFlash");
    }
}
