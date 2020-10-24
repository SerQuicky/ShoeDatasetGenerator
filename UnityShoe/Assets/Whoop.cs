using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class Whoop : MonoBehaviour
{

    private float gerb = 0;
    //public Transform transform;

    public int backgroundIndex = 1;
    public Image backgroundImage;
    public Camera cam;

    public int shoeMatrix = 1;

    public Vector3 topLeftCoord;
    public Vector3 bottomRightCoord;

    private float screenWidth;
    private float screenHeight;

    private List<GameObject> shoes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        Debug.Log("Screen-Height is: " + Screen.height + " Screen-Width is:" + Screen.width);
        backgroundImage.sprite = Resources.Load<Sprite>("Sprites/background" + backgroundIndex);
        GameObject source = Resources.Load<GameObject>("Shoes/vans");

        if (shoeMatrix > 5)
        {
            shoeMatrix = 5;
        }

        for (int x = 0; x < shoeMatrix; x++)
        {
            for (int y = 0; y < shoeMatrix; y++)
            {
                float xc = topLeftCoord.x + (Mathf.Abs(bottomRightCoord.x - topLeftCoord.x) / 5) * x;
                float yc = topLeftCoord.y - (Mathf.Abs(bottomRightCoord.y - topLeftCoord.y) / 5) * y;
                Quaternion randomQuat = Quaternion.identity; //Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));

                GameObject gameobject = Instantiate<GameObject>(source, new Vector3(xc, yc, topLeftCoord.z), randomQuat);
                shoes.Add(gameobject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<BoundingBox> boundingBoxes = new List<BoundingBox>();

        if(gerb < 10000)
        {
            shoes.ForEach(shoe =>
            {
                Vector3 centerShoe = cam.WorldToScreenPoint(shoe.transform.position);
                CapsuleCollider collider = shoe.transform.GetComponent<CapsuleCollider>();
                Vector3 centerCollider = cam.WorldToScreenPoint(collider.transform.position);
                Vector3 tl = new Vector3(shoe.transform.position.x - (collider.height * shoe.transform.localScale.y / 2), shoe.transform.position.y + (collider.radius * shoe.transform.localScale.x), 0f);
                Vector3 br = new Vector3(shoe.transform.position.x + (collider.height * shoe.transform.localScale.y / 2), shoe.transform.position.y - (collider.radius * shoe.transform.localScale.y), 0f);
                float width = Mathf.Abs(tl.x - br.x);
                float height = Mathf.Abs(tl.y - br.y);
                Debug.DrawLine(tl, br, Color.red);
                Debug.Log(centerShoe);
                boundingBoxes.Add(new BoundingBox(0, new Vector3(centerShoe.x, screenHeight - centerShoe.y, 0f), width, height));
                Debug.DrawLine(tl, br, Color.red);
            });

            //CreateTrainData(boundingBoxes);
        }
        gerb++;


        /*Vector3 screenPos = cam.WorldToScreenPoint(transform.position);

        Bounds bounds = GetShoeBounds(transform);
        Vector3 min = cam.WorldToScreenPoint(bounds.min);
        Vector3 max = cam.WorldToScreenPoint(bounds.max);
        float width = Mathf.Abs(max.x - min.x);
        float height = Mathf.Abs(max.y - min.y);
        Debug.DrawLine(new Vector3(bounds.min.x, bounds.min.y, 0f), new Vector3(bounds.max.x, bounds.max.y, 0), Color.red);*/
    }

    void CreateTrainData(List<BoundingBox> boundingBoxes)
    {
        if(gerb != 0)
        {
            string text = "";
            boundingBoxes.ForEach(box =>
            {
                text += box.classifier + " " + (box.center.x / screenWidth) + " " + (box.center.y / screenHeight) + " " + (box.width / screenWidth) + " " + (box.height / screenHeight) + "\n";
            });

            string path = Application.dataPath + "/image_" + gerb + ".txt";
            File.WriteAllText(path, text);
            ScreenCapture.CaptureScreenshot("image_" + gerb + ".png");
        }
    }


    public class BoundingBox
    {
        public float classifier;
        public Vector3 center;
        public float width;
        public float height;


        public BoundingBox(float classifier, Vector3 center, float width, float height)
        {
            this.classifier = classifier;
            this.center = center;
            this.width = width;
            this.height = height;
        }
    }
}