using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game;

public enum BoatAct { P, D, PP, DD, PD };

public struct Next {
    public BoatAct boatAct;
}
public class FirstController : MonoBehaviour, SceneController, UserAction {
	public BankController fromBank;
	public BankController toBank;
	public BoatController boat;
    public checkController gameover;
	private ICharacterController[] roles;
    private Next next;
    private int status;
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

    public void nextMove() {
        int direction = boat.direction;
        ICharacterController p, d;
        if (status == 0) {
            GetNextPassager();
            if (direction == 1 && next.boatAct == BoatAct.PP) {
                p = fromBank.findCharacter(0);
                moveCharacter(p);
                p = fromBank.findCharacter(0);
                moveCharacter(p);
            }
            else if (direction == 1 && next.boatAct == BoatAct.P) {
                p = fromBank.findCharacter(0);
                moveCharacter(p);
            }
            else if (direction == 1 && next.boatAct == BoatAct.PD) {
                p = fromBank.findCharacter(0);
                moveCharacter(p);
                d = fromBank.findCharacter(1);
                moveCharacter(d);
            }
            else if (direction == 1 && next.boatAct == BoatAct.D) {
                d = fromBank.findCharacter(1);
                moveCharacter(d);
            }
            else if (direction == 1 && next.boatAct == BoatAct.DD) {
                d = fromBank.findCharacter(1);
                moveCharacter(d);
                d = fromBank.findCharacter(1);
                moveCharacter(d);
            }
            else if (direction == -1 && next.boatAct == BoatAct.PP) {
                p = toBank.findCharacter(0);
                moveCharacter(p);
                p = toBank.findCharacter(0);
                moveCharacter(p);
            }
            else if (direction == -1 && next.boatAct == BoatAct.P) {
                p = toBank.findCharacter(0);
                moveCharacter(p);
            }
            else if (direction == -1 && next.boatAct == BoatAct.PD) {
                p = toBank.findCharacter(0);
                moveCharacter(p);
                d = toBank.findCharacter(1);
                moveCharacter(d);
            }
            else if (direction == -1 && next.boatAct == BoatAct.D) {
                d = toBank.findCharacter(1);
                moveCharacter(d);
            }
            else if (direction == -1 && next.boatAct == BoatAct.DD) {
                d = toBank.findCharacter(1);
                moveCharacter(d);
                d = toBank.findCharacter(1);
                moveCharacter(d);
            }
            if (direction == 1) direction = -1;
            else direction = 1;
            status = 1;
        }
        else if (status == 1) {
            moveBoat();
            status = 2;
        }
        else if (status == 2) {
            ICharacterController[] pass = boat.getRoles();
            for (int i = 0; i < pass.Length; i++)
                if (pass[i] != null) moveCharacter(pass[i]);
            status = 0;
        }
    }

    private int randomValue() {
        float a = Random.Range(0f, 1f);
        if (a <= 0.5f) return 1;
        else return 2;
    }

    public void GetNextPassager() {
        int from_P, from_D, direction = boat.direction;
        int[] fromNum = fromBank.getNum();
        from_P = fromNum[0];
        from_D = fromNum[1];
        if (from_P == 3 && from_D == 3 && direction == 1) {
            int turn = randomValue();
            if (turn == 1) next.boatAct = BoatAct.PD;
            else next.boatAct = BoatAct.DD;
        }
        else if (direction == -1 && from_P == 2 && from_D == 2) next.boatAct = BoatAct.P;
        else if (direction == -1 && from_P == 3 && from_D == 2) next.boatAct = BoatAct.D;
        else if (direction == -1 && from_P == 3 && from_D == 1) next.boatAct = BoatAct.D;
        else if (direction == 1 && from_P == 3 && from_D == 2) next.boatAct = BoatAct.DD;
        else if (direction == -1 && from_P == 3 && from_D == 0) next.boatAct = BoatAct.D;
        else if (direction == 1 && from_P == 3 && from_D == 1) next.boatAct = BoatAct.PP;
        else if (direction == -1 && from_P == 1 && from_D == 1) next.boatAct = BoatAct.PD;
        else if (direction == 1 && from_P == 2 && from_D == 2) next.boatAct = BoatAct.PP;
        else if (direction == -1 && from_P == 0 && from_D == 0) next.boatAct = BoatAct.D;
        else if (direction == 1 && from_P == 0 && from_D == 3) next.boatAct = BoatAct.DD;
        else if (direction == -1 && from_P == 0 && from_D == 1) {
            if (randomValue() == 1) next.boatAct = BoatAct.D;
            else next.boatAct = BoatAct.P;
        }
        else if (direction == -1 && from_P == 0 && from_D == 2) next.boatAct = BoatAct.D;
        else if (direction == 1 && from_P == 2 && from_D == 1) next.boatAct = BoatAct.P;
        else if (direction == 1 && from_P == 0 && from_D == 2) next.boatAct = BoatAct.DD;
        else if (direction == 1 && from_P == 1 && from_D == 1) next.boatAct = BoatAct.PD;
    }
}