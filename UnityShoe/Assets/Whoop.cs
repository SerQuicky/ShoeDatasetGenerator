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


    // enviorement variables
    private float frames = -1;
    private int iteration = 0;
    public bool debug = true;


    private float screenWidth;
    private float screenHeight;

    public float speed = 3;
    public float rotWidth = 10;
    public float rotHeight = 12;
    private float timeCounter = 0;

    private int scene;

    public Vector3 spawnLayerTL;
    public Vector3 spawnLayerBR;
    public List<Vector3> positions = new List<Vector3>();

    // list holders for camera perspektives, generated shoes and 3d shoe model sources
    private List<IShoeInterface> sources;
    private List<IShoeInterface> shoes = new List<IShoeInterface>();


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
            light.color = Random.ColorHSV();
            backgroundImage.sprite = Resources.Load<Sprite>("Sprites/background" + Random.Range(1, 4));


            for (int k = 0; k < shoes.Count; k++)
            {
                Destroy(shoes[k].ShoeType.gameObject);
            }

            shoes = new List<IShoeInterface>();
            positions = new List<Vector3>();
            InitScene();
        }
    }

    // build up a scene
    void InitScene()
    {
        int baseIndex = Random.Range(0, 5);

        for (int x = 0; x < 2; x++)
        {

            int randomIndex = Random.Range(0, 5);
            Debug.Log(Random.Range(0.1f, 1.0f));
            float xc = spawnLayerTL.x + (Mathf.Abs(spawnLayerBR.x + Mathf.Abs(spawnLayerTL.x)) * Random.Range(0.1f, 1.0f));
            float yc = spawnLayerTL.y - (Mathf.Abs(spawnLayerBR.y + Mathf.Abs(spawnLayerTL.y)) * Random.Range(0.1f, 1.0f));
            float zc = spawnLayerTL.z;

            Debug.Log(new Vector3(xc, yc, zc));

            GameObject gameobject = Instantiate<GameObject>(sources[randomIndex].ShoeType, new Vector3(xc, yc, zc), sources[randomIndex].ResolveShoeQuaternion(false));
            positions.Add(new Vector3(xc, yc, zc));
            shoes.Add(indexToShoe(randomIndex, sources[randomIndex].Name, gameobject));
        }

        frames = 0;
    }

    // Update is called once per frame
    void Update()
    {

        timeCounter += Time.deltaTime * speed;
        List<BoundingBox> boundingBoxes = new List<BoundingBox>();


        if (frames < 150 && frames > -1)
        {
            for (int i = 0; i < shoes.Count; i++)
            {
                // update shoe possition
                float x = positions[i].x + Mathf.Cos(timeCounter) * rotWidth;
                float y = positions[i].y + Mathf.Sin(timeCounter) * rotHeight;
                shoes[i].ShoeType.transform.position = new Vector3(x, y, positions[i].z);

                // collider of the shoe
                CapsuleCollider collider = shoes[i].ShoeType.transform.GetComponent<CapsuleCollider>();
                System.Tuple<Vector3, Vector3> tuple = GetSceneTuples(shoes[i], collider);

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
            }
            CreateTrainData(boundingBoxes);
            frames++;
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