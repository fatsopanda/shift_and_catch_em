using UnityEngine;
using System.Collections;

public class BulletEventHandler : MonoBehaviour {

    [SerializeField] float m_speed;
    [SerializeField] float m_velocity;
    [SerializeField] Rigidbody2D m_rigidbody2D;
    [SerializeField] BoxCollider2D m_boxCollider2D;

    void Awake() {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start () {
        m_velocity = m_speed;
        m_rigidbody2D.velocity = new Vector2(0f, m_velocity);
        StartCoroutine("DestroyYourself");
    }

    void OnEnable() {
        if (!m_boxCollider2D.enabled)
            m_boxCollider2D.enabled = true;

        m_rigidbody2D.velocity = new Vector2(0f, m_velocity);

        // Destroy every shot bullet by latest after 1.5 seconds has passed (in the coroutine)
        StartCoroutine("DestroyYourself");
    }

    void OnTriggerEnter2D(Collider2D other) {
        if ( other.tag == "Sushi" || other.tag == "Burger" || other.tag == "Donut" || other.tag == "SmallMeteor" || other.tag == "BigMeteor" )
        {
            m_boxCollider2D.enabled = false;
            m_rigidbody2D.velocity = new Vector2(0f, 0f);
            StartCoroutine("DestroyYourself");
        }
    }

    IEnumerator DestroyYourself() {
        if (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds ( 1.5f );
            gameObject.SetActive(false);
        }
    }
}
