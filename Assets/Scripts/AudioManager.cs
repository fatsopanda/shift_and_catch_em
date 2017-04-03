using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    [SerializeField] AudioClip[] m_audioClip;
    [SerializeField] AudioSource m_audioSource;


    void Awake() {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void Play(int i) {
        m_audioSource.clip = m_audioClip[i];
        m_audioSource.Play();
    }
}
