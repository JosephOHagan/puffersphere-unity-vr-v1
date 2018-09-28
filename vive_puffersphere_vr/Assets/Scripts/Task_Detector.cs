using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;


//this script used to test alignment selection tasks. The marker and the target will turn blue if the task is complete
public class Task_Detector : MonoBehaviour {
    private float Hold_Time;
    private float Current_Time;
    private bool count;
    private bool isComplete;
    private bool isReported;
    public float Fix_Time;
    private float Current_Fix_Time;
    public Material Mat;
    private string path;
    // Use this for initialization
    void Start () {
        count = false;
        isComplete = false;
        Current_Time = 0f;
        Hold_Time = 0.5f;

        isReported = false;
        Mat.color = Color.white;
        Current_Fix_Time = 0f;
        path = "Assets/Log/logs.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("----------");
        writer.Close();
    }
	
	// Update is called once per frame
	void Update () {

        if (count==true && isComplete==false)
        {
            
            Current_Time += Time.deltaTime;
            //Debug.Log(Current_Time);
            if (Current_Time>= Hold_Time && isComplete == false) {
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                GameObject current_marker = GameObject.FindWithTag("Marker");
                //offline test
                current_marker.GetComponent<SpriteRenderer>().color = Color.green;
                isComplete = true;
                float time = GameObject.Find("SceneManager").GetComponent<Scene_Manager>().Time_Stemp();
                Debug.Log("isComplete = true; Time is:"+ time+" Seconds");
                Debug.Log("Wait for 3 second to report accuracy");
                StreamWriter writer = new StreamWriter(path, true);
                writer.WriteLine("Time is: "+ time + " seconds");
                writer.Close();
                return;

            }
            return;

        }

        if (isComplete == true && isReported == false)
        {
            Current_Fix_Time += Time.deltaTime;
           // Debug.Log(Current_Fix_Time);
            if (Current_Fix_Time >= Fix_Time)
            {
                Vector3 position_dot = GameObject.FindWithTag("Marker").transform.position;
                Vector3 position_circle = GameObject.FindWithTag("Target").transform.position;
                Vector3 delta = position_dot - position_circle;
                float distance = (float)Math.Sqrt(Math.Pow(delta.x, 2) + Math.Pow(delta.y, 2) + Math.Pow(delta.z, 2));
                float r = GameObject.Find("Globe_Tracked").GetComponent<SpiralSphere>().radius + 0.004f;
                float angle = 2*((float)Math.Asin(distance/(2*r))*180/Mathf.PI);
                isReported = true;
                Mat.color = Color.gray;
                Debug.Log("Task completed. The distance is: "+ angle+" degree");


                StreamWriter writer = new StreamWriter(path, true);
                writer.WriteLine("The distance is: " + angle + " degree");
                writer.Close();


                return;
            }
            return;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter");
        if (other.gameObject.tag=="Marker") {
            count = true;
            //Debug.Log("begin to count");
        }
    }


    void OnTriggerExit(Collider other)
    {
       // Debug.Log("OnTriggerExi");
        if (other.gameObject.tag == "Marker")
        {
            count = false;
            Current_Time = 0.0f;
            //Debug.Log("reset count");

        }
    }




}
