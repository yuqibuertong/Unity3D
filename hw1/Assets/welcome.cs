using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class welcome : MonoBehaviour {
    public Texture2D img;
    void Start() {
        Debug.Log("start!!!");
    }

    private void OnGUI() {
        float height = Screen.height * 0.5f;
        float width = Screen.width * 0.5f;
        GUIStyle myStyle = new GUIStyle();
        myStyle.normal.background = img;
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "", myStyle);
        GUIStyle playStyle = new GUIStyle();
        playStyle.fontSize = 40;
        playStyle.normal.textColor = new Color(255, 255, 255);
        GUIStyle gameStyle = new GUIStyle();
        gameStyle.fontSize = 70;
        gameStyle.normal.textColor = new Color(255, 255, 255);

        GUI.Button(new Rect(width - 170, height - 200, 200, 50), "Tic Tac Toe", gameStyle);
        if (GUI.Button(new Rect(width - 30, height + 100, 100, 50), "Play", playStyle))
            play();
    }

    void play() {
        SceneManager.LoadScene("tic");
    }
}
