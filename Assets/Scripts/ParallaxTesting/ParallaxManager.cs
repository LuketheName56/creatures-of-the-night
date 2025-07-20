using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPosition;
    float distance; //from camera start position to environment element

    GameObject[] backgrounds;
    Vector3[] startPositions;
    float[] backSpeed;

    float farthestBack;

    [Range(.01f, 0.05f)]
    public float parallaxSpeed;

    void Start()
    {
        cam = Camera.main.transform;
        camStartPosition = cam.position;

        int numOfEnvironmentObjects = transform.childCount;
        startPositions = new Vector3[numOfEnvironmentObjects];

        backSpeed = new float[numOfEnvironmentObjects];
        backgrounds = new GameObject[numOfEnvironmentObjects];

        for (int i = 0; i < numOfEnvironmentObjects; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            startPositions[i] = backgrounds[i].transform.position;
        }
        backSpeedCalculate(numOfEnvironmentObjects);
    }

    void backSpeedCalculate(int numOfEnvironmentObjects)
    {

        for (int i = 0; i < numOfEnvironmentObjects; i++)
        {
            float zDistance = backgrounds[i].transform.position.z - cam.position.z;

            if (zDistance > farthestBack)
            {
                farthestBack = zDistance;
            }
        }

        for (int i = 0; i < numOfEnvironmentObjects; i++)
        {
            float zDistance = backgrounds[i].transform.position.z - cam.position.z;

            //normalizes speed -> easier for assigning range
            backSpeed[i] = 1 - (zDistance / farthestBack);
        }
    }

    private void LateUpdate()
    {
        distance = cam.position.x - camStartPosition.x;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            Vector3 newPosition = startPositions[i] + new Vector3(distance, 0, 0) * speed;
            backgrounds[i].transform.position = newPosition;
        }

    }
}
