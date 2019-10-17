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

public class physicsAction : SSAction {
    private Vector3 stPoint, gPoint = Vector3.zero, cur = Vector3.zero;

    private physicsAction() { }
    public override void Start() { }

    public static physicsAction GetSSAction(GameObject disk, float angle, float power) {
        ConstantForce originalForce = disk.GetComponent<ConstantForce>();
        if (originalForce) {
            originalForce.enabled = true;
            originalForce.force = new Vector3(0, -power, 0);
        }
        else {
            disk.AddComponent<Rigidbody>().useGravity = false;
            disk.AddComponent<ConstantForce>().force = new Vector3(0, -power, 0);
        }
        float x = Random.Range(-15f, 15f), y = Random.Range(0f, 10f);
        Vector3 position = new Vector3(x, y, 0);
        disk.transform.position = position;
        physicsAction act = CreateInstance<physicsAction>();
        if (disk.GetComponent<DiskData>().direction.x == -1)
            act.stPoint = Quaternion.Euler(new Vector3(0, 0, -angle)) * Vector3.left * power;
        else act.stPoint = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
        return act;
    }

    public override void Update() { }
}