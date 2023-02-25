using System.Collections;
using UnityEngine;
using System;

public class EegBenchmark : MonoBehaviour
{
    private Camera mMainCamera;
    // Start is called before the first frame update
    private void Start()
    {
        mMainCamera = Camera.main;
        // StartCoroutine(cubeAngleTest(-5.125f, 5));
        /*createCube(0, 0, 0, 10);
        createCube(0, 0, 1, 5);
        createCube(0, 0, 0, 1);
        createCube(0, 0, 10, 5);
        destroyCube(0, 0, 0);
        destroyAllCubes();*/
        // StartCoroutine(distanceTest(3000));
    }

    // Update is called once per frame
    private void Update()
    {

    }
    
    /**
     * Teleports the player to given coordinates.
     *
     * ax, ay, and az are all in degrees, as if you were entering them in the Unity editor
     */
    private void teleportPlayer(float x, float y, float z, float ax, float ay, float az)
    {
        mMainCamera.transform.position = new Vector3(x, y, z);
        mMainCamera.transform.eulerAngles = new Vector3(ax, ay, az);
    }
    
    private void createCube(float x, float y, float z, float L)
    {
        GameObject aCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        aCube.transform.position = new Vector3(x, y, z);
        aCube.transform.localScale = new Vector3(L, L, L);
        aCube.name = "TestingCube";
    }

    private void destroyCube(float x, float y, float z)
    {
        GameObject[] gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach (GameObject gameObject in gameObjects)
        {

            if (gameObject.name == "TestingCube" && gameObject.transform.position == new Vector3(x, y, z))
            {
                Destroy(gameObject, 5);
            }

        }

    }

    private void destroyAllCubes()
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
    private IEnumerator cubeAngleTest(float distance, float seconds)
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

    private IEnumerator distanceTest(float time_change)
    {
        System.Random rng = new System.Random();

        createCube(0, 0, 0, 1);
        var dist = 1;
        teleportPlayer(dist, 0, 0, 0, 270, 0);
        int certainty = rng.Next(10);
        while (true)
        {
            DateTime start = DateTime.UtcNow;
            long startTimeMilliseconds = new DateTimeOffset(start).ToUnixTimeMilliseconds();
            teleportPlayer(dist*=2, 0, 0, 0, 270, 0);
            while (true)
            {
                UnityEngine.Debug.LogFormat("{0}, {1}", dist, certainty);
                DateTime now = DateTime.UtcNow;
                long unixTimeMilliseconds = new DateTimeOffset(now).ToUnixTimeMilliseconds();
                yield return new WaitForSeconds(.1f);
                if (unixTimeMilliseconds - startTimeMilliseconds > time_change)
                {
                    break;
                }
            }
        }
    }
}

