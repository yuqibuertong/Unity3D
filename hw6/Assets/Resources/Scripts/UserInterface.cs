using UnityEngine;

public interface IUserAction {
    void instructions();
    void scoreDisplay();
}

public class UserInterface : MonoBehaviour {
    private IUserAction action;
    public GameObject role;
    public float speed = 1;
    public SceneController sceneController;
    GUIStyle style = new GUIStyle(), resultStyle = new GUIStyle();

    void Start () {
        action = SSDirector.getInstance().currentScenceController as IUserAction;
        sceneController = SSDirector.getInstance().currentScenceController as SceneController;
        role = sceneController.role;
        style.fontSize = 30;
        style.normal.textColor = new Color(255, 255, 255);
        resultStyle.fontSize = 80;
        resultStyle.normal.textColor = new Color(255, 255, 255);
    }

    void Update () {
        if (sceneController.gamestate == 1) {
            if (Input.GetKeyDown(KeyCode.Space) && role.transform.position.y < 0.55)
                role.GetComponent<Rigidbody>().AddForce(new Vector3(0, 4, 0), ForceMode.Impulse);
            float pos_x = Input.GetAxis("Horizontal") * speed, pos_z = Input.GetAxis("Vertical") * speed;
            pos_x *= Time.deltaTime;
            pos_z *= Time.deltaTime;
            if (pos_x != 0 || pos_z != 0) role.GetComponent<Animator>().SetInteger("state", 1);
            else role.GetComponent<Animator>().SetInteger("state", 0);
            role.transform.Translate(pos_x, 0, pos_z);
        }
    }

    void OnGUI() {
        if (GUI.RepeatButton(new Rect(0, 0, 210, 40), "INSTRUCTION", style)) action.instructions();
        GUI.Button(new Rect(0, 40, 120, 40), "SCORE", style);
        action.scoreDisplay();
        if (sceneController.gamestate == 0)
            GUI.Label(new Rect(Screen.width/2 - 250, Screen.height/2 - 80, 200, 40), "GAME OVER", resultStyle);
    }
}