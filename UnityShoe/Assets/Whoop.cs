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

    public Transform test;

    public bool debug = true;

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
            float xc = topLeftCoord.x + (Mathf.Abs(bottomRightCoord.x - topLeftCoord.x) / 5) * x;
            float yc = topLeftCoord.y - Mathf.Abs(bottomRightCoord.y - topLeftCoord.y);
            Quaternion randomQuat = Quaternion.identity; //Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));

            GameObject gameobject = Instantiate<GameObject>(source, new Vector3(xc, yc, topLeftCoord.z), randomQuat);
            shoes.Add(gameobject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Normal();
    }

    void Normal()
    {
        List<BoundingBox> boundingBoxes = new List<BoundingBox>();

        if (gerb < 5)
        {
            shoes.ForEach(shoe =>
            {
                // collider of the shoe
                CapsuleCollider collider = shoe.transform.GetComponent<CapsuleCollider>();

                // top left and bottom right corner points
                Vector3 tl = new Vector3(shoe.transform.position.x - (collider.height * shoe.transform.localScale.y / 2) + collider.center.x, shoe.transform.position.y + (collider.radius * shoe.transform.localScale.x / 2) + Mathf.Abs(collider.center.y * shoe.transform.localScale.y), 0f);
                Vector3 br = new Vector3(shoe.transform.position.x + (collider.height * shoe.transform.localScale.y / 2) + collider.center.x + 5f, shoe.transform.position.y - (collider.radius * shoe.transform.localScale.y / 2) - Mathf.Abs(collider.center.y * shoe.transform.localScale.y) - 5f, 0f);

                // 3d coords to 2d pixels
                Vector3 centerShoe = cam.WorldToScreenPoint(new Vector3(shoe.transform.position.x, shoe.transform.position.y, 0f));
                Vector3 tl_cord = cam.WorldToScreenPoint(tl);
                Vector3 br_cord = cam.WorldToScreenPoint(br);

                // absolute width and height of the box (top-left to bottom-right cornerpoint)
                float width = Mathf.Abs(tl_cord.x - br_cord.x);
                float height = Mathf.Abs(tl_cord.y - br_cord.y);

                // append bounding box
                boundingBoxes.Add(new BoundingBox(0, new Vector3(centerShoe.x, screenHeight - centerShoe.y, 0f), width, height));

                if(debug)
                {
                    Debug.DrawLine(tl, br, Color.red);
                }
            });

            CreateTrainData(boundingBoxes);
        }
        gerb++;
    }


    void Test()
    {
        Vector3 centerShoe = cam.WorldToScreenPoint(test.position);
        Debug.Log(centerShoe);
        ScreenCapture.CaptureScreenshot("image_test.png");
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

            string path = Application.dataPath + "/training_data/image_" + gerb + ".txt";
            Debug.Log(Application.dataPath);
            File.WriteAllText(path, text);
            ScreenCapture.CaptureScreenshot("./Assets/training_data/image_" + gerb + ".png");
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