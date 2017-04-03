using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31.StateKit;

public class ShipIdleState : SKMecanimState<PlayerController> {

    public override void begin() {
        _machine.animator.SetBool("isCannon", false);
        _machine.animator.SetBool("moveLeft", false);
        _machine.animator.SetBool("moveRight", false);
    }

    public override void reason() {
        if (Input.GetAxisRaw("Horizontal") < 0)
            _machine.changeState<ShipMoveLeftState>();

        else if (Input.GetAxisRaw("Horizontal") > 0)
            _machine.changeState<ShipMoveRightState>();

        else if (Input.GetKeyDown("2") && !_context.isTransfering)
        {
            _context.isTransfering = true;
            _context.StartCoroutine("TransferToCannon");
            _machine.changeState<CannonIdleState>();
        }
    }

    public override void update(float deltaTime, AnimatorStateInfo stateInfo) {

    }
}
