using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31.StateKit;

public class ShipMoveRightState : SKMecanimState<PlayerController> {

    public override void begin() {
        _machine.animator.SetBool("moveLeft", true);
        _machine.animator.SetBool("moveRight", true);
    }

    public override void reason() {
        if (Input.GetAxisRaw("Horizontal") == 0)
            _machine.changeState<ShipIdleState>();

        else if (Input.GetKeyDown("2") && !_context.isTransfering)
        {
            _context.isTransfering = true;
            _context.StartCoroutine("TransferToCannon");
            _machine.changeState<CannonIdleState>();
        }
    }

    public override void update(float deltaTime, AnimatorStateInfo stateInfo) {

    }

    public override void end() {
        _machine.animator.SetBool("moveRight", false);
    }
}
