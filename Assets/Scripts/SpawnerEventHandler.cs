using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEventHandler : MonoBehaviour {

    [SerializeField] PoolManager m_gameObjectPool;
    [SerializeField] GameObject  m_gameObject;
    [SerializeField] GameManager m_gameManager;

    [SerializeField] float       m_minXPosition;
    [SerializeField] float       m_maxXPosition;
    [SerializeField] float       m_minSpawnDelay;
    [SerializeField] float       m_maxSpawnDelay;
    [SerializeField] float       m_YPosition;

    [SerializeField] bool        m_increaseDifficulty;   

    [SerializeField] int         m_randomNumber;
    [SerializeField] int         m_previousPlayerScore;
    [SerializeField] int         m_difficultyIncreaseLimit;

    void Awake() {
        m_gameObjectPool = GameObject.FindWithTag("PoolManager").GetComponent<PoolManager>();
        m_gameManager    = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        m_minSpawnDelay = 1.5f;
        m_maxSpawnDelay = 2.5f;
        m_difficultyIncreaseLimit = 1500;

        if (gameObject.tag == "CollectSpawner")
        {
            m_minXPosition = -6.0f;
            m_maxXPosition = 6.0f;
            m_YPosition    = 12.35f;
        }
        else if (gameObject.tag == "MeteorSpawner")
        {
            m_minXPosition = -6.0f;
            m_maxXPosition = 6.0f;
            m_YPosition    = 12.35f;
        }
        else if (gameObject.tag == "BackgroundMeteorSpawner")
        {
            m_minXPosition  = -10.0f;
            m_maxXPosition  = 10.0f;
            m_minSpawnDelay = 0.25f;
            m_maxSpawnDelay = 1.0f;
            m_YPosition     = 11.0f;
        }
    }

    void Start () {
        if (gameObject.tag != "BackgroundMeteorSpawner")
            StartCoroutine("SpawnGameObject");
        else if (gameObject.tag == "BackgroundMeteorSpawner")
            StartCoroutine("SpawnBackgroundMeteors");

        m_previousPlayerScore = m_gameManager.playerScore;
        m_increaseDifficulty  = false;
    }

    void Update() {
        if (m_gameManager.playerScore >= (m_previousPlayerScore + m_difficultyIncreaseLimit) && !m_increaseDifficulty && gameObject.tag != "BackgroundMeteorSpawner")
        {
            m_previousPlayerScore = m_gameManager.playerScore;
            m_increaseDifficulty = true;
            IncreaseDifficulty();
        }

    }

    IEnumerator SpawnGameObject() {
        while (true)
        {
            yield return new WaitForSeconds( Random.Range(m_minSpawnDelay, m_maxSpawnDelay));
            Vector2 m_spawnPosition = new Vector2(Random.Range(m_minXPosition, m_maxXPosition), m_YPosition);
            m_randomNumber = Random.Range(0, 3);

            switch(m_randomNumber) {
            case 0:
            {
                if (gameObject.tag == "MeteorSpawner")
                    m_gameObject = m_gameObjectPool.GetPooledObject(PoolManager.PooledObject.MeteorObject_1);
                else if (gameObject.tag == "CollectSpawner")
                    m_gameObject = m_gameObjectPool.GetPooledObject(PoolManager.PooledObject.SushiObject);
            }
            break;

            case 1:
            {
                if (gameObject.tag == "MeteorSpawner")
                    m_gameObject = m_gameObjectPool.GetPooledObject(PoolManager.PooledObject.MeteorObject_2);
                else if (gameObject.tag == "CollectSpawner")
                    m_gameObject = m_gameObjectPool.GetPooledObject(PoolManager.PooledObject.BurgerObject);
            }
            break;

            case 2:
            {
                if (gameObject.tag == "MeteorSpawner")
                    m_gameObject = m_gameObjectPool.GetPooledObject(PoolManager.PooledObject.MeteorObject_3);
                else if (gameObject.tag == "CollectSpawner")
                    m_gameObject = m_gameObjectPool.GetPooledObject(PoolManager.PooledObject.DonutObject);
            }
            break;
            }

            if (m_gameObject != null)
            {
                m_gameObject.transform.position = m_spawnPosition;
                m_gameObject.SetActive(true);
            }
        }
    }

    IEnumerator SpawnBackgroundMeteors() {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(m_minSpawnDelay, m_maxSpawnDelay));
            Vector2 m_spawnPosition = new Vector2(Random.Range(m_minXPosition, m_maxXPosition), m_YPosition);
            m_gameObject = m_gameObjectPool.GetPooledObject(PoolManager.PooledObject.BackgroundMeteorObject);
            if (m_gameObject != null)
            {
                m_gameObject.transform.position = m_spawnPosition;
                m_gameObject.SetActive(true);
            }
        }
    }

    void OnDisable() {
        if (gameObject.tag != "BackgroundMeteorSpawner")
            StopCoroutine("SpawnGameObject");
        else if (gameObject.tag == "BackgroundMeteorSpawner")
            StopCoroutine("SpawnBackgroundMeteors");
    }

    void IncreaseDifficulty() {
        if (m_minSpawnDelay <= 0.0f)
            m_minSpawnDelay = 0.0f;
        else if (m_maxSpawnDelay <= 0.5f)
            m_maxSpawnDelay = 0.5f;
        else
        {
            m_minSpawnDelay -= 0.15f;
            m_maxSpawnDelay -= 0.15f;
        }

        m_increaseDifficulty = false;
    }
}
