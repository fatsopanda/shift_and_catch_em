using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31.StateKit;

public class CannonIdleState : SKMecanimState<PlayerController> {

    public override void begin() {
        _machine.animator.SetBool("isCannon", true);
    }

    public override void reason() {
        if (Input.GetKeyDown("space") && !_context.isShooting)
        {
            _context.isShooting = true;
            _machine.changeState<CannonShootState>();
        }

        else if (Input.GetKeyDown("1") && !_context.isTransfering && !_context.isShooting)
        {
            _context.isTransfering = true;
            _context.StartCoroutine("TransferToShip");
            _machine.changeState<ShipIdleState>();
        }
    }

    public override void update(float deltaTime, AnimatorStateInfo stateInfo) {

    }
}