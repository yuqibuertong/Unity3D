using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using myspace;

public class SceneController : MonoBehaviour, ISceneController, IUserAction {
    public GameObject role;
    public GameObject blood;
    public Text GameText;
    public int gamestate = 0;
    public ScoreRecorder Recorder;

    void Awake() {
        SSDirector director = SSDirector.getInstance();
        director.currentScenceController = this;
        director.currentScenceController.LoadResources();
    }

    void Start () {
        gamestate = 2;
        RoleTrigger.gameOver += GameOver;
        StartCoroutine(waitForOneSecond());
    }

	void Update () {
        if (gamestate == 0) blood.transform.position = role.transform.position + new Vector3(0, 0.2f, 0);
        if (role.transform.position.y < -10) GameOver();
	}

    public void LoadResources() {
        Instantiate(Resources.Load("Prefabs/Light"));
        Instantiate(Resources.Load("Prefabs/Entry"));
        blood = Instantiate(Resources.Load("Prefabs/Blood"), new Vector3(40, 40, 40), Quaternion.identity) as GameObject;
        role = Instantiate(Resources.Load("Prefabs/role")) as GameObject;
        float pos_z = 0, pos_x;
        int dir, sum = 0, size = 5;
        GameObject maze, temp;
        guardFactory gf = guardFactory.getInstance();
        for (int i = 0; i < size; i++) {
            dir = Random.Range(0, 2);
            pos_x = i * 2.8f;
            for (int j = 0; j <= i; j++, sum++) {
                if (sum%3 == 0)
                    maze = Instantiate(Resources.Load("Prefabs/g_maze"), new Vector3(pos_x, 0, pos_z), Quaternion.identity) as GameObject;
                else if (sum%3 == 1)
                    maze = Instantiate(Resources.Load("Prefabs/y_maze"), new Vector3(pos_x, 0, pos_z), Quaternion.identity) as GameObject;
                else maze = Instantiate(Resources.Load("Prefabs/b_maze"), new Vector3(pos_x, 0, pos_z), Quaternion.identity) as GameObject;
                temp = gf.getNewGuard(pos_x - 0.9f, 0, pos_z - 0.9f);
                temp.transform.parent = maze.transform;
                if (j != i)
                    if (dir == 0) pos_z += 2.8f;
                    else pos_z -= 2.8f;
            }
        }
    }

    public IEnumerator waitForOneSecond() {
        int readyTime = 3;
        while (readyTime > 0 && gamestate == 2) {
            GameText.text = readyTime.ToString();
            yield return new WaitForSeconds(1);
            readyTime--;
        }
        GameText.text = "";
        gamestate = 1;
    }

    void GameOver() {
        blood.GetComponent<ParticleSystem>().Play();
        gamestate = 0;
    }

    public void instructions() {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(255, 255, 255);
        style.fontSize = 20;
        GUI.Label(new Rect(220, 0, 400, 50), "Try to escape away from Guard. Each success escape for one point.\nBeing caught by Guard causes GAME OVER", style);
    }

    public void scoreDisplay() {
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = new Color(255, 255, 255);
        GUI.Button(new Rect(120, 40, 120, 40), Recorder.Score.ToString(), style);
    }
}
