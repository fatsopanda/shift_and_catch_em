using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Prime31.StateKit;

public class GameOverState : SKState<GameManager> {

    public override void begin() {
        _context.StartCoroutine("RestartLevel");
    }

    public override void reason() {
    }

    public override void update(float deltaTime) {
        if (Input.GetKeyDown("space") && _context.isWaitingPlayer)
            SceneManager.LoadScene(1);

        if (Input.GetKeyDown("escape") && _context.isWaitingPlayer)
            SceneManager.LoadScene(0);
    }
}
