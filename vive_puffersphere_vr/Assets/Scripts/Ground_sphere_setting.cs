using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Ground_sphere_setting : MonoBehaviour {
    private GameObject Vive_Tracker;
    private bool is_set;
    public float x_offset;
    public float y_offset;
    public float z_offset;
    public bool if_simulated_rotation;
    private Quaternion[] current_frame;
    private int n = 0;
    private bool is_turing;
    private bool last_frame_turing;
    private float turn_time;
    private string path;
    // Use this for initialization
    public bool isset()
    {
        return is_set;

    }

    void Start () {
        
        is_set = false;
        Vive_Tracker = GameObject.Find("Device5");
        current_frame = new Quaternion[4];
        last_frame_turing = false;
        turn_time = 0f;
        path = "Assets/Log/logs.txt";
    }
	
	// Update is called once per frame
	void Update () {      
            
            if (!is_set)
            {
                // gameObject.transform.position = new Vector3( Vive_Tracker.transform.position.x+ x_offset, Vive_Tracker.transform.position.y + y_offset, Vive_Tracker.transform.position.z + z_offset);
                is_set = true;            
            }
            if (!if_simulated_rotation)
            {
                // gameObject.transform.rotation = Vive_Tracker.transform.rotation;

                if (n < 4)
                {

                    current_frame[n] = gameObject.transform.rotation;
                    n++;

                }
                else
                {
                    for (int m = 0; m < 3; m++)
                    {
                        current_frame[m] = current_frame[m + 1];
                    }
                    current_frame[3] = gameObject.transform.rotation;
                    n++;
                    is_turing = Checkrotate(current_frame[3], current_frame[0]);
                }

                if(is_turing==true)
                {
                    turn_time += Time.deltaTime;
                    if (!last_frame_turing)
                    {
                        last_frame_turing = true;
                    }

                }
                else
                {
                    if (last_frame_turing == true)
                    {
                        if (turn_time > 0.1f)
                        {
                            StreamWriter writer = new StreamWriter(path, true);
                            writer.WriteLine("Finished operation: Rotation; It lasts:" + turn_time + "Seconds");
                            writer.Close();
                        }
                        turn_time = 0f;
                        last_frame_turing = false;
                    }
                    


                }


            }
       

       
    }

    public bool Checkrotate(Quaternion a, Quaternion b)
    {
        bool state;
        float angle = Quaternion.Angle(a, b);
        if (angle > 0.5f)
        {
            state = true;
        }
        else
        {
            state = false;
        }
        
        return state;
    }
}
