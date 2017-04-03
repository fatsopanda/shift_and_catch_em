using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Prime31.StateKit;

public class PlayingState : SKState<GameManager> {

    public override void begin() {

    }

    public override void reason() {
        if (Input.GetKeyDown(KeyCode.Escape) && !_context.isWaitingPlayer && SceneManager.GetActiveScene().buildIndex != 0)
            _machine.changeState<PauseState>();

        else if (_context.playerHealth == 0 && !_context.playerIsDead)
            _machine.changeState<GameOverState>();
    }

    public override void update(float deltaTime) {
        if (_context.playerHealth <= 0)
            _context.playerHealth = 0;

        else if (_context.playerScore >= ( _context.previousPlayerScore + 1500 ) && !_context.playerIsHealed && _context.playerHealth < 3)
        {
            _context.previousPlayerScore = _context.playerScore;
            _context.HealPlayer(1);
        }
    }
}
