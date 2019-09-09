using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {
    public Texture2D img;
    private int turn = 1;
    private int num = 3;//
    private int[,] state = new int[3, 3];
    void Start() {
        Debug.Log("start!!!");
        replay();
    }

    private void OnGUI()
    {
        //buttons and background
        int bHeight = 100;
        int bWidth = 100;
        float height = Screen.height * 0.5f - 100 * num / 2;
        float width = Screen.width * 0.5f - 100 * num / 2;
        GUIStyle myStyle = new GUIStyle();
        myStyle.normal.background = img;
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "", myStyle);
        GUIStyle resultStyle = new GUIStyle();
        resultStyle.fontSize = 40;
        resultStyle.normal.textColor = new Color(255, 255, 255);

        //game logic
        int result = check();
        if (result == 1) GUI.Label(new Rect(width - 30, height - 70, 100, 50), "Game over, O wins!", resultStyle);
        else if (result == 2) GUI.Label(new Rect(width - 30, height -70, 100, 50), "Game over, X wins!", resultStyle);
        if (GUI.Button(new Rect(width + 90, height + num * bHeight + 50, 3 * bWidth, 50), "Replay", resultStyle))
            replay();
        for (int i = 0; i < num; i++)
            for (int j = 0; j < num; j++) {
                if (state[i, j] == 1)
                    GUI.Button(new Rect(width + i * bWidth, height + j * bHeight, bWidth, bHeight), "O");
                if (state[i, j] == 2)
                    GUI.Button(new Rect(width + i * bWidth, height + j * bHeight, bWidth, bHeight), "X");
                if (GUI.Button(new Rect(width + i * bWidth, height + j * bHeight, bWidth, bHeight), ""))
                    if (result == 0) {
                        if (turn == 1) state[i, j] = 1;
                        else state[i, j] = 2;
                        turn = 3 - turn;
                    }
            }
    }

    private int check() {
        int i, j;
        for (i = 0; i < num; i++)
            if (state[i, 0] != 0 && state[i, 0] == state[i, 1] && state[i, 1] == state[i, 2])
                return state[i, 0];
        for (i = 0; i < num; i++)
            if (state[0, i] != 0 && state[0, i] == state[1, i] && state[1, i] == state[2, i])
                return state[0, i];
        if (state[1, 1] != 0 &&
            (state[0, 0] == state[1, 1] && state[1, 1] == state[2, 2]) ||
            (state[0, 2] == state[1, 1] && state[1, 1] == state[2, 0]))
            return state[1, 1];
        return 0;
    }

    void replay() {
        for (int i = 0; i < num; i++)
            for (int j = 0; j < num; j++)
                state[i, j] = 0;
    }
}
