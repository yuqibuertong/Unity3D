using UnityEngine;

public class Gate : MonoBehaviour {
    public delegate void AddScore();
    public static event AddScore addScore;

    private void Start() { }

    void change2State(int n) {
        transform.parent.GetComponent<GetGuard>().guard.GetComponent<GuardController>().state = n;
        transform.parent.GetComponent<GetGuard>().guard.GetComponent<Animator>().SetInteger("state1", n);
    }

    void add() {
        if (addScore != null) addScore();
    }

    void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.tag == "Guard")
            transform.parent.GetComponent<GetGuard>().guard.GetComponent<GuardController>().state = 0;
        if (collider.gameObject.tag == "Player")
            if (transform.parent.GetComponent<GetGuard>().guard.GetComponent<GuardController>().state == 0)
                change2State(1);
            else {
                change2State(0);
                add();
            }
    }
}