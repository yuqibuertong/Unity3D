using UnityEngine;

public class GetGuard : MonoBehaviour {
    public GameObject guard;

    private void Start() { }

    void OnCollisionEnter(Collision collider)  {
        if (collider.gameObject.tag == "Guard") 
            guard = collider.gameObject;
    }
}