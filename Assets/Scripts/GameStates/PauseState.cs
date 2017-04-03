using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Prime31.StateKit;

public class PauseState : SKState<GameManager> {

    private bool paused;
    private UIManager   m_UIManager  = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();

    public override void begin() {
        paused = true;

        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
           m_UIManager.EnablePausePanel(true);
            if (_context.musicAudioSource != null)
                _context.musicAudioSource.volume = 0.5f;
        }
    }

    public override void reason() {
        if (paused && Input.GetKeyDown(KeyCode.Escape))
        {
            m_UIManager.EnablePausePanel(false);
            if (_context.musicAudioSource != null)
                _context.musicAudioSource.volume = 1.0f;
            Time.timeScale = 1;
            paused = false;

            _machine.changeState<PlayingState>();
        }

        else if (paused && Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1;
            if (_context.musicAudioSource != null)
                _context.musicAudioSource.volume = 1.0f;
            SceneManager.LoadScene(0);
        }
    }

    public override void update(float deltaTime) {
    }

    public override void end() {
        paused = false;
    }
}
