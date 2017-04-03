using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager: MonoBehaviour {

    [SerializeField] GameManager    m_gameManager;

    [SerializeField] GameObject     m_menuPanel;
    [SerializeField] GameObject     m_creditsPanel;
    [SerializeField] GameObject     m_infoPanel;
    [SerializeField] GameObject     m_backButton;

    [SerializeField] Image          m_heartImage1;
    [SerializeField] Image          m_heartImage2;
    [SerializeField] Image          m_heartImage3;

    [SerializeField] Image          m_deathFlashImage;

    [SerializeField] Image          m_pauseImage;
    [SerializeField] Text           m_pauseText;
    [SerializeField] Text           m_pauseContinueText;

    [SerializeField] Image          m_closureImage;
    [SerializeField] Text           m_closureText;
    [SerializeField] Text           m_closureContinueText;

    [SerializeField] int            m_playerHealth;
    [SerializeField] int            m_playerScore;

    [SerializeField] bool           m_menuScene;
    [SerializeField] bool           m_showCredits;
    [SerializeField] bool           m_showHowto;
    [SerializeField] bool           m_quit;

    Text scoreText;

    void Awake() {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            m_menuScene = true;
        else
            m_menuScene = false;

        if (m_menuScene)
        {
            m_backButton = GameObject.Find("BackButton");

            m_menuPanel = GameObject.Find("MenuPanel");
            m_creditsPanel = GameObject.Find("CreditsPanel");
            m_infoPanel = GameObject.Find("InstructionsPanel");

            m_backButton.SetActive(false);
            m_creditsPanel.SetActive(false);
            m_infoPanel.SetActive(false);
            m_quit = true;
        }
        else if (!m_menuScene)
        {
            m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            m_heartImage1 = GameObject.Find("Heart1").GetComponent<Image>();
            m_heartImage2 = GameObject.Find("Heart2").GetComponent<Image>();
            m_heartImage3 = GameObject.Find("Heart3").GetComponent<Image>();

            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

            m_deathFlashImage = GameObject.Find("DeathFlashPanel").GetComponent<Image>();
            m_deathFlashImage.enabled = false;

            m_pauseImage = GameObject.Find("PausePanel").GetComponent<Image>();
            m_pauseText = GameObject.Find("Paused").GetComponent<Text>();
            m_pauseContinueText = GameObject.Find("WannaContinue").GetComponent<Text>();
            m_pauseText.enabled = false;
            m_pauseContinueText.enabled = false;

            m_closureImage = GameObject.Find("ClosurePanel").GetComponent<Image>();
            m_closureText = GameObject.Find("TryAgain").GetComponent<Text>();
            m_closureText.enabled = false;

            m_closureContinueText = GameObject.Find("ContinueTxt").GetComponent<Text>();
            m_closureContinueText.enabled = false;
        }
    }

    void Update () {
        if (m_menuScene)
        {
            if (Input.GetKeyDown("escape") && (m_showHowto || m_showCredits))
                PressBackButton();

            if (Input.GetKeyDown("escape") && !m_showHowto && !m_showCredits && m_quit)
                Application.Quit();
        }
        else if (!m_menuScene)
            UpdateGameUI();
    }

    public void PressHowtoButton() {
        m_menuPanel.SetActive(false);
        m_backButton.SetActive(true);
        m_infoPanel.SetActive(true);
        m_showHowto = true;
        m_quit = false;
    }

    public void PressCreditsButton() {
        m_menuPanel.SetActive(false);
        m_backButton.SetActive(true);
        m_creditsPanel.SetActive(true);
        m_showCredits = true;
        m_quit = false;
    }
    public void PressBackButton() {
        if (m_showHowto)
        {
            m_infoPanel.SetActive(false);
            m_backButton.SetActive(false);
            m_menuPanel.SetActive(true);
            m_showHowto = false;
        }
    
        if (m_showCredits) 
        {
            m_creditsPanel.SetActive(false);
            m_backButton.SetActive(false);
            m_menuPanel.SetActive(true);
            m_showCredits = false;
        }
        StartCoroutine("QuitDelay");
    }

    public void PressPlayButton() {
        SceneManager.LoadScene(1);
    }

    public void DeathFlash() {
        StartCoroutine("ShowDeathFlash");
    }

    public void EnablePausePanel(bool isEnabled) {
        if (isEnabled)
        {
            m_pauseImage.enabled = true;
            m_pauseText.enabled = true;
            m_pauseContinueText.enabled = true;
        }
        else if (!isEnabled)
        {
            m_pauseImage.enabled = false;
            m_pauseText.enabled = false;
            m_pauseContinueText.enabled = false;
        }
    }

    public void EnableClosurePanel(bool isEnabled) {
        if (isEnabled)
        {
            m_closureImage.enabled = true;
            m_closureText.enabled = true;
            m_closureContinueText.enabled = true;
        }
        else if (!isEnabled)
        {
            m_closureImage.enabled = false;
            m_closureText.enabled = false;
            m_closureContinueText.enabled = false;
        }
    }

    IEnumerator QuitDelay() {
        yield return new WaitForSeconds ( 2 );
        m_quit = true;
    }

    IEnumerator ShowDeathFlash() {
        m_deathFlashImage.enabled = true;
        yield return new WaitForSeconds ( 0.2f );
        m_deathFlashImage.enabled = false;
    }

    void UpdateGameUI() {
        m_playerScore = m_gameManager.playerScore;
        m_playerHealth = m_gameManager.playerHealth;

        switch (m_playerHealth) {
            case (3):
            {
                m_heartImage1.enabled = true;
                m_heartImage2.enabled = true;
                m_heartImage3.enabled = true;
            }
            break;

            case (2):
            {
                m_heartImage3.enabled = false;
                m_heartImage1.enabled = true;
                m_heartImage2.enabled = true;
            }
            break;

            case (1):
            {
                m_heartImage3.enabled = false;
                m_heartImage2.enabled = false;
                m_heartImage1.enabled = true;
            }
            break;

            case (0):
            {
                m_heartImage3.enabled = false;
                m_heartImage2.enabled = false;
                m_heartImage1.enabled = false;
            }
            break;
        }
        scoreText.text = "" + m_playerScore;
    }
}
