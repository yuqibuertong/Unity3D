﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game;

public class FirstController : MonoBehaviour, SceneController, UserAction {
	public BankController fromBank;
	public BankController toBank;
	public BoatController boat;
    public checkController gameover;
	private ICharacterController[] roles;
	Vector3 water_p = new Vector3(0, 0, 0);
	UserGUI GUI;
    public MySceneActionManager actionManager;

    void Start() {
		Director director = Director.getInstance();
		GUI = gameObject.AddComponent<UserGUI>() as UserGUI;
		roles = new ICharacterController[6];
		director.currentSceneController = this;
		loadResource();
        actionManager = gameObject.AddComponent<MySceneActionManager>() as MySceneActionManager;
    }

	public void loadResource() {
        int i;
        ICharacterController role;
        Vector3 t;
        GameObject water = Instantiate (Resources.Load ("Perfabs/Water", typeof(GameObject)), water_p, Quaternion.identity) as GameObject;
		water.name = "water";
        fromBank = new BankController("from");
        toBank = new BankController("to");
		boat = new BoatController();
        gameover = new checkController();
		for (i = 0; i < 3; i++) {
			role = new ICharacterController ("priest");
            role.role.name = "priest" + i;
            t = fromBank.pos[fromBank.emptyNum()];
            t.x *= fromBank.direction;
            role.role.transform.position = t;
            role.getOn(fromBank);
            fromBank.getOn(role);
			roles[i] = role;
		}
		for (i = 0; i < 3; i++) {
			role = new ICharacterController ("devil");
            role.role.name = "devil" + i;
            t = fromBank.pos[fromBank.emptyNum()];
            t.x *= fromBank.direction;
            role.role.transform.position = t;
            role.getOn(fromBank);
            fromBank.getOn(role);
			roles[i+3] = role;
		}
	}

	public void moveBoat() {
        int i;
        for (i = 0; i < boat.roles.Length; i++)
            if (boat.roles[i] != null) break;
        if (i == boat.roles.Length) return;
        boat.Move();
        int[] fromNum = fromBank.getNum();
        int[] toNum = toBank.getNum();
        int[] boatNum = boat.getNum();
        int direction = boat.direction;
		GUI.result = gameover.check(fromNum, toNum, boatNum, direction);
	}

	public void moveCharacter(ICharacterController role) {
        BankController bank;
        Vector3 t, pos;
        if (role.onBoat) {
			if (boat.direction == -1) bank = toBank;
			else bank = fromBank;
			boat.getOff(role.role.name);
            t = bank.pos[bank.emptyNum()];
            t.x *= bank.direction;
            role.moveable.destination(t);
            role.getOn(bank);
            bank.getOn(role);
		}
        else {
            bank = role.bank;
			if (boat.emptyNum() == -1) return;
			if (bank.direction != boat.direction) return;
            bank.getOff(role.role.name);
            int e = boat.emptyNum();
            if (boat.direction == -1) pos = boat.to_pos[e];
            else pos = boat.from_pos[e];
            role. moveable.destination(pos);
            role.getOn(boat);
			boat.getOn(role);
		}
        int[] fromNum = fromBank.getNum();
        int[] toNum = toBank.getNum();
        int[] boatNum = boat.getNum();
        int direction = boat.direction;
		GUI.result = gameover.check(fromNum, toNum, boatNum, direction);
	}

	public void replay() {
        Vector3 t;
        boat.moveable.move = 0;
        if (boat.direction == -1) boat.Move();
        boat.roles = new ICharacterController[2];
        fromBank.roles = new ICharacterController[6];
        toBank.roles = new ICharacterController[6];
		for (int i = 0; i < roles.Length; i++) {
            roles[i].moveable.move = 0;
            roles[i].bank = (Director.getInstance().currentSceneController as FirstController).fromBank;
            roles[i].getOn(roles[i].bank);
            t = roles[i].bank.pos[roles[i].bank.emptyNum()];
            t.x *= roles[i].bank.direction;
            roles[i].role.transform.position = t;
            roles[i].bank.getOn(roles[i]);
        }
	}
}