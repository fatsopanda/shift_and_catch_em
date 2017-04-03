using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {

    public enum PooledObject
    {
        Bullet,
        MeteorObject_1,
        MeteorObject_2,
        MeteorObject_3,
        SushiObject,
        BurgerObject,
        DonutObject,
        BackgroundMeteorObject
    };

    [Header("Bullets", order=1)]
    [SerializeField] int m_bulletLimit = 100;
    [SerializeField] GameObject m_bulletObject;
    [SerializeField] List<GameObject> m_bulletPool;
    [Space(10)]

    [Header("Meteor Objects", order=2)]
    [SerializeField] int m_meteorLimit = 24;
    [SerializeField] GameObject m_meteorObject1;
    [SerializeField] List<GameObject> m_meteorObjectPool1;
    [SerializeField] GameObject m_meteorObject2;
    [SerializeField] List<GameObject> m_meteorObjectPool2;
    [SerializeField] GameObject m_meteorObject3;
    [SerializeField] List<GameObject> m_meteorObjectPool3;
    [Space(10)]

    [Header("Collectables", order=3)]
    [SerializeField] int m_collectableLimit = 30;
    [SerializeField] GameObject m_sushiObject;
    [SerializeField] List<GameObject> m_sushiObjectPool;

    [SerializeField] GameObject m_burgerObject;
    [SerializeField] List<GameObject> m_burgerObjectPool;

    [SerializeField] GameObject m_donutObject;
    [SerializeField] List<GameObject> m_donutObjectPool;
    [Space(10)]

    [Header("Background Meteors", order=6)]
    [SerializeField] List<GameObject> m_backgroundMeteorObjects;
    [SerializeField] List<GameObject> m_backgroundMeteorObjectPool;

    void Start () {
        // Create pools for all of the used gameobjects
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            CreatePool(ref m_bulletPool, m_bulletObject, m_bulletLimit);

            CreatePool(ref m_meteorObjectPool1, m_meteorObject1, m_meteorLimit);
            CreatePool(ref m_meteorObjectPool2, m_meteorObject2, m_meteorLimit);
            CreatePool(ref m_meteorObjectPool3, m_meteorObject3, m_meteorLimit);

            CreatePool(ref m_sushiObjectPool, m_sushiObject, m_collectableLimit);
            CreatePool(ref m_burgerObjectPool, m_burgerObject, m_collectableLimit);
            CreatePool(ref m_donutObjectPool, m_donutObject, m_collectableLimit);
        }

        CreateBackgroundMeteorPool(ref m_backgroundMeteorObjectPool, ref m_backgroundMeteorObjects);
    }

    void CreatePool(ref List<GameObject> gameObjectPool, GameObject m_gameObject, int gameObjectPoolSize) {
        if (m_gameObject == null)
            Debug.Log("ERROR: No game object passed for object pool. Skipping!");
        else
        {
            gameObjectPool = new List<GameObject>();
            for (int i = 0; i < gameObjectPoolSize; i++)
            {
                GameObject tempObject = (GameObject)Instantiate(m_gameObject);
                tempObject.SetActive(false);
                gameObjectPool.Add(tempObject);
            }
        }
    }

    // Background Meteors are single objects in an array. Add them twice into the pool
    void CreateBackgroundMeteorPool(ref List<GameObject> backgroundPool, ref List<GameObject> backgroundMeteors) {
        backgroundPool = new List<GameObject>();
        int i = 0;
        int loopCounter = 0;

        while (loopCounter < 2)
        { 
            GameObject tempObject = (GameObject)Instantiate(backgroundMeteors[i]);
            tempObject.SetActive(false);
            backgroundPool.Add(tempObject);
            i++;

            if (i == backgroundMeteors.Count)
            {
                i = 0;
                loopCounter++;
            }
        }
    }

    public GameObject GetPooledObject(PooledObject m_object) {
        switch (m_object) {
            case PooledObject.Bullet:
            {
                for (int i = 0; i < m_bulletPool.Count; i++)
                {
                    if (!m_bulletPool[i].activeInHierarchy)
                        return m_bulletPool[i];
                }
            }
            break;

            case PooledObject.MeteorObject_1:
            {
                for (int i = 0; i < m_meteorObjectPool1.Count; i++)
                {
                    if (!m_meteorObjectPool1[i].activeInHierarchy)
                        return m_meteorObjectPool1[i];
                }
            }
            break;

            case PooledObject.MeteorObject_2:
            {
                for (int i = 0; i < m_meteorObjectPool2.Count; i++)
                {
                    if (!m_meteorObjectPool2[i].activeInHierarchy)
                        return m_meteorObjectPool2[i];
                }
            }
            break;

            case PooledObject.MeteorObject_3:
            {
                for (int i = 0; i < m_meteorObjectPool3.Count; i++)
                {
                    if (!m_meteorObjectPool3[i].activeInHierarchy)
                        return m_meteorObjectPool3[i];
                }
            }
            break;

            case PooledObject.SushiObject:
            {
                for (int i = 0; i < m_sushiObjectPool.Count; i++)
                {
                    if (!m_sushiObjectPool[i].activeInHierarchy)
                        return m_sushiObjectPool[i];
                }
            }
            break;

            case PooledObject.BurgerObject:
            {
                for (int i = 0; i < m_burgerObjectPool.Count; i++)
                {
                    if (!m_burgerObjectPool[i].activeInHierarchy)
                        return m_burgerObjectPool[i];
                }
            }
            break;

            case PooledObject.DonutObject:
            {
                for (int i = 0; i < m_donutObjectPool.Count; i++)
                {
                    if (!m_donutObjectPool[i].activeInHierarchy)
                        return m_donutObjectPool[i];
                }
            }
            break;

            case PooledObject.BackgroundMeteorObject:
            {
                for (int i = 0; i < m_backgroundMeteorObjectPool.Count; i++)
                {
                    if (!m_backgroundMeteorObjectPool[i].activeInHierarchy)
                        return m_backgroundMeteorObjectPool[i];
                }
            }
            break;
        }
        return null;
    }
}
