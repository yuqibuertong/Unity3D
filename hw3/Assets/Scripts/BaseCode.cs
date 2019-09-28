using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game;

namespace game {
	public class Director : System.Object {
		private static Director _instance;
		public SceneController currentSceneController { get; set; }
		public static Director getInstance() {
			if (_instance == null) _instance = new Director();
			return _instance;
		}
	}

	public interface UserAction {
		void moveBoat();
		void moveCharacter(ICharacterController c);
		void replay();
	}

	public interface SceneController {
		void loadResource();
	}

	public class Moveable: MonoBehaviour {
		float speed = 20;
		public int move;
		Vector3 left, right;

		void Update() {
			if (move == 1) {
				transform.position = Vector3.MoveTowards(transform.position, right, speed * Time.deltaTime);
				if (transform.position == right) move = 2;
			}
            else if (move == 2) {
				transform.position = Vector3.MoveTowards(transform.position, left, speed * Time.deltaTime);
				if (transform.position == left) move = 0;
			}
		}

		public void destination(Vector3 dest) {
			left = dest;
			right = dest;
			if (dest.y == transform.position.y) move = 2;	
			else if (dest.y < transform.position.y) right.y = transform.position.y;
			else right.x = transform.position.x;
			move = 1;
		}
	}

	public class ICharacterController {
		public GameObject role;
		public Moveable moveable;
		public bool onBoat;
        public BankController bank;
		public int type;
		UserGUI GUI;
		public ICharacterController(string c) {
			if (c == "priest") {
				role = Object.Instantiate(Resources.Load ("Perfabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
				type = 0;
			}
            else {
				role = Object.Instantiate(Resources.Load ("Perfabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
				type = 1;
			}
			moveable = role.AddComponent(typeof(Moveable)) as Moveable;
		    GUI = role.AddComponent(typeof(UserGUI)) as UserGUI;
			GUI.role = this;
		}

		public void getOn(BoatController b) {
            bank = null;
			role.transform.parent = b.boat.transform;
			onBoat = true;
		}

		public void getOn(BankController b) {
            bank = b;
			role.transform.parent = null;
			onBoat = false;
		}
	}

    public class BankController {
		public ICharacterController[] roles;
        public Vector3[] pos;
		public int direction;
		GameObject bank;
        Vector3 from_p = new Vector3(10, 1, 0), to_p = new Vector3(-10, 1, 0);

		public BankController(string dir) {
			pos = new Vector3[] {new Vector3(6.5F,2.25F,0), new Vector3(7.5F,2.25F,0), new Vector3(8.5F,2.25F,0), 
				new Vector3(9.5F,2.25F,0), new Vector3(10.5F,2.25F,0), new Vector3(11.5F,2.25F,0)};
			roles = new ICharacterController[6];
			if (dir == "from") {
                bank = Object.Instantiate (Resources.Load ("Perfabs/Rock 10", typeof(GameObject)), from_p, Quaternion.identity, null) as GameObject;
                bank.name = "from";
                direction = 1;
			} else {
                bank = Object.Instantiate (Resources.Load ("Perfabs/Rock 10", typeof(GameObject)), to_p, Quaternion.identity, null) as GameObject;
                bank.name = "to";
                direction = -1;
			}
        }

        public int[] getNum()
        {
            int[] num = { 0, 0 };
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == null) continue;
                if (roles[i].type == 0) num[0]++;
                else num[1]++;
            }
            return num;
        }

        public int emptyNum() {
			for (int i = 0; i < roles.Length; i++)
				if (roles[i] == null) return i;
			return -1;
		}

		public void getOn(ICharacterController characterCtrl) {
			int index = emptyNum();
			roles[index] = characterCtrl;
		}

		public ICharacterController getOff(string name) {
			for (int i = 0; i < roles.Length; i++) {
				if (roles[i] != null && roles[i].role.name == name) {
					ICharacterController actor = roles[i];
					roles[i] = null;
					return actor;
				}
			}
			return null;
		}
	}

	public class BoatController {
		public Vector3[] from_pos;
		public Vector3[] to_pos;
		public GameObject boat;
		public Moveable moveable;
		public int direction;
		public ICharacterController[] roles = new ICharacterController[2];
		Vector3 fPos = new Vector3 (4.5f, 0.95f, 0), tPos = new Vector3 (-4.5f, 0.95f, 0);

		public BoatController() {
			direction = 1;
			from_pos = new Vector3[] {new Vector3 (4, 1.45F, 0), new Vector3 (5F, 1.45F, 0)};
			to_pos = new Vector3[] {new Vector3 (-5F, 1.45F, 0), new Vector3 (-4F, 1.45F, 0)};
			boat = Object.Instantiate (Resources.Load ("Perfabs/boat", typeof(GameObject)), fPos, Quaternion.identity) as GameObject;
			boat.name = "boat";
			moveable = boat.AddComponent (typeof(Moveable)) as Moveable;
			boat.AddComponent (typeof(UserGUI));
		}

		public void Move() {
			if (direction == 1) {
				moveable.destination(tPos);
				direction = -1;
			}
            else {
				moveable.destination(fPos);
				direction = 1;
			}
		}

		public void getOn(ICharacterController role) {
			int e = emptyNum();
			roles[e] = role;
		}

		public int emptyNum() {
			for (int i = 0; i < roles.Length; i++)
				if (roles[i] == null) return i;
			return -1;
		}

		public ICharacterController getOff(string name) {
			for (int i = 0; i < roles.Length; i++)
				if (roles[i] != null && roles[i].role.name == name) {
					ICharacterController c = roles[i];
					roles[i] = null;
					return c;
				}
			return null;
		}
 
		public int[] getNum() {
			int[] num = {0, 0};
			for (int i = 0; i < roles.Length; i++) {
				if (roles[i] == null) continue;
				if (roles[i].type == 0) num[0]++;
				else num[1]++;
			}
			return num;
		}
	}

    public class checkController {
        public int check(int[] fromNum, int[] toNum, int[] boatNum, int direction) {
            int from_priest = 0, from_devil = 0, to_priest = 0, to_devil = 0;
            from_priest += fromNum[0];
            from_devil += fromNum[1];
            to_priest += toNum[0];
            to_devil += toNum[1];
            if (to_priest + to_devil == 6) return 2;
            if (direction == -1) {
                to_priest += boatNum[0];
                to_devil += boatNum[1];
            }
            else {
                from_priest += boatNum[0];
                from_devil += boatNum[1];
            }
            if ((from_priest < from_devil && from_priest > 0) || (to_priest < to_devil && to_priest > 0)) return 1;
            return 0;
        }
    }
}