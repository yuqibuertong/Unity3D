using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceAction : SSAction, ISSActionCallback{
    public List<SSAction> act;
    public int repeat = -1;
    public int start = 0;

    public static SequenceAction GetSSAcition(int _repeat, int _start, List<SSAction> _act) {
        SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
        action.repeat = _repeat;
        action.act = _act;
        action.start = _start;
        return action;
    }

    void OnDestroy () { }

    public override void Update() {
        if (act.Count == 0) return;
        if (start < act.Count)
            act[start].Update();
    }

    public override void Start() {
        foreach (SSAction action in act) {
            action.gameobj = this.gameobj;
            action.trans = this.trans;
            action.callback = this;
            action.Start();
        }
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null) {
        source.done = false;
        this.start++;
        if (this.start >= act.Count) {
            this.start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0) {
                this.done = true;
                this.callback.SSActionEvent(this);
            }
        }
    }
}
