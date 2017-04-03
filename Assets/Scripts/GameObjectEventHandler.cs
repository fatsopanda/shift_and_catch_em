using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectEventHandler : MonoBehaviour {

    [SerializeField] GameManager       m_gameManager;
    [SerializeField] List<GameObject>  m_particles;

    [SerializeField] SpriteRenderer    m_spriteRenderer;
    [SerializeField] Rigidbody2D       m_rigidbody2D;
    [SerializeField] PolygonCollider2D m_collider2D;

    [SerializeField] CameraManager     m_cameraManager;
    [SerializeField] AudioManager      m_audioManager;
    [SerializeField] PlayerController  m_playerObject;

    [SerializeField] int               m_objectHealth;
    [SerializeField] int               m_objectStartHealth;
    [SerializeField] int               m_objectScore;

    [SerializeField] float             m_objectMoveSpeed;
    [SerializeField] float             m_objectRotateSpeed;
    [SerializeField] float             m_hitImpulse;

    [SerializeField] bool              m_isPooled;
    [SerializeField] bool              m_isExploding;
    [SerializeField] bool              m_inContact;
    [SerializeField] bool              m_isCollectable;

    void Awake() {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody2D    = GetComponent<Rigidbody2D>();
        m_collider2D     = GetComponent<PolygonCollider2D>();
        m_cameraManager  = Camera.main.GetComponent<CameraManager>();
        m_gameManager    = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_audioManager   = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        m_playerObject   = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Start () {
        m_isPooled          = false;
        m_objectMoveSpeed   = Random.Range(1, 4);
        m_objectRotateSpeed = Random.Range(-4f, 4f);

        if (gameObject.tag == "SmallMeteor")
        {
            m_objectStartHealth = 1;
            m_objectScore = 25;
            m_isCollectable = false;
        }
        else if (gameObject.tag == "BigMeteor")
        {
            m_objectStartHealth = 2;
            m_objectScore = 50;
            m_isCollectable = false;
        }
        else if (gameObject.tag == "Sushi" || gameObject.tag == "Burger" || gameObject.tag == "Donut")
        {
            m_objectStartHealth = 2;
            m_objectScore = 100;
            m_isCollectable = true;
        }

        m_objectHealth = m_objectStartHealth;
        DisableParticles();
    }

    void OnEnable() {
        m_isExploding  = false;
        m_inContact    = false; 

        if (!m_spriteRenderer.enabled) 
            m_spriteRenderer.enabled = true;

        if (m_rigidbody2D.isKinematic)
            m_rigidbody2D.isKinematic = false;

        if (!m_collider2D.enabled)
            m_collider2D.enabled = true;

        if (m_isPooled)
        {
            m_objectHealth = m_objectStartHealth;
            m_objectMoveSpeed = Random.Range(1, 4);
            m_objectRotateSpeed   = Random.Range(-4f, 4f);
        }
    }

    void Update () {
        m_rigidbody2D.velocity = new Vector2(0f, -m_objectMoveSpeed);
        transform.Rotate(0, 0, m_objectRotateSpeed);

        if (m_objectHealth <= 0 && !m_isExploding)
            StartCoroutine("Explode");
    }

    void OnTriggerEnter2D (Collider2D other) {
        if (!m_isCollectable)
        {
            if (other.tag == "Player" && !m_isExploding)
            {
                m_gameManager.HitPlayer(1);
                m_objectHealth -= 1;
            }

            if (gameObject.tag == "SmallMeteor" && !m_inContact && (other.tag == "SmallMeteor" || other.tag == "BigMeteor"))
            {
                m_objectMoveSpeed += Random.Range(2, 3);
                m_inContact = true;
            }
        }
        else if (m_isCollectable)
        {
            if (other.tag == "Player" && m_playerObject.playerMode == PlayerController.PlayerMode.Ship)
            {
                m_gameManager.AddPoints(m_objectScore);
                m_audioManager.Play(13);
                gameObject.SetActive(false);
            }

            if (other.tag == "Player" && m_playerObject.playerMode == PlayerController.PlayerMode.Cannon)
            {
                m_objectScore   = 0;
                m_objectHealth -= m_objectStartHealth;
            }
        }

        if (other.tag == "Bullet")
        {
            if (gameObject.tag == "SmallMeteor")
                m_objectHealth -= 1;
            else if (gameObject.tag == "BigMeteor")
            {
                m_objectHealth -= 1;
                StartCoroutine("HitFlash");
            }
            else if (m_isCollectable)
            {
                m_objectHealth -= 1;
                StartCoroutine("HitFlash");
            }
        }
    }

    void DisableParticles() {
        for (int i = 0; i < m_particles.Count; i++)
            m_particles[i].SetActive(false);
    }

    IEnumerator HitFlash() {
        m_spriteRenderer.material.SetFloat("_FlashAmount", 1);

        if (m_audioManager != null)
            m_audioManager.Play(Random.Range(10, 12) );

        yield return new WaitForSeconds ( 0.05f );

        m_spriteRenderer.material.SetFloat("_FlashAmount", 0);
    }

    IEnumerator Explode() {
        m_isExploding = true;

        m_gameManager.AddPoints(m_objectScore);

        m_spriteRenderer.enabled = false;
        m_collider2D.enabled = false;
        m_rigidbody2D.isKinematic = true;

        if (m_audioManager != null && gameObject.tag == "SmallMeteor")
        {
            m_audioManager.Play(Random.Range(4, 6));

            for (int i = 0; i < m_particles.Count; i++)
            {
                m_particles[i].transform.position = transform.position;
                m_particles[i].SetActive(true);
            }
        }
        else if (m_audioManager != null && gameObject.tag == "BigMeteor")
        {
            m_audioManager.Play(6);
            
            for (int i = 0; i < m_particles.Count; i++)
            {
                m_particles[i].transform.position = transform.position;
                m_particles[i].SetActive(true);
            }
        }
        else if (m_audioManager != null && m_isCollectable)
        {
            m_audioManager.Play(7);

            for (int i = 0; i < m_particles.Count; i++)
            {
                m_particles[i].transform.position = transform.position;
                m_particles[i].SetActive(true);
            }
        }

        m_cameraManager.ScreenShake(CameraManager.ShakeStrength.Normal);

        yield return new WaitForSeconds ( 2.5f );

        m_isPooled = true;
        DisableParticles();
        gameObject.SetActive(false);
    }
}
