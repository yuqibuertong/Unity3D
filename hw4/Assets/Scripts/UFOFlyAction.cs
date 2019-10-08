using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOFlyAction : SSAction {
    public float gravity = -5, time;
    private Vector3 stPoint, gPoint = Vector3.zero, cur = Vector3.zero;

    private UFOFlyAction() { }
    public override void Start() { }

    public static UFOFlyAction GetSSAction(Vector3 dir, float angle, float power) {
        UFOFlyAction act = CreateInstance<UFOFlyAction>();
        if (dir.x == -1) act.stPoint = Quaternion.Euler(new Vector3(0, 0, -angle)) * Vector3.left * power;
        else act.stPoint = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
        return act;
    }

    public override void Update() {
        time += Time.fixedDeltaTime;
        gPoint.y = gravity * time;
        trans.position += (stPoint + gPoint) * Time.fixedDeltaTime;
        cur.z = Mathf.Atan((stPoint.y + gPoint.y) / stPoint.x);
        trans.eulerAngles = cur;
        if (this.trans.position.y < -10) {
            this.done = true;
            this.callback.SSActionEvent(this);
        }
    }
}
