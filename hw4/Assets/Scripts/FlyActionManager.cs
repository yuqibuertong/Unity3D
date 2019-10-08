using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyActionManager : SSActionManager {
    public FirstController controller;
    public UFOFlyAction act;

	void Start () {
        controller = (FirstController)SSDirector.getInstance().currentScenceController;
        controller.actionManager = this;
	}

    public void UFOFly(GameObject disk, float angle, float power) {
        act = UFOFlyAction.GetSSAction(disk.GetComponent<DiskData>().direction, angle, power);
        this.RunAction(disk, act, this);
    }
}