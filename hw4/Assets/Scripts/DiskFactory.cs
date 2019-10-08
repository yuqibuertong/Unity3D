using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {
    public GameObject disk = null;
    private List<DiskData> close = new List<DiskData>();
    private List<DiskData> open = new List<DiskData>();

    public GameObject getDisk(int level) {
        int num = 0, s1 = 1, s2 = 4, s3 = 7;
        float Y = -20f;
        string type;
        disk = null;

        switch (level) {
            case 1: num = Random.Range(0, s1); break;
            case 2: num = Random.Range(0, s2); break;
            case 3: num = Random.Range(0, s3); break;
        }

        if (num <= s1) type = "disk1";
        else if (num <= s2 && num > s1) type = "disk2";
        else type = "disk3";

        for (int i = 0; i < open.Count; i++)
            if (open[i].tag == type) {
                disk = open[i].gameObject;
                open.Remove(open[i]);
                break;
            }
        if (disk == null) {
            if (type == "disk1") {
                disk = Instantiate(Resources.Load<GameObject>("Prefabs/disk1"), new Vector3(0, Y, 0), Quaternion.identity);
                disk.GetComponent<DiskData>().score = 10;
            }
            else if (type == "disk2") {
                disk = Instantiate(Resources.Load<GameObject>("Prefabs/disk2"), new Vector3(0, Y, 0), Quaternion.identity);
                disk.GetComponent<DiskData>().score = 20;
            }
            else if (type == "disk3") {
                disk = Instantiate(Resources.Load<GameObject>("Prefabs/disk3"), new Vector3(0, Y, 0), Quaternion.identity);
                disk.GetComponent<DiskData>().score = 30;
            }
            float X = Random.Range(-1f, -1f) < 0 ? -1 : 1;
            disk.GetComponent<DiskData>().direction = new Vector3(X, Y, 0);
            disk.transform.localScale = disk.GetComponent<DiskData>().scale;
        }
        close.Add(disk.GetComponent<DiskData>());
        return disk;
    }

    public void freeDisk(GameObject disk) {
        for (int i = 0; i < close.Count; i++)
            if (disk.GetInstanceID() == close[i].gameObject.GetInstanceID()) {
                close[i].gameObject.SetActive(false);
                open.Add(close[i]);
                close.Remove(close[i]);
                break;
            }
    }
}