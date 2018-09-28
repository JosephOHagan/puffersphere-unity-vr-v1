using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Valve.VR;

public class LineGenerator : MonoBehaviour {

    public int subdivisions = 12;//360/subdivision should be int;
    public int Points = 360;
    public GameObject empty;
    private float radius;
    private Vector3 center;//sphere center
    private LineRenderer lineRenderer;
    //  private bool is_generated;

    // Use this for initialization

    private void Start () {

        radius = gameObject.GetComponent<SpiralSphere>().radius + 0.001f;
        center = GameObject.Find("Empty").transform.position;
        
         int tile = 180 / Points;
         int part = 360 / subdivisions;
         //draw lines
         for (int i = 0; i < subdivisions; i++)
         {
             //horizontal
             for (int m = -180; m < 180;)
             {

                 Vector3[] V3Points = new Vector3[Points];
                 int num = 0;

                 for (int n = -90; n < 90;)
                 {
                     Vector2 V2Point = new Vector2(m, n);
                     Vector3 V3Point = TransformCoordinateData(V2Point);
                     V3Points[num] = new Vector3(V3Point.x + center.x, V3Point.y + center.y, V3Point.z + center.z);
                     //V3Points[num] = V3Point;
                     num++;
                     n = n + tile;
                 }
                 LineRender(V3Points);

                 m = m + part;
             }

             //vertical
             for (int n = -90; n < 90;)
             {

                 Vector3[] V3Points = new Vector3[2 * Points];
                 int num = 0;

                 for (int m = -180; m < 180;)
                 {
                     Vector2 V2Point = new Vector2(m, n);
                     Vector3 V3Point = TransformCoordinateData(V2Point);
                     V3Points[num] = new Vector3(V3Point.x + center.x, V3Point.y + center.y, V3Point.z + center.z);
                     //V3Points[num] = V3Point;
                     num++;
                     m = m + tile;
                 }
                 LineRender(V3Points);
                 n = n + part;
             }


         }
    }



    //transform the spherical coordinate data to V3 coorsinate
    public Vector3 TransformCoordinateData(Vector2 SphericalCoordinate)
    {
   
            float longitude = SphericalCoordinate.x * Mathf.PI / 180;//fita
            float latitude = (90.0f - SphericalCoordinate.y) * Mathf.PI / 180;//theta
            float X = (float)radius * (float)Math.Sin((float)latitude) * (float)Math.Cos((float)longitude);
            float Y = (float)radius * (float)Math.Cos((float)latitude);
            float Z = (float)radius * (float)Math.Sin((float)latitude) * (float)Math.Sin((float)longitude);
            Vector3 coordinatesV3 = new Vector3(X, Y, Z);
            return coordinatesV3;

    }


    //draw lines
    public void LineRender(Vector3[] coorV3)
    {

            GameObject instant;// = new GameObject();

            instant = Instantiate(empty, gameObject.transform, true) as GameObject;
        //instant.transform.parent = gameObject.transform;

            lineRenderer = (LineRenderer)instant.GetComponent("LineRenderer");
            lineRenderer.useWorldSpace = false;
            lineRenderer.positionCount = coorV3.Length;
            lineRenderer.SetPositions(coorV3);

    }
}
