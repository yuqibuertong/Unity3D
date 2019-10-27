using UnityEngine;

public class CameraMove : MonoBehaviour {
    private Vector3 offset;
    public GameObject role;

    void Start () {
        role = (SSDirector.getInstance().currentScenceController as SceneController).role;
        offset = role.transform.position - this.transform.position;
	}

	void Update () {
        transform.position = role.transform.position - offset;
	}
}