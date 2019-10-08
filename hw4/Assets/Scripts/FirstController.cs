using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {
    public FlyActionManager actionManager;
    public DiskFactory factory;
    public UserGUI GUI;
    public ScoreRecorder record;

    private Queue<GameObject> queue = new Queue<GameObject>();
    private List<GameObject> notHit = new List<GameObject>();
    private int level = 1;
    private float speed = 2f;
    private bool isPlay = false, isOver = false, isStart = false;

    void Start () {
        SSDirector director = SSDirector.getInstance();     
        director.currentScenceController = this;             
        factory = Singleton<DiskFactory>.Instance;
        record = Singleton<ScoreRecorder>.Instance;
        actionManager = gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
        GUI = gameObject.AddComponent<UserGUI>() as UserGUI;
    }
	
	void Update () {
        if(isStart) {
            if (isOver) CancelInvoke("LoadResources");
            if (!isPlay) {
                InvokeRepeating("LoadResources", 1f, speed);
                isPlay = true;
            }
            createDisk();
            if (level == 1 && record.score >= 30) {
                level++;
                speed = speed - 0.6f;
                CancelInvoke("LoadResources");
                isPlay = false;
            }
            else if (level == 2 && record.score >= 100) {
                level++;
                speed = speed - 0.5f;
                CancelInvoke("LoadResources");
                isPlay = false;
            }
        }
    }

    public void LoadResources() {
        queue.Enqueue(factory.getDisk(level)); 
    }

    private void createDisk() {
        if (queue.Count != 0) {
            GameObject disk = queue.Dequeue();
            notHit.Add(disk);
            disk.SetActive(true);
            float x = Random.Range(-1f, 1f) < 0 ? -1 : 1, y = Random.Range(1f, 4f);
            disk.GetComponent<DiskData>().direction = new Vector3(x, y, 0);
            Vector3 position = new Vector3(-disk.GetComponent<DiskData>().direction.x * 16, y, 0);
            disk.transform.position = position;
            float power = Random.Range(10f, 15f);
            float angle = Random.Range(15f, 28f);
            actionManager.UFOFly(disk,angle,power);
        }
        for (int i = 0; i < notHit.Count; i++)
            if (notHit[i].transform.position.y < -10 && notHit[i].gameObject.activeSelf == true) {
                factory.freeDisk(notHit[i]);
                notHit.Remove(notHit[i]);
                GUI.bloodReduce();
            }
    }

    public void hit(Vector3 pos) {
        bool isHit = false;
        RaycastHit[] hits;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++) {
            RaycastHit temp = hits[i];
            if (temp.collider.gameObject.GetComponent<DiskData>() != null) {
                for (int j = 0; j < notHit.Count; j++)
                    if (temp.collider.gameObject.GetInstanceID() == notHit[j].gameObject.GetInstanceID())
                        isHit = true;
                if (!isHit) return;
                notHit.Remove(temp.collider.gameObject);
                record.Record(temp.collider.gameObject);
                temp.collider.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                StartCoroutine(WaitingParticle(0.08f, temp, factory, temp.collider.gameObject));
                break;
            }
        }
    }

    public int getScore() {
        return record.score;
    }

    public int getLevel() {
        return level;
    }

    public void restart() {
        record.score = 0;
        level = 1;
        speed = 2f;
        isOver = false;
        isPlay = false;
    }

    public void gameOver() {
        isOver = true;
    }

    public void begin() {
        isStart = true;
    }

    IEnumerator WaitingParticle(float wait_time, RaycastHit hit, DiskFactory disk_factory, GameObject obj) {
        yield return new WaitForSeconds(wait_time); 
        hit.collider.gameObject.transform.position = new Vector3(0, -9, 0);
        disk_factory.freeDisk(obj);
    }
}
public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback {
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null);
}

public interface ISceneController {
    void LoadResources();
}

public interface IUserAction {
    void restart();
    void hit(Vector3 pos);
    void gameOver();
    int getScore();
    int getLevel();
    void begin();
}