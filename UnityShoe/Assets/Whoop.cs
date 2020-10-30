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
    public GameObject background;
    public Camera cam;

    public int shoeMatrix = 1;

    public int run = 1;

    private float screenWidth;
    private float screenHeight;

    public Light light;
    public Color lightColor;

    public Transform test;

    public GameObject testShoe;
    private Business fullShoe;

    public bool debug = true;
    public bool mix = false;



    private int uff = 2;
    private List<IShoeInterface> sources;


    private List<IShoeInterface> shoes = new List<IShoeInterface>();
    List<int> test2 = new List<int>() { 3, 4, 5 };

    private List<CameraSettings> settings = new List<CameraSettings>()
    {
        new CameraSettings(new Vector3(-0.7f,7f,-783.4f), Quaternion.identity, new Vector3(-2.29f, 13.71f, 75f), Quaternion.identity, new Vector3(-265, 150, 10), new Vector3(293, 108, -200)),
        new CameraSettings(new Vector3(-768, 621, 134), Quaternion.Euler(50f, 90f, 0f), new Vector3(-112f, -58f, 75f), Quaternion.Euler(70f, 90f, 0f), new Vector3(-260, 133, 200), new Vector3(-210, 133, -140)),
        new CameraSettings(new Vector3(-768, 621, 134), Quaternion.Euler(50f, -90f, 0f), new Vector3(-1272f, -36f, 234f), Quaternion.Euler(70f, -90f, 0f), new Vector3(-1284, 133, 200.1f), new Vector3(-1220.8f, 133, 65))
    };


    // Start is called before the first frame update
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        backgroundImage.sprite = Resources.Load<Sprite>("Sprites/background" + backgroundIndex);
        light.color = lightColor;
        sources = new List<IShoeInterface>()
        {
            new Vans("vans", Resources.Load<GameObject>("Shoes/vans")),
            new Soccer("soccer", Resources.Load<GameObject>("Shoes/soccer")),
            new Converse("converse", Resources.Load<GameObject>("Shoes/converse")),
            new Superstar("superstar", Resources.Load<GameObject>("Shoes/superstar")),
            new Business("business", Resources.Load<GameObject>("Shoes/business"))
        };

        fullShoe = new Business("business", testShoe);

        if (shoeMatrix > 5)
        {
            shoeMatrix = 5;
        }

        initScene();
    }


    void initScene()
    {
        cam.transform.position = settings[uff].position;
        cam.transform.rotation = settings[uff].angle;
        background.transform.position = settings[uff].backgroundPosition;
        background.transform.rotation = settings[uff].backgroundAngle;

        //int baseIndex = Random.Range(0, 5);
        // TODO: Check x y z in shoes for front........
        int baseIndex = 4;

        for (int x = 0; x < shoeMatrix; x++)
        {

            int randomIndex = mix ? Random.Range(0, 5) : baseIndex;
            float xc = settings[uff].layerPositionTL.x + (Mathf.Abs(settings[uff].layerPositionBR.x - settings[uff].layerPositionTL.x) / 10) * Random.Range(0, 10);
            float yc = settings[uff].layerPositionTL.y - (Mathf.Abs(settings[uff].layerPositionBR.y - settings[uff].layerPositionTL.y) / 10) * Random.Range(0, 10);
            float zc = settings[uff].layerPositionTL.z - (Mathf.Abs(settings[uff].layerPositionBR.z - settings[uff].layerPositionTL.z) / 10) * Random.Range(0, 10);

            Debug.Log(new Vector3(xc, yc, zc));


            GameObject gameobject = Instantiate<GameObject>(sources[randomIndex].ShoeType, new Vector3(xc, yc, zc), sources[randomIndex].ResolveShoeQuaternion());
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
                System.Tuple<Vector3, Vector3> tuple = shoe.GetTopRightCoords(collider);

                // 3d coords to 2d pixels
                Vector3 centerShoe = cam.WorldToScreenPoint((tuple.Item1 + tuple.Item2) / 2);
                Vector3 tl_cord = cam.WorldToScreenPoint(tuple.Item1);
                Vector3 br_cord = cam.WorldToScreenPoint(tuple.Item2);

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


    public IShoeInterface indexToShoe(int index, string name, GameObject gameObject)
    {
        switch (index)
        {
            case 0:
                return new Vans(name, gameObject);
            case 1:
                return new Soccer(name, gameObject);
            case 2:
                return new Converse(name, gameObject);
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

    public class CameraSettings
    {
        public Vector3 position;
        public Quaternion angle;
        public Vector3 backgroundPosition;
        public Quaternion backgroundAngle;
        public Vector3 layerPositionTL;
        public Vector3 layerPositionBR;


        public CameraSettings(Vector3 position, Quaternion angle, Vector3 backgroundPosition, Quaternion backgroundAngle, Vector3 layerPositionTL, Vector3 layerPositionBR)
        {
            this.position = position;
            this.angle = angle;
            this.backgroundPosition = backgroundPosition;
            this.backgroundAngle = backgroundAngle;
            this.layerPositionTL = layerPositionTL;
            this.layerPositionBR = layerPositionBR;
        }
    }
}