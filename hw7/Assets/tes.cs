using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tes : MonoBehaviour {
    ParticleSystem ps;
    int direction;
    void Start() {
        ps= GetComponent<ParticleSystem>();
        direction = 0;
    }

    void Update() {
        int minX = -10, maxX = 10 ;
        if (direction == 0 && ps.transform.position.x > minX)
            ps.transform.position += Vector3.left * Time.deltaTime * 30;
        else if (direction == 1 && ps.transform.position.x < maxX)
            ps.transform.position += Vector3.right * Time.deltaTime * 30;
        else direction = 1 - direction;
    }
}