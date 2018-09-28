using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Select_Detector : MonoBehaviour {
    private bool count;
    private bool isComplete;
    private bool isReported;
    public float Hold_Time;
    private float Current_Time;
    private float Current_Fix_Time;
    private bool Current_isOnSphere;
    private GameObject finger_manager;
    public Material Mat;
    private string path;

    // Use this for initialization
    void Start () {
        count = false;
        isComplete = false;
        isReported = false;
        Current_Time = 0f;
        Current_Fix_Time = 0f;
        finger_manager = GameObject.Find("Globe_Tracked");
        Mat.color = Color.white;
        path = "Assets/Log/logs.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("-----------");
        writer.Close();
    }
	
	// Update is called once per frame
	void Update () {

        Current_isOnSphere = finger_manager.GetComponent<Rotate>().Current_isOnSphere;

   

            Vector3 Current_Finger_Position = GameObject.Find("TouchManager").GetComponent<TouchScript.InputSources.TuioInput>().xyzPosition;
            Vector3 marker_Position = gameObject.transform.position;

            count = Is_in_Touch(Current_Finger_Position, marker_Position);


            if (count == true && isComplete == false)
            {

                Current_Time += Time.deltaTime;
                if (Current_Time >= Hold_Time && isComplete == false)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                    isComplete = true;
                    //Get current time for this task from Marker_Generater
                    float time = GameObject.Find("SceneManager").GetComponent<Scene_Manager>().Time_Stemp();
                    Debug.Log("isComplete = true; Time: " + time + "second");
                    Debug.Log("Wait for 3 second to report accuracy");
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.WriteLine("Time is: " + time + " seconds");
                    writer.Close();
                    return;

                }
                return;

            }

            if (isComplete == true && isReported == false)
            {
                Current_Fix_Time += Time.deltaTime;
                if(Current_Fix_Time > 3.0f)
                {
                  //  Vector3 delta = Current_Finger_Position - marker_Position;
                    float dis = Vector3.Distance(Current_Finger_Position, marker_Position);
                    float r = GameObject.Find("Globe_Tracked").GetComponent<SpiralSphere>().radius + 0.004f;
                    float angle = 2 * ((float)Math.Asin(dis / (2 * r)) * 180 / Mathf.PI);
                    isReported = true;
                    Mat.color = Color.gray;
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.WriteLine("The distance is: " + angle + " degree");
                    writer.Close();
                    Debug.Log("Task completed. The distance is: " + angle + " degree");
                }

            }
       
        
    }

    private bool Is_in_Touch(Vector3 Finger_Position, Vector3 Marker_Position)
    {
        bool isTouch;

        float distance = Vector3.Distance(Finger_Position, Marker_Position);
        if (distance < 0.015f)
        {
            isTouch = true;
        }
        else
        {
            isTouch = false;
        }

        return isTouch;

    }
}
