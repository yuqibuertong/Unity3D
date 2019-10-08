using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {
    private IUserAction act;
    public int blood = 10;
    GUIStyle bStyle = new GUIStyle();
    GUIStyle scoreStyle = new GUIStyle();
    GUIStyle textStyle = new GUIStyle();
    GUIStyle style = new GUIStyle();
    private bool isStart = false;

    void Start () {
        act = SSDirector.getInstance().currentScenceController as IUserAction;
    }

    public void bloodReduce() {
        if (blood > 0) blood--;
    }
	
	void OnGUI () {
        bStyle.fontSize = 40;
        bStyle.normal.textColor = new Color(1, 0, 0);
        textStyle.fontSize = 40;
        textStyle.normal.textColor = new Color(1, 1, 1);
        scoreStyle.fontSize = 40;
        scoreStyle.normal.textColor = new Color(0, 0, 0);
        style.fontSize = 60;
        style.normal.textColor = new Color(1, 1, 1);

        if (isStart) {
            if (Input.GetButtonDown("Fire1")) act.hit(Input.mousePosition);
            GUI.Label(new Rect(10, 5, 200, 50), "SCORE", textStyle);
            GUI.Label(new Rect(10, 50, 200, 50), "LEVEL", textStyle);
            GUI.Label(new Rect(Screen.width - 380, 5, 50, 50), "BLOOD", textStyle);
            GUI.Label(new Rect(200, 5, 200, 50), act.getScore().ToString(), scoreStyle);
            GUI.Label(new Rect(200, 50, 200, 50), act.getLevel().ToString(), scoreStyle);
            for (int i = 0; i < blood; i++)
                GUI.Label(new Rect(Screen.width - 220 + 20 * i, 5, 50, 50), "#", bStyle);
            if (blood == 0) {
                GUI.Label(new Rect(Screen.width / 2 - 130, Screen.height / 2 - 120, 100, 100), "Game Over", style);
                if (GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 30, 100, 50), "REPLAY")) {
                    blood = 10;
                    act.restart();
                    return;
                }
                act.gameOver();
            }
        }
        else {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 120, 100, 100), "Hit UFO", style);
            if (GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 30, 100, 50), "START")) {
                isStart = true;
                act.begin();
            }
        }
    }
}
