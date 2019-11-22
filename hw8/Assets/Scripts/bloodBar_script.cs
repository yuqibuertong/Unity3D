using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodBar_script : MonoBehaviour {
    public float curBlood = 10f;
    private float targetBlood = 10f;
    private Rect bloodBarArea;
    private Rect addButton;
    private Rect minusButton;
    private Rect num1;
    private Rect num2;
    private Rect num3;
    private bool status;

	void Start () {
        bloodBarArea = new Rect(Screen.width - 220, 20, 200, 50);
        addButton = new Rect(Screen.width - 220, 50, 40, 20);
        minusButton = new Rect(Screen.width - 60, 50, 40, 20);
        num1 = new Rect(Screen.width - 220, 80, 40, 20);
        num2 = new Rect(Screen.width - 140, 80, 40, 20);
        num3 = new Rect(Screen.width - 60, 80, 40, 20);
	}

    public void addBlood(float num) {
        targetBlood = targetBlood + num > 10f? 10f : targetBlood + num;
    }

    public void minusBlood(float num) {
        targetBlood = targetBlood - num < 0f? 0f : targetBlood - num;
    }

    private void OnGUI() {
        if (GUI.Button(addButton, " + ")) status = true;
        if (GUI.Button(minusButton, " - ")) status = false;
        if (status) {
            if (GUI.Button(num1, " 1 ")) addBlood(1);
            if (GUI.Button(num2, " 2 ")) addBlood(2);
            if (GUI.Button(num3, " 3 ")) addBlood(3);
        }
        else {
            if (GUI.Button(num1, " 1 ")) minusBlood(1);
            if (GUI.Button(num2, " 2 ")) minusBlood(2);
            if (GUI.Button(num3, " 3 ")) minusBlood(3);
        }
        curBlood = Mathf.Lerp(curBlood, targetBlood, 0.1f);
        GUI.HorizontalScrollbar(bloodBarArea, 0f, curBlood, 0f, 10f);
    }
}