using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Whoop : MonoBehaviour
{

    private float gerb;
    public Transform target;

    public GameObject Object;
    public Camera cam;

    public float screenWidth;
    public float screenHeight;

    void CreateText()
    {
        string path = Application.dataPath + "/Log.txt";
        File.WriteAllText(path, "test");
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateText();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        Debug.Log("Screen-Height is: " + Screen.height + " Screen-Width is:" + Screen.width);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
        Debug.Log("target is " + screenPos.x + " pixels from the left");
        Debug.Log("target is " + screenPos.y + " pixels from the top");

        Bounds bounds = GetShoeBounds();
        //Debug.DrawLine(new Vector3(bounds.min.x, bounds.min.y, 0f), new Vector3(bounds.max.x, bounds.max.y, 0), Color.red);

        Vector3 min = cam.WorldToScreenPoint(bounds.min);
        Vector3 max = cam.WorldToScreenPoint(bounds.max);

        float width = max.x - min.x > 0 ? max.x - min.x : (max.x - min.x) * -1;
        float height = max.y - min.y > 0 ? max.y - min.y : (max.y - min.y) * -1;


        Debug.Log("min: " + min.x + " and " + max.x + " width " + width);
        Debug.Log("max: " + min.y + " and " + max.y + " height " + height);

        if (gerb < 10)
        {
            CreateTrainData(gerb, screenPos.x, screenPos.y, width, height);
            gerb++;
        }
    }

    void CreateTrainData(float index, float centerX, float centerY, float width, float height)
    {
        ScreenCapture.CaptureScreenshot("image_" + index + ".png");
        string path = Application.dataPath + "/image_" + index + ".txt";
        File.WriteAllText(path, "0 " + (centerX / screenWidth) + " " + (centerY / screenHeight) + " " + (width / screenWidth) + " " + (height / screenHeight));
    }

    Bounds GetShoeBounds()
    {
        Bounds bounds = new Bounds(transform.position, Vector3.one);
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        return bounds;
    }
}
