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

    public Light light;
    public Color lightColor;

    public Transform test;

    public bool debug = true;
    public bool mix = false;

    private List<Shoe> shoes = new List<Shoe>();

    // Start is called before the first frame update
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        backgroundImage.sprite = Resources.Load<Sprite>("Sprites/background" + backgroundIndex);
        GameObject source = Resources.Load<GameObject>("Shoes/business");
        light.color = lightColor;

        if (shoeMatrix > 5)
        {
            shoeMatrix = 5;
        }

        for (int x = 0; x < shoeMatrix; x++)
        {

            //double rDouble = r.NextDouble() * range; //for doubles
            float xc = topLeftCoord.x + (Mathf.Abs(bottomRightCoord.x - topLeftCoord.x) / 5) * Random.Range(0, 4);
            float yc = topLeftCoord.y - (Mathf.Abs(bottomRightCoord.y - topLeftCoord.y) / 5) * Random.Range(0, 4);
            float zc = topLeftCoord.z - (Mathf.Abs(bottomRightCoord.z - topLeftCoord.z) / 5) * Random.Range(0, 4);
            Quaternion randomQuat = ResolveShoeQuaternion("business"); //Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));

            GameObject gameobject = Instantiate<GameObject>(source, new Vector3(xc, yc, zc), randomQuat);
            shoes.Add(new Shoe("business", gameobject));
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

        if (gerb < 30)
        {
            shoes.ForEach(shoe =>
            {
                // collider of the shoe
                CapsuleCollider collider = shoe.gameObject.transform.GetComponent<CapsuleCollider>();

                // top left and bottom right corner points
                Vector3 tl = new Vector3(shoe.gameObject.transform.position.x - (collider.height * shoe.gameObject.transform.localScale.y / 2) + collider.center.x, shoe.gameObject.transform.position.y + (collider.radius * shoe.gameObject.transform.localScale.x / 2) + Mathf.Abs(collider.center.y * shoe.gameObject.transform.localScale.y), shoe.gameObject.transform.position.z) + ResolveShoePivot(shoe.type + "_tl");
                Vector3 br = new Vector3(shoe.gameObject.transform.position.x + (collider.height * shoe.gameObject.transform.localScale.y / 2) + collider.center.x, shoe.gameObject.transform.position.y - (collider.radius * shoe.gameObject.transform.localScale.y / 2) - Mathf.Abs(collider.center.y * shoe.gameObject.transform.localScale.y), shoe.gameObject.transform.position.z) + ResolveShoePivot(shoe.type + "_br");

                // 3d coords to 2d pixels
                Vector3 centerShoe = cam.WorldToScreenPoint((tl + br) / 2);
                Debug.Log(centerShoe);
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

            //CreateTrainData(boundingBoxes);
        }
        gerb++;

        if (debug)
        {
            CapsuleCollider collider = test.transform.GetComponent<CapsuleCollider>();
            // top left and bottom right corner points
            Vector3 tl = new Vector3(test.transform.position.x - (collider.height * test.transform.localScale.y / 2) + collider.center.x, test.transform.position.y + (collider.radius * test.transform.localScale.x / 2) + Mathf.Abs(collider.center.y * test.transform.localScale.y), test.transform.position.z) + ResolveShoePivot("business_tl");
            Vector3 br = new Vector3(test.transform.position.x + (collider.height * test.transform.localScale.y / 2) + collider.center.x, test.transform.position.y - (collider.radius * test.transform.localScale.y / 2) - Mathf.Abs(collider.center.y * test.transform.localScale.y), test.transform.position.z) + ResolveShoePivot("business_br");

            Debug.Log(collider.transform.position);
            Debug.Log(tl);
            Debug.Log(br);


            Debug.DrawLine(tl, br, Color.red);
        }
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


    // resolve inaccurate 
    Vector3 ResolveShoePivot(string shoe)
    {
        switch (shoe)
        {
            case "vans_tl":
                return new Vector3(0, 0, 0);
            case "vans_br":
                return new Vector3(5f, -5, 0);
            case "soccer_tl":
                return new Vector3(0, 8f, 0);
            case "soccer_br":
                return new Vector3(0, 8f, 0);
            case "superstar_tl":
                return new Vector3(80f, 15f, 0);
            case "superstar_br":
                return new Vector3(80f, 15f, 0);
            case "converse_tl":
                return new Vector3(0, 15f, 0);
            case "converse_br":
                return new Vector3(0, 15f, 0);
            case "business_tl":
                return new Vector3(-5f, 20f, 0);
            case "business_br":
                return new Vector3(-10f, 7.5f, 0);
            default:
                return new Vector3(0, 0, 0);
        }
    }

    Quaternion ResolveShoeQuaternion(string shoe)
    {
        switch (shoe)
        {
            case "vans":
                return Quaternion.identity;
            case "soccer":
                return Quaternion.identity;
            case "superstar":
                return Quaternion.Euler(0f, 90f, 0f);
            case "converse":
                return Quaternion.Euler(0f, 90f, 0f);
            case "business":
                return Quaternion.Euler(270f, 90f, 0f);
            default:
                return Quaternion.identity;
        }
    }

    float ResolveYRotationPivot(string shoe)
    {
        switch (shoe)
        {
            case "vans":
                return 1;
            case "soccer":
                return 1;
            case "superstar":
                return 100 / 100;
            default:
                return 1;
        }
    }




    public class Shoe
    {
        public string type;
        public GameObject gameObject;

        public Shoe(string type, GameObject gameObject)
        {
            this.type = type;
            this.gameObject = gameObject;
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