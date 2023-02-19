using System.Collections;
using System;
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
        StartCoroutine(cubeAngleTest(-5.125f, 5));
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
    public IEnumerator cubeAngleTest(float distance, float seconds)
    {
        int angle = 0;
        float xValue = 0;
        float zValue = 0;

        createCube(0, 0, 0, 1);
        /**Create a cube of size 1 centered at the origin, where only one side is flashing
        Teleport the player to a predetermined distance away, facing the flashing side of the cube head-on
        Repeat the following:
            Until a predetermined amount of time has passed, print the angle (0 at the start), time since teleport, and focus certainty separated by commas
            If the certainty by the end of the time is less than 0.5, exit loop
            Change the angle by 5 degrees
            Teleport the player to the same distance from the cube, but off at the new angle so that they are no longer head-on and become increasingly angled
         */
        while (1 <= seconds)
        {
            if(angle <= 90)
            {
                xValue = Mathf.Cos((float)((Math.PI / 180) * angle)) * distance;
                zValue = Mathf.Sin((float)((Math.PI / 180) * angle)) * distance;
                teleportPlayer(xValue, 0, zValue, 0, -angle + 90, 0);
                DateTime timeOfTeleport = DateTime.UtcNow;
                long timeOfTeleportMili = new DateTimeOffset(timeOfTeleport).ToUnixTimeMilliseconds();
                angle += 5;
                while (true)
                {
                    DateTime current = DateTime.UtcNow;
                    long currentTimeSeconds = new DateTimeOffset(current).ToUnixTimeMilliseconds();
                    if((currentTimeSeconds - timeOfTeleportMili) > seconds * 1000)
                    {
                        break;
                    }
                    yield return new WaitForSeconds(0.1f);
                    Debug.Log(angle +", " + (currentTimeSeconds - timeOfTeleportMili) + ", " + 1);
                }
                
            }
            else
            {
                break;
            }
        }

        yield return new WaitForSeconds(0.1f);
    }
}

