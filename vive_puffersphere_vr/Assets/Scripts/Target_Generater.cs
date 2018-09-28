using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Target_Generater : MonoBehaviour {
    public GameObject target;
    private int Pre_index;
    private Camera Current_Cam;
    private float radius;
    private int Current_index;
    public bool If_fix;
    private GameObject Current_Target;
    private Vector3 center;
    private Vector3 positionsV3;
    private Vector3 velocity = Vector3.zero;
    // Use this for initialization
    void Start () {
        Pre_index = 0;
        positionsV3 = TransformCoordinateData(new Vector2(0,0));

    }
	
	// Update is called once per frame
	void Update () {

        if (GameObject.Find("Globe_Tracked")) {
            if (If_fix) {
                Current_index = GameObject.Find("Points").GetComponent<Marker_Generater>().index();

            }
            else
            {
                Current_index = GameObject.Find("Globe_Tracked").GetComponent<Marker_Generater>().index();
            }




            //if points index changed,begin the instantiate progress
            if (Pre_index != Current_index)
            {

                if (Pre_index != 0)
                {
                    Destroy(GameObject.Find("Target" + Pre_index.ToString()));
                }


                Pre_index = Current_index;

                GameObject Current_Target;
                center = GameObject.Find("Empty").transform.position;
                Current_Target = Instantiate(target, new Vector3(positionsV3.x + center.x, positionsV3.y + center.y, positionsV3.z + center.z), Quaternion.identity) as GameObject;
                Current_Target.transform.LookAt(center, Vector3.up);
                Current_Target.transform.parent = gameObject.transform;
                Current_Target.name = "Target" + Current_index.ToString();
            }
        }
        
        //fix target position
        //if don't need just delete
        if (GameObject.FindWithTag("Target"))
        {
            // Debug.Log("There is a target");
            GameObject current_target = GameObject.FindWithTag("Target");
            Vector3 target_position = current_target.transform.position;//Target position
            Vector3 current_center = GameObject.Find("Empty").transform.position;//current center position
            float distance = Check_distance(target_position, current_center);
            //Debug.Log(distance);
            if (distance < GameObject.Find("Globe_Tracked").GetComponent<SpiralSphere>().radius)
            {
                Vector3 new_position = new Vector3(positionsV3.x + current_center.x, positionsV3.y + current_center.y, positionsV3.z + current_center.z);
                current_target.transform.position= Vector3.MoveTowards(target_position, new_position, 0.007f);

            }
            if (distance > GameObject.Find("Globe_Tracked").GetComponent<SpiralSphere>().radius + 0.01f)
            {
                Vector3 new_position = new Vector3(positionsV3.x + current_center.x, positionsV3.y + current_center.y, positionsV3.z + current_center.z);
                current_target.transform.position = Vector3.MoveTowards(target_position, new_position, 0.007f);
            }

        }

    
    }

    public float Check_distance(Vector3 a, Vector3 b)
    {
        float distance_check;
        Vector3 delta_position = a - b;
        distance_check = (float)Math.Sqrt(Math.Pow(delta_position.x, 2) + Math.Pow(delta_position.y, 2) + Math.Pow(delta_position.z, 2));
        return distance_check;
    }

    public Vector3 TransformCoordinateData(Vector2 SphericalCoordinate)
    {

        Vector3 coordinatesV3;
        radius = GameObject.Find("Globe_Tracked").GetComponent<SpiralSphere>().radius + 0.003f;

      
            float longitude = SphericalCoordinate.x * Mathf.PI / 180;//fita
            float latitude = (90.0f - SphericalCoordinate.y) * Mathf.PI / 180;//theta
            float X = (float)radius * (float)Math.Sin((float)latitude) * (float)Math.Cos((float)longitude);
            float Y = (float)radius * (float)Math.Cos((float)latitude);
            float Z = (float)radius * (float)Math.Sin((float)latitude) * (float)Math.Sin((float)longitude);
            coordinatesV3 = new Vector3(X, Y, Z);
 
        return coordinatesV3;
    }
}
