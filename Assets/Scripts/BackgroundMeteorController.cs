using UnityEngine;
using System.Collections;

public class BackgroundMeteorController : MonoBehaviour {

    [SerializeField] float m_objectSpeed;
    [SerializeField] float m_objectRotateSpeed;
    [SerializeField] bool m_isPooled;

    [SerializeField] Rigidbody2D m_Rigidbody2D;

    void Awake() {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start () {
        m_objectSpeed = Random.Range(2, 6);
        m_objectRotateSpeed = Random.Range(-8f, -8f);
    }

    void OnEnable() {
        m_objectSpeed = Random.Range(2, 6);
        m_objectRotateSpeed = Random.Range(-8f, -8f);
    }

    void Update () {
        m_Rigidbody2D.velocity = new Vector2 (0.0f, -m_objectSpeed);
        transform.Rotate(0.0f, 0.0f, m_objectRotateSpeed);
    }
}
