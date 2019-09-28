using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game;

public class SSAction : ScriptableObject {
    public bool enable = true;
    public bool destroy = false;
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

public class SSMoveToAction : SSAction {
    public Vector3 goal;
    public float speed; 

    private SSMoveToAction() { }
    public static SSMoveToAction GetSSAction(Vector3 goal, float speed) {
        SSMoveToAction action = ScriptableObject.CreateInstance<SSMoveToAction>();
        action.goal = goal;
        action.speed = speed;
        return action;
    }

    public override void Update() {
        this.trans.position = Vector3.MoveTowards(this.trans.position, goal, speed * Time.deltaTime);
        if (this.trans.position == goal) {
            this.destroy = true;
            this.callback.SSActionEvent(this);
        }
    }

    public override void Start() { }
}

public class SequenceAction : SSAction, ISSActionCallback {
    public List<SSAction> actList;
    public int repeat = -1;
    public int start = 0;

    public static SequenceAction GetSSAcition(int rep, int st, List<SSAction> act) {
        SequenceAction action = ScriptableObject.CreateInstance<SequenceAction>();
        action.repeat = rep;
        action.actList = act;
        action.start = st;
        return action;
    }

    public override void Update() {
        if (actList.Count == 0) return;
        if (start < actList.Count) actList[start].Update();
    }

    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null) {
        source.destroy = false;
        this.start++;
        if (this.start >= actList.Count) {
            this.start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0) {
                this.destroy = true;
                this.callback.SSActionEvent(this); 
            }
        }
    }

    public override void Start() {
        foreach (SSAction action in actList) {
            action.gameobj = this.gameobj;
            action.trans = this.trans;
            action.callback = this;
            action.Start();
        }
    }

    void OnDestroy() { }
}

public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback {
    void SSActionEvent(SSAction act, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null);
}

public class SSActionManager : MonoBehaviour, ISSActionCallback {
    private Dictionary<int, SSAction> act = new Dictionary<int, SSAction>();
    private List<SSAction> add = new List<SSAction>();
    private List<int> delete = new List<int>();

    protected void Update() {
        foreach (SSAction ac in add) act[ac.GetInstanceID()] = ac;
        add.Clear();

        foreach (KeyValuePair<int, SSAction> kv in act) {
            SSAction ac = kv.Value;
            if (ac.destroy) delete.Add(ac.GetInstanceID());
            else if (ac.enable) ac.Update();
        }

        foreach (int key in delete) {
            SSAction ac = act[key];
            act.Remove(key);
            DestroyObject(ac);
        }
        delete.Clear();
    }

    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager) {
        action.gameobj = gameobject;
        action.trans = gameobject.transform;
        action.callback = manager;
        add.Add(action);
        action.Start();
    }

    public void SSActionEvent(SSAction act, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null) { }
}

public class MySceneActionManager : SSActionManager {
    private SSMoveToAction moveBoatTo;
    private SequenceAction moveRoleTo;
    public FirstController sceneController;

    protected void Start() {
        sceneController = (FirstController)Director.getInstance().currentSceneController;
        sceneController.actionManager = this;
    }

    public void moveBoat(GameObject b, Vector3 goal, float speed) {
        moveBoatTo = SSMoveToAction.GetSSAction(goal, speed);
        this.RunAction(b, moveBoatTo, this);
    }

    public void moveCharacter(GameObject role, Vector3 left, Vector3 right, float speed) {
        SSAction action1 = SSMoveToAction.GetSSAction(left, speed);
        SSAction action2 = SSMoveToAction.GetSSAction(right, speed);
        moveRoleTo = SequenceAction.GetSSAcition(1, 0, new List<SSAction> { action1, action2 });
        this.RunAction(role, moveRoleTo, this);
    }
}