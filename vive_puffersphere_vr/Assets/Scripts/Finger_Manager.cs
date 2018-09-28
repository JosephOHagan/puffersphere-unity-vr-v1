using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Finger_Manager : MonoBehaviour {
    public GameObject finger;
    private bool is_in_interaction;
    public float testvalue;
    private string path;
    private bool finger_state_last_frame;
    private StreamWriter writer;
    public int scene_id;
    private Quaternion angle_last;
    private Quaternion angle_now;
    private GameObject rotate_obj;
    private float Time_interaction;
    // Use this for initialization

    public bool Is_in_interaction()
    {

        return is_in_interaction;

    }

    void Start () {
        is_in_interaction = false;
        finger_state_last_frame = false;
        path = "Assets/Log/logs.txt";
        
        if (scene_id==1 || scene_id == 3) {
            rotate_obj = GameObject.Find("Points");
            angle_last = rotate_obj.transform.rotation;
        }
        if (scene_id==2 || scene_id == 4)
        {
            rotate_obj = GameObject.Find("Globe_Tracked");
            angle_last = rotate_obj.transform.rotation;
        }
        if (scene_id == 5 || scene_id == 6)
        {
            rotate_obj = GameObject.Find("Empty");
            angle_last = rotate_obj.transform.rotation;
        }

        
        
        Time_interaction = 0f;
    }

    private void NewMethod()
    {
        rotate_obj = GameObject.Find("Globe_Tracked");
    }

    // Update is called once per frame
    void Update () {

        // ====== ====== ====== Change to index point ====== ====== ====== 
        // Vector3 Index_position = GameObject.Find("Human_RightHandIndex3").transform.position;
        Vector3 Index_position = GameObject.Find("TouchManager").GetComponent<TouchScript.InputSources.TuioInput>().xyzPosition;

        Vector3 center = GameObject.Find("Empty").transform.position;

        bool test = true;

        if (Index_position != new Vector3(10000.0f, 10000.0f, 10000.0f) )
        {
            float distance = Vector3.Distance(Index_position, center);
            test = (distance > testvalue);
        }             

        if (test)
        {
            // if (GameObject.FindWithTag("Finger"))
            // {
            //   Destroy(GameObject.FindWithTag("Finger"));
            // }

            Time_interaction = GameObject.Find("SceneManager").GetComponent<Scene_Manager>().Time_Stemp();

            is_in_interaction = false;

            if(is_in_interaction != finger_state_last_frame)
            {
                float current_time = GameObject.Find("SceneManager").GetComponent<Scene_Manager>().Time_Stemp();
                writer = new StreamWriter(path, true);
                writer.WriteLine("Current Time:"+ current_time+"; Event: Hand out.");
                writer.Close();
                if (Time_interaction> 0.1f) {
                    if (scene_id == 1 || scene_id == 2 || scene_id == 3 || scene_id == 4)
                    {
                        angle_now = rotate_obj.transform.rotation;
                        float angle = Quaternion.Angle(angle_now, angle_last);
                        if (angle > 5f)
                        {
                            writer = new StreamWriter(path, true);
                            writer.WriteLine("Finished operation: Rotation; It lasts:" + Time_interaction + "Seconds");
                            writer.Close();
                        }
                        else
                        {
                            writer = new StreamWriter(path, true);
                            writer.WriteLine("Finished operation: Tap; It lasts:" + Time_interaction + "Seconds");
                            writer.Close();
                        }
                    }
                    if (scene_id == 5 || scene_id == 6)
                    {
                        angle_now = rotate_obj.transform.rotation;
                        float angle = Quaternion.Angle(angle_now, angle_last);
                        if (angle < 0.5f)
                        {
                            writer = new StreamWriter(path, true);
                            writer.WriteLine("Finished operation: Tap; It lasts:" + Time_interaction + "Seconds");
                            writer.Close();
                        }
                    }
                }
                finger_state_last_frame = is_in_interaction;
                Time_interaction = 0f;
            }
        }
        else
        {

            Time_interaction += Time.deltaTime;
           // if (!GameObject.FindWithTag("Finger"))
           // {
               // Instantiate(finger, new Vector3(0, 0, 0), Quaternion.identity);
           // }
         
            is_in_interaction = true;
            if (is_in_interaction != finger_state_last_frame)
            {
                float current_time = GameObject.Find("SceneManager").GetComponent<Scene_Manager>().Time_Stemp();
                writer = new StreamWriter(path, true);
                writer.WriteLine("Current Time:" + current_time + "; Event: Hand in.");
                writer.Close();
                finger_state_last_frame = is_in_interaction;
                if (scene_id == 1 || scene_id == 2 || scene_id == 3 || scene_id == 4 || scene_id == 5 || scene_id == 6)
                {
                    angle_last = rotate_obj.transform.rotation;
                   
                }
            }

        }

    }
}
