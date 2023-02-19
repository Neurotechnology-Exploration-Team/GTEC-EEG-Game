using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EegBenchmark : MonoBehaviour
{
    private Camera m_MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera = Camera.main;
        createCube(0, 0, 0, 10);
        createCube(0, 0, 1, 5);
        createCube(0, 0, 0, 1);
        createCube(0, 0, 10, 5);
        destroyCube(0, 0, 0);
        destroyAllCubes();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /**
     * Teleports the player to given coordinates.
     *
     * ax, ay, and az are all in degrees, as if you were entering them in the Unity editor
     */
    public void teleportPlayer(float x, float y, float z, float ax, float ay, float az)
    {
        m_MainCamera.transform.position = new Vector3(x, y, z);
        m_MainCamera.transform.eulerAngles = new Vector3(ax, ay, az);
    }
    public void createCube(float x, float y, float z, float L)
    {
        GameObject aCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        aCube.transform.position = new Vector3(x, y, z);
        aCube.transform.localScale = new Vector3(L, L, L);
        aCube.name = "TestingCube";
    }

    public void destroyCube(float x, float y, float z)
    {
        GameObject[] gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
       
        foreach(GameObject gameObject in gameObjects)
        {
       
            if (gameObject.name == "TestingCube" && gameObject.transform.position == new Vector3(x, y, z))
            {
                Destroy(gameObject, 5);
            }
                       
        }
           
    }

    public void destroyAllCubes()
    {
        GameObject[] gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach (GameObject gameObject in gameObjects)
        {

            if (gameObject.name == "TestingCube")
            {
                Destroy(gameObject, 10);
            }

        }

    }
}

