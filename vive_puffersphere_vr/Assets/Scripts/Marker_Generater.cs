using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


//This script generate maekers on the sphere
public class Marker_Generater : MonoBehaviour {
    public GameObject marker;
    private Vector2[] positionsV2;
    private Vector3[] positionsV3;
    private float radius;
    private Vector3 center;
    private int n = 0;
    private float Current_Time;
    private StreamWriter writer;
    private bool marker_genarete;
    private string path;
    public int index()
    {
        return n;

    }

    public float TimeCount()
    {

        return Current_Time;


    }
    public bool is_marker()
    {

        return marker_genarete;

    }
    // Use this for initialization
    void Start () {
        positionsV2 = new Vector2[18];

        // define points with (longtitude,latitude)
        positionsV2[0] = new Vector2(0f, 45f);
        positionsV2[1] = new Vector2(0f, 45f);
        positionsV2[2] = new Vector2(0f, 45f);
        positionsV2[3] = new Vector2(60f, 45f);
        positionsV2[4] = new Vector2(60f, 45f);
        positionsV2[5] = new Vector2(60f, 45f);
        positionsV2[6] = new Vector2(120f, 45f);
        positionsV2[7] = new Vector2(120f, 45f);
        positionsV2[8] = new Vector2(120f, 45f);
        positionsV2[9] = new Vector2(180f, 45f);
        positionsV2[10] = new Vector2(180f, 45f);
        positionsV2[11] = new Vector2(180f, 45f);
        positionsV2[12] = new Vector2(240f, 45f);
        positionsV2[13] = new Vector2(240f, 45f);
        positionsV2[14] = new Vector2(240f, 45f);
        positionsV2[15] = new Vector2(300f, 45f);
        positionsV2[16] = new Vector2(300f, 45f);
        positionsV2[17] = new Vector2(300f, 45f);
        positionsV3 = TransformCoordinateData(positionsV2);
        Current_Time = 0f;
        marker_genarete = false;
        path = "Assets/Log/logs.txt";
    }

    // Update is called once per frame
    void Update () {
        //when press mouse, detele last marker and generate new one
        if (Input.GetMouseButtonDown(0)) {
            Current_Time = 0f;
            if (!marker_genarete) { marker_genarete = true; }
            //stop at the last point
            if (n == positionsV3.Length) {
                Debug.Log("It is already the last Point");
                writer = new StreamWriter(path, true);
                writer.WriteLine("-------!-!-------Task-All-Complete-------!-!------");
                writer.Close();
                return;
            }

            //destroy last marker
            if (n != 0) {
                Destroy(GameObject.Find((n-1).ToString()));
            }
            GameObject Current_Marker;
            center = GameObject.Find("Empty").transform.position;
            GameObject.Find("Globe_Tracked").transform.rotation = new Quaternion(0,0,0, 1);
            Current_Marker = Instantiate(marker, new Vector3(positionsV3[n].x + center.x, positionsV3[n].y + center.y, positionsV3[n].z + center.z), Quaternion.identity) as GameObject; 
            Current_Marker.transform.LookAt(center,Vector3.up);
            Current_Marker.transform.parent = gameObject.transform;
            Current_Marker.GetComponent<SpriteRenderer>().color = Color.red;
            Current_Marker.name = n.ToString();
            writer = new StreamWriter(path, true);
            float current_time = GameObject.Find("SceneManager").GetComponent<Scene_Manager>().Time_Stemp();
            writer.WriteLine(("Current Time:"+ current_time+"; Task:" + n+"begins" ));
            writer.Close();
            n++;
        }
        Current_Time += Time.deltaTime;//caculate time finishing a task
    }

    //tranform V2 array to V3 array
    public Vector3[] TransformCoordinateData(Vector2[] SphericalCoordinate)
    {

        Vector3[] coordinatesV3 = new Vector3[SphericalCoordinate.Length];
        radius = GameObject.Find("Globe_Tracked").GetComponent<SpiralSphere>().radius+0.003f;
       
        for (int c = 0; c < SphericalCoordinate.Length; c++)
        {
            float longitude = SphericalCoordinate[c].x * Mathf.PI / 180;//fita
            float latitude = (90.0f - SphericalCoordinate[c].y) * Mathf.PI / 180;//theta
            float X = (float)radius * (float)Math.Sin((float)latitude) * (float)Math.Cos((float)longitude);
            float Y = (float)radius * (float)Math.Cos((float)latitude);
            float Z = (float)radius * (float)Math.Sin((float)latitude) * (float)Math.Sin((float)longitude);
            coordinatesV3[c] = new Vector3(X, Y, Z);
        }
        return coordinatesV3;
    }

    
}
