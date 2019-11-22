using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bloodBar_GUI : MonoBehaviour {
    private float curBlood = 0f;
    private float targetBlood = 0f;
    public Slider bloodBar;
    GameObject btnPlus, btnMinus;

    private void Start() {
        btnPlus = GameObject.Find("addButton");
        Button a = btnPlus.GetComponent<Button>();
        btnMinus = GameObject.Find("minusButton");
        Button b = btnMinus.GetComponent<Button>();
        a.onClick.AddListener(delegate () {
            this.OnClick(btnPlus);
        });
        b.onClick.AddListener(delegate () {
            this.OnClick(btnMinus);
        });
    }

    private void OnClick(GameObject sender) {
        if (sender.name == "addButton") addBlood();
        if (sender.name == "minusButton") minusBlood();
    }

    public void addBlood() {
        targetBlood = targetBlood - 1f < 0f ? 0f : targetBlood - 1f;
    }

    public void minusBlood() {
        targetBlood = targetBlood + 1f > 10f ? 10f : targetBlood + 1f;
    }

    void Update() {
        curBlood = Mathf.Lerp(curBlood, targetBlood, 0.1f);
        bloodBar.value = curBlood;
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }
}