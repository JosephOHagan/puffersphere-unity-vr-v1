using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Valve.VR;
using HI5;

public class Rotate : MonoBehaviour
{

    public float distance_toTouch;
    private Vector3 Current_Finger_Position;
    private Vector3[] Last_Six_Finger_Position;
    private Vector3 center;
    private float radius;
    private int n = 0;
    public bool Current_isOnSphere;
    private bool Last_isOnSphere;
    private float median_angle;
    private float base_damping = 0.999f;
    private float finger_damping = 0.95f;
    private Vector3 cross_damping;
    //private Component positions;

    // private Vector3 Thumb_position;
    private Vector3 Index_position;

    //private Vector3 Middle_position;
    //private Vector3 Ring_position;
    //private Vector3 Pinky_position;


    public bool Simulated_Rotate_With_Inertia;
    // Use this for initialization
    void Start()
    {
        // positions = GameObject.Find("HI5_Right_Human").GetComponent<HI5_Instance>();
        Last_Six_Finger_Position = new Vector3[6];
        radius = 0.3f;
        Last_isOnSphere = false;
        median_angle = 0f;
    }

    // Update is called once per frame
    void Update()
    {


        // Thumb_position = HandBones[4].localPosition;
        Index_position = GameObject.Find("TouchManager").GetComponent<TouchScript.InputSources.TuioInput>().xyzPosition;
        //Middle_position = HandBones[12].localPosition;
        //Ring_position = HandBones[16].localPosition;
        //Pinky_position = HandBones[20].localPosition;
        center = GameObject.Find("Empty").transform.position;
        GameObject.Find("Track_Point").transform.position = Index_position;


        GameObject Current_Marker = GameObject.Find("Track_Point_Marker");       
        Current_Marker.transform.position = new Vector3(Index_position.x + center.x, Index_position.y + center.y, Index_position.z + center.z);
        Current_Marker.transform.LookAt(center, Vector3.up);       

        Current_Finger_Position = Index_position;
        //Debug.Log("Current_Finger_Position:"+ Current_Finger_Position);
        Current_isOnSphere = Is_in_Touch(distance_points(Current_Finger_Position));
        //Debug.Log("Distance:"+ distance_points(Current_Finger_Position));

        if (Current_isOnSphere)
        {

            if (n < 6)
            {

                Last_Six_Finger_Position[n] = Current_Finger_Position;
                if (n > 2)
                {
                    Rotate_in_touch(Last_Six_Finger_Position[n], Last_Six_Finger_Position[n - 1]);
                }
                n++;

            }
            else
            {
                for (int m = 0; m < 5; m++)
                {
                    Last_Six_Finger_Position[m] = Last_Six_Finger_Position[m + 1];
                }
                Last_Six_Finger_Position[5] = Current_Finger_Position;
                n++;
                Rotate_in_touch(Last_Six_Finger_Position[5], Last_Six_Finger_Position[4]);
            }
        }
        else
        {
            if (Last_isOnSphere)
            {
                if (Simulated_Rotate_With_Inertia)
                {
                    //rotate with damping
                    if (n >= 5)
                    {

                        Vector3[] v = new Vector3[6];
                        for (int a = 0; a < 5; a++)
                        {
                            v[a] = Last_Six_Finger_Position[a] - center;
                        }

                        //reset 
                        Last_Six_Finger_Position = new Vector3[6];
                        n = 0;
                        float[] angle = new float[5];
                        for (int b = 0; b < 4; b++)
                        {
                            angle[b] = Vector3.Angle(v[b + 1], v[b]);
                        }
                        Array.Sort(angle);

                        median_angle = -angle[2];
                        cross_damping = Vector3.Cross(v[3], v[2]).normalized;

                        Quaternion previous = gameObject.transform.rotation;
                        gameObject.transform.rotation = Quaternion.AngleAxis(median_angle, cross_damping) * previous;
                    }
                }
                else
                {
                    Last_Six_Finger_Position = new Vector3[6];
                    n = 0;

                }
            }
            else
            {
                if (Simulated_Rotate_With_Inertia)
                {

                    if (median_angle > 0.1f || median_angle < -0.1f)
                    {
                        median_angle = median_angle * base_damping * finger_damping;
                        Quaternion previous = gameObject.transform.rotation;
                        gameObject.transform.rotation = Quaternion.AngleAxis(median_angle, cross_damping) * previous;
                    }


                }
            }

        }


        Last_isOnSphere = Current_isOnSphere;






    }

    private void Rotate_in_touch(Vector3 last_Position, Vector3 Current_Position)

    {
        Quaternion previous = gameObject.transform.rotation;
        Vector3 v2 = last_Position - center;
        Vector3 v1 = Current_Position - center;
        Vector3 cross = Vector3.Cross(v1, v2).normalized;
        float angle = Vector3.Angle(v1, v2);
        gameObject.transform.rotation = Quaternion.AngleAxis(angle, cross) * previous;
        //  Debug.Log("Privious:" + previous + "; curremt:" + gameObject.transform.rotation);
    }


    private bool Is_in_Touch(float distance)
    {
        bool ison;
        if (distance < radius + distance_toTouch)
        // if (distance < radius + 0.1f)
        {
            ison = true;
        }
        else
        {
            ison = false;
        }

        return ison;
    }


    private float distance_points(Vector3 position)
    {
        Vector3 delta = position - center;
        float distance = (float)Math.Sqrt(Math.Pow(delta.x, 2) + Math.Pow(delta.y, 2) + Math.Pow(delta.z, 2));
        return distance;
    }


}
