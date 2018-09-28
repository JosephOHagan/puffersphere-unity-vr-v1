using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Load_Scene : MonoBehaviour {
    void Start()
    {
        Button this_button = gameObject.GetComponent<Button>();
        this_button.onClick.AddListener(TaskOnClick);
    }
    // Use this for initialization
    void TaskOnClick()
    {
        if(gameObject.name == "1")
        {
            sceneload("Fix_1_FR_Select");
        }
        if (gameObject.name == "2")
        {
            sceneload("Fix_2_RR_Select");
        }
        if (gameObject.name == "3")
        {
            sceneload("Fix_3_FR_Alig");
        }
        if (gameObject.name == "4")
        {
            sceneload("Fix_4_RR_Alig");
        }
        if (gameObject.name == "5")
        {
            sceneload("Rotate_1_FR_Select");
        }
        if (gameObject.name == "6")
        {
            sceneload("Rotate_2_RR_Select");
        }
        if (gameObject.name == "7")
        {
            sceneload("Rotate_3_FR_Alig");
        }
        if (gameObject.name == "8")
        {
            sceneload("Rotate_4_RR_Alig");
        }


    }
    private void sceneload(string a)
    {
        SceneManager.LoadScene(a);

    }
}
