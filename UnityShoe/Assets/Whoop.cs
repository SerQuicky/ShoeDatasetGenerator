using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class Whoop : MonoBehaviour
{

    private float gerb = 0;

    public int backgroundIndex = 1;
    public Image backgroundImage;
    public Camera cam;

    public int shoeMatrix = 1;

    public int run = 1;

    public Vector3 topLeftCoord;
    public Vector3 bottomRightCoord;

    private float screenWidth;
    private float screenHeight;

    public Light light;
    public Color lightColor;

    public Transform test;

    public GameObject testShoe;
    private Business fullShoe;

    public bool debug = true;
    public bool mix = false;

    private List<IShoeInterface> shoes = new List<IShoeInterface>();

    // Start is called before the first frame update
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        backgroundImage.sprite = Resources.Load<Sprite>("Sprites/background" + backgroundIndex);
        light.color = lightColor;
        IShoeInterface[] sources = { new Vans("vans", Resources.Load<GameObject>("Shoes/vans")), new Soccer("soccer", Resources.Load<GameObject>("Shoes/soccer")),
                                    new Converse("converse", Resources.Load<GameObject>("Shoes/converse")), new Superstar("superstar", Resources.Load<GameObject>("Shoes/superstar")),
                                    new Business("business", Resources.Load<GameObject>("Shoes/business"))};

        fullShoe = new Business("business", testShoe);

        if (shoeMatrix > 5)
        {
            shoeMatrix = 5;
        }




        //int baseIndex = Random.Range(0, 5);
        int baseIndex = 3;

        for (int x = 0; x < shoeMatrix; x++)
        {

            int randomIndex = mix ? Random.Range(0, 5) : baseIndex;
            //double rDouble = r.NextDouble() * range; //for doubles
            float xc = topLeftCoord.x + (Mathf.Abs(bottomRightCoord.x - topLeftCoord.x) / 5) * Random.Range(0, 5);
            float yc = topLeftCoord.y - (Mathf.Abs(bottomRightCoord.y - topLeftCoord.y) / 5) * Random.Range(0, 5);
            float zc = topLeftCoord.z - (Mathf.Abs(bottomRightCoord.z - topLeftCoord.z) / 5) * Random.Range(0, 5);
            Quaternion randomQuat = ResolveShoeQuaternion(sources[randomIndex].Name); //Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));

            GameObject gameobject = Instantiate<GameObject>(sources[randomIndex].ShoeType, new Vector3(xc, yc, zc), randomQuat);
            shoes.Add(indexToShoe(randomIndex, sources[randomIndex].Name, gameobject));
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<BoundingBox> boundingBoxes = new List<BoundingBox>();

        if (gerb < 2500)
        {
            shoes.ForEach(shoe =>
            {
                // collider of the shoe
                CapsuleCollider collider = shoe.ShoeType.transform.GetComponent<CapsuleCollider>();

                System.Tuple<Vector3, Vector3> tuple = shoe.GetFrontCoords(collider);

                Debug.Log(shoe.Name);
                //Debug.Log(tuple);


                // top left and bottom right corner points
                Vector3 tl = new Vector3(shoe.ShoeType.transform.position.x - (collider.height * shoe.ShoeType.transform.localScale.y / 2) + collider.center.x, shoe.ShoeType.transform.position.y + (collider.radius * shoe.ShoeType.transform.localScale.x / 2) + Mathf.Abs(collider.center.y * shoe.ShoeType.transform.localScale.y), shoe.ShoeType.transform.position.z) + ResolveShoePivot(shoe.Name + "_tl");
                Vector3 br = new Vector3(shoe.ShoeType.transform.position.x + (collider.height * shoe.ShoeType.transform.localScale.y / 2) + collider.center.x, shoe.ShoeType.transform.position.y - (collider.radius * shoe.ShoeType.transform.localScale.y / 2) - Mathf.Abs(collider.center.y * shoe.ShoeType.transform.localScale.y), shoe.ShoeType.transform.position.z) + ResolveShoePivot(shoe.Name + "_br");


                // 3d coords to 2d pixels
                Vector3 centerShoe = cam.WorldToScreenPoint((tl + br) / 2);
                Vector3 tl_cord = cam.WorldToScreenPoint(tl);
                Vector3 br_cord = cam.WorldToScreenPoint(br);

                // absolute width and height of the box (top-left to bottom-right cornerpoint)
                float width = Mathf.Abs(tl_cord.x - br_cord.x);
                float height = Mathf.Abs(tl_cord.y - br_cord.y);

                // append bounding box
                boundingBoxes.Add(new BoundingBox(0, new Vector3(centerShoe.x, screenHeight - centerShoe.y, 0f), width, height));

                if (debug)
                {
                    //Debug.DrawLine(tl, br, Color.red);
                    Debug.DrawLine(tuple.Item1, tuple.Item2, Color.red);
                }
            });

            //CreateTrainData(boundingBoxes);
        }
        gerb++;

        if (debug)
        {
            //CapsuleCollider collider = test.transform.GetComponent<CapsuleCollider>();

            // top left and bottom right corner points
            //System.Tuple<Vector3, Vector3> temp = fullShoe.GetFrontCoords(collider);

            //Debug.DrawLine(tl, br, Color.red);
            //Debug.DrawLine(temp.Item1, temp.Item2, Color.red);
        }
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

            string path = Application.dataPath + "/training_data/image_" + gerb + "-" + run + ".txt";
            Debug.Log(Application.dataPath);
            File.WriteAllText(path, text);
            ScreenCapture.CaptureScreenshot("./Assets/training_data/image_" + gerb + "-" + run + ".png");
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
                return new Vector3(0f, 7.5f, 0);
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



    public IShoeInterface indexToShoe(int index, string name, GameObject gameObject)
    {
        switch (index)
        {
            case 0:
                return new Vans(name, gameObject);
            case 1:
                return new Converse(name, gameObject);
            case 2:
                return new Soccer(name, gameObject);
            case 3:
                return new Superstar(name, gameObject);
            case 4:
                return new Business(name, gameObject);
            default:
                return new Vans(name, gameObject);
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