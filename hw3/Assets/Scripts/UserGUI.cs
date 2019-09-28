using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game;

public class UserGUI : MonoBehaviour {
	public UserAction action;
	public int result = 0;
    public ICharacterController role;
	GUIStyle style= new GUIStyle();
	GUIStyle bStyle = new GUIStyle("button");

    void Start() {
		action = Director.getInstance().currentSceneController as UserAction;
		style.fontSize = 50;
        style.normal.textColor = new Color(255, 255, 255);
        style.alignment = TextAnchor.MiddleCenter;
		bStyle.fontSize = 30;
	}

    void OnMouseDown() {
        if (gameObject.name == "boat") action.moveBoat();
        else action.moveCharacter(role);
    }

	void OnGUI() {
		if (result != 0 && GUI.Button(new Rect(Screen.width/2-70, Screen.height/2 - 90, 120, 60), "Replay", bStyle)) {
			result = 0;
			action.replay();
		}
		if (result == 1)
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-180, 100, 50), "You lose!", style);
		else if(result == 2)
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-180, 100, 50), "You win!", style);
	}
}