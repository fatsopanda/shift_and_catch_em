using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31.StateKit;

public class CannonShootState : SKMecanimState<PlayerController> {

    public override void begin() {
        _machine.animator.SetBool("shoot", true);
        _context.StartCoroutine("ShootBullet");
    }

    public override void reason() {
        if (Input.GetKeyUp("space"))
            _machine.changeState<CannonIdleState>();
    }

    public override void update(float deltaTime, AnimatorStateInfo stateInfo) {

    }

    public override void end() {
        _machine.animator.SetBool("shoot", false);
    }
}