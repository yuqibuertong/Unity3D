using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject {
    public bool able = true;
    public bool done = false;
    public GameObject gameobj;
    public Transform trans;
    public ISSActionCallback callback;

    protected SSAction() { }

    public virtual void Start() {
        throw new System.NotImplementedException();
    }

    public virtual void Update() {
        throw new System.NotImplementedException();
    }
}
