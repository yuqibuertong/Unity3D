using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour, ISSActionCallback {
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> add = new List<SSAction>();
    private List<int> delete = new List<int>();
    
    protected void Update() {
        foreach (SSAction ac in add) actions[ac.GetInstanceID()] = ac;
        add.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions) {
            SSAction t = kv.Value;
            if (t.done) delete.Add(t.GetInstanceID());
            else if (t.able) t.Update();
        }

        foreach (int key in delete) {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        delete.Clear();
    }

    public void RunAction(GameObject obj, SSAction act, ISSActionCallback call) {
        act.gameobj = obj;
        act.trans = obj.transform;
        act.callback = call;
        add.Add(act);
        act.Start();
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
     int intParam = 0, string strParam = null, Object objectParam = null) { }
}
