using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviourSingletonPersistent<MusicPlayer> {

    public GameObject m_musicPlayerObject;
    public AudioSource m_musicAudioSource;

    void Start() {
            m_musicPlayerObject = GameObject.Find("MusicPlayer");
            m_musicAudioSource = m_musicPlayerObject.GetComponent<AudioSource>();
            PlayMusic();
    }

    void Update() {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!GetComponent<AudioSource>().isPlaying)
                UnPauseMusic();
        }
    }

    public void PlayMusic() {
        m_musicAudioSource.Play();
    }

    public void UnPauseMusic() {
        m_musicAudioSource.UnPause();
    }

    public void PauseMusic() {
        m_musicAudioSource.Pause();
    }
}
