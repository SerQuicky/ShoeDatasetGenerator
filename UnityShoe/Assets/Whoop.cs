using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class Whoop : MonoBehaviour
{

    // unity object references
    public Image backgroundImage;
    public GameObject background;
    public Camera cam;
    public Light light;
    public GameObject testShoe;
    private Business fullShoe;


    // enviorement variables
    private float frames = -1;
    public int shoeMatrix = 1;
    private int iteration = 0;
    public bool debug = true;
    public bool mix = false;


    private float screenWidth;
    private float screenHeight;

    private int scene;

    // list holders for camera perspektives, generated shoes and 3d shoe model sources
    private List<IShoeInterface> sources;
    private List<IShoeInterface> shoes = new List<IShoeInterface>();
    private List<CameraSettings> settings = new List<CameraSettings>()
    {
        new CameraSettings(new Vector3(-0.7f,7f,-783.4f), Quaternion.identity, new Vector3(-2.29f, 13.71f, 75f), Quaternion.identity, new Vector3(-200, 120, 10), new Vector3(293, 108, -200)),
        new CameraSettings(new Vector3(-768, 621, 134), Quaternion.Euler(50f, 90f, 0f), new Vector3(-112f, -58f, 75f), Quaternion.Euler(70f, 90f, 0f), new Vector3(-260, 150, 200), new Vector3(-210, 150, -140)),
        new CameraSettings(new Vector3(-768, 621, 134), Quaternion.Euler(50f, -90f, 0f), new Vector3(-1272f, -36f, 234f), Quaternion.Euler(70f, -90f, 0f), new Vector3(-1284, 150, 200.1f), new Vector3(-1220.8f, 150, 65)),
        new CameraSettings(new Vector3(-0.7f,7f,-783.4f), Quaternion.identity, new Vector3(-2.29f, 13.71f, 75f), Quaternion.identity, new Vector3(-200, 120, 10), new Vector3(293, 108, -200)),
        new CameraSettings(new Vector3(-8,798,117), Quaternion.Euler(90, 0, 0f), new Vector3(-2.29f, 13.71f, 75f), Quaternion.Euler(90, 0, 0f), new Vector3(-149.7f, 250f, 190.1f), new Vector3(192.1f, 250f, 118.8f)),
    };


    // Start is called before the first frame update
    void Start()
    {
        // get width and height of the camera view (screen)
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        // load the 3d shoe models
        sources = new List<IShoeInterface>()
        {
            new Vans("vans", Resources.Load<GameObject>("Shoes/vans")),
            new Soccer("soccer", Resources.Load<GameObject>("Shoes/soccer")),
            new Converse("converse", Resources.Load<GameObject>("Shoes/converse")),
            new Superstar("superstar", Resources.Load<GameObject>("Shoes/superstar")),
            new Business("business", Resources.Load<GameObject>("Shoes/business"))
        };

        // just for debug purposes
        fullShoe = new Business("business", testShoe);

        // set a max value for shoes in the screen
        if (shoeMatrix > 5)
        {
            shoeMatrix = 5;
        }


        // start the routine for the sceen switching
        StartCoroutine(InitScript());
    }

    IEnumerator InitScript()
    {
        // how many scenes should be simulated?
        for (iteration = 0; iteration < 10; iteration++)
        {
            // each simulation runs around 10 seconds
            yield return new WaitForSeconds(10);

            // set random scene, color, background image and destroy all shoes from the previous scene
            scene = Random.Range(0, 5);
            light.color = Random.ColorHSV();
            backgroundImage.sprite = Resources.Load<Sprite>("Sprites/background" + Random.Range(1, 4));


            for (int k = 0; k < shoes.Count; k++)
            {
                Destroy(shoes[k].ShoeType.gameObject);
            }

            shoes = new List<IShoeInterface>();
            InitScene();
        }
    }

    // build up a scene
    void InitScene()
    {
        cam.transform.position = settings[scene].position;
        cam.transform.rotation = settings[scene].angle;
        background.transform.position = settings[scene].backgroundPosition;
        background.transform.rotation = settings[scene].backgroundAngle;

        int baseIndex = Random.Range(0, 5);
        //int baseIndex = 4;

        for (int x = 0; x < shoeMatrix; x++)
        {

            int randomIndex = mix ? Random.Range(0, 5) : baseIndex;
            float xc = settings[scene].layerPositionTL.x + (Mathf.Abs(settings[scene].layerPositionBR.x - settings[scene].layerPositionTL.x)) * Random.Range(0, shoeMatrix) / shoeMatrix;
            float yc = settings[scene].layerPositionTL.y - (Mathf.Abs(settings[scene].layerPositionBR.y - settings[scene].layerPositionTL.y)) * Random.Range(0, shoeMatrix) / shoeMatrix;
            float zc = settings[scene].layerPositionTL.z - (Mathf.Abs(settings[scene].layerPositionBR.z - settings[scene].layerPositionTL.z)) * Random.Range(0, shoeMatrix) / shoeMatrix;

            GameObject gameobject = Instantiate<GameObject>(sources[randomIndex].ShoeType, new Vector3(xc, yc, zc), sources[randomIndex].ResolveShoeQuaternion(scene == 3));
            shoes.Add(indexToShoe(randomIndex, sources[randomIndex].Name, gameobject));
        }

        frames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        List<BoundingBox> boundingBoxes = new List<BoundingBox>();

        if (frames < 50 && frames > -1)
        {
            shoes.ForEach(shoe =>
            {
                // collider of the shoe
                CapsuleCollider collider = shoe.ShoeType.transform.GetComponent<CapsuleCollider>();
                System.Tuple<Vector3, Vector3> tuple = GetSceneTuples(shoe, collider);

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
                    Debug.DrawLine(tuple.Item1, tuple.Item2, Color.red);
                }
            });
            CreateTrainData(boundingBoxes);
            frames++;
        }

        if (debug)
        {
            DebugSettings();
        }
    }


    // save frame for the trainingsdata
    void CreateTrainData(List<BoundingBox> boundingBoxes)
    {
        if(frames != 0)
        {
            string text = "";
            boundingBoxes.ForEach(box =>
            {
                text += box.classifier + " " + (box.center.x / screenWidth) + " " + (box.center.y / screenHeight) + " " + (box.width / screenWidth) + " " + (box.height / screenHeight) + "\n";
            });

            string path = Application.dataPath + "../../trainings_data/labels/image_" + +iteration + "" + frames + ".txt";
            File.WriteAllText(path, text);
            ScreenCapture.CaptureScreenshot(System.IO.Path.Combine("trainings_data/images/", "image_" + iteration + "" + frames + ".png"));
        }
    }

    /* Helper functions */


    void DebugSettings()
    {
        //CapsuleCollider collider = test.transform.GetComponent<CapsuleCollider>();

        // top left and bottom right corner points
        //System.Tuple<Vector3, Vector3> temp = fullShoe.GetFrontCoords(collider);

        //Debug.DrawLine(tl, br, Color.red);
        //Debug.DrawLine(temp.Item1, temp.Item2, Color.red);
    }




    /* Resolver functions */

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


    public System.Tuple<Vector3, Vector3> GetSceneTuples(IShoeInterface shoe, CapsuleCollider collider)
    {
        switch (scene)
        {
            case 0:
                return shoe.GetFrontCoords(collider);
            case 1:
                return shoe.GetTopLeftCoords(collider);
            case 2:
                return shoe.GetTopRightCoords(collider);
            case 3:
                return shoe.GetFrontCoords(collider);
            case 4:
                return shoe.GetTopCoords(collider);
            default:
                return shoe.GetFrontCoords(collider);
        }
    }


    /* Simple classes for some settings or data storage */


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