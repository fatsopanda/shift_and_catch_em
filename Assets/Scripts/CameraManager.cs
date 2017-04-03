using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public enum ShakeStrength { Small, Normal, Big };

    [SerializeField] float m_shakeAmount;
    [SerializeField] float m_smallShakeAmount;
    [SerializeField] float m_bigShakeAmount;
    [SerializeField] float m_travelTime;

    Vector3 m_cameraOrigin;

    void Start () {
        m_travelTime = 10.0f;
        m_cameraOrigin = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
    }

    void Update () {
        transform.position = Vector3.Lerp(transform.position, m_cameraOrigin, Time.deltaTime * m_travelTime);
    }

    public void ScreenShake(ShakeStrength m_shakeStrength) {
        switch (m_shakeStrength) {
            case ShakeStrength.Small:
            {
                float m_shakeLength = 0.1f;
                StartCoroutine(ScreenShake(m_shakeLength, m_smallShakeAmount));
            }
            break;

            case ShakeStrength.Normal:
            {
                float m_shakeLength = 0.3f;
                StartCoroutine(ScreenShake(m_shakeLength, m_shakeAmount));
            }
            break;

            case ShakeStrength.Big:
            {
                float m_shakeLength = 0.5f;
                StartCoroutine(ScreenShake(m_shakeLength, m_bigShakeAmount));
            }
            break;
        }
    }

    IEnumerator ScreenShake(float m_shakeLength, float m_shakeAmount) {
        float m_elapsedTime = 0.0f;
        while (m_elapsedTime < m_shakeLength)
        {
            m_elapsedTime += Time.deltaTime;
            transform.position = transform.position + Random.insideUnitSphere * m_shakeAmount;
            yield return null;
        }
    }
}
