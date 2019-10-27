using UnityEngine;

public class GuardController : MonoBehaviour {
    private float pos_x, pos_y = 0.5f, pos_z;
    public float speed = 0.5f;
    private float dis = 0;
    private bool getTurn = true;
    public int state = 0, direction = 0;
    public GameObject role;
    public SceneController sceneController;

	void Start () {
        sceneController = SSDirector.getInstance().currentScenceController as SceneController;
        role = sceneController.role;
        pos_x = transform.position.x;
        pos_z = transform.position.z;
	}

	void FixedUpdate () {
        if (state == 0) patrol();
        else if (state == 1) chase(role);
    }

    void patrol() {
        if (getTurn) {
            switch (direction) {
                case 0:
                    pos_x += 2f; break;
                case 1:
                    pos_z += 2f; break;
                case 2:
                    pos_x -= 2f; break;
                case 3:
                    pos_z -= 2f; break;
            }
            getTurn = false;
        }
        transform.LookAt(new Vector3(pos_x, 0, pos_z));
        transform.Rotate(new Vector3(0, 180, 0));
        dis = Vector3.Distance(transform.position, new Vector3(pos_x, pos_y, pos_z));
        if (dis > 0.6)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(pos_x, pos_y, pos_z), speed * Time.deltaTime);
        else {
            direction = (direction + 1) % 4;
            getTurn = true;
        }
    }

    void chase(GameObject role) {
        if (GetComponent<Animator>().GetInteger("state1") != 2) {
            speed = 1f + (float)(sceneController.Recorder.Score / 4);
            transform.position = Vector3.MoveTowards(transform.position, role.transform.position, 0.5f * speed * Time.deltaTime);
            transform.LookAt(role.transform.position);
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }
}