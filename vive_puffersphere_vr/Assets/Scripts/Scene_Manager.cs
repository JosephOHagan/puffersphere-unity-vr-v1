using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Scene_Manager : MonoBehaviour
{
    public bool if_points_rotate;
    private float time_stamp;
    private string path;

    public float Time_Stemp()
    {
        return time_stamp;

    }

    // Use this for initialization
    void Start()
    {
        time_stamp = 0f;
        string scenename =SceneManager.GetActiveScene().name;
        path = "Assets/Log/logs.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("------Current Scene:------"+ scenename+"-------------");
        writer.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            if (!if_points_rotate) {
                GameObject globe = GameObject.Find("Globe_Tracked");
                globe.transform.rotation = Quaternion.identity;
            }
            else
            {
                GameObject globe = GameObject.Find("Points");
                globe.transform.rotation = Quaternion.identity;

            }
        }

        time_stamp += Time.deltaTime;


    }
}
