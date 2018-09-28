using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finger_position_tracking : MonoBehaviour {
    private GameObject index;
    private Vector3 center;

	// Update is called once per frame
	void Update () {
        Vector3 index_position = GameObject.Find("TouchManager").GetComponent<TouchScript.InputSources.TuioInput>().xyzPosition;
        center = GameObject.Find("Empty").transform.position;
        float distance = Vector3.Distance(index_position, center);
        Vector3 delta = index_position - center;
        float radius = GameObject.Find("Globe_Tracked").GetComponent<SpiralSphere>().radius + 0.004f;
        float rate = radius/distance;
        gameObject.transform.position = new Vector3(center.x+rate*delta.x, center.y + rate * delta.y, center.z + rate * delta.z);
        gameObject.transform.LookAt(center,Vector3.up);
    }
}
