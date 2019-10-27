using UnityEngine;

public class RotateCamera : MonoBehaviour {
    void Start() {
        transform.parent = (SSDirector.getInstance().currentScenceController as SceneController).role.transform;
    }
}