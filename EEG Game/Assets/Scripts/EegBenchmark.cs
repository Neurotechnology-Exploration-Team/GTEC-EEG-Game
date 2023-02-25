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
    private void TeleportPlayer(float x, float y, float z, float ax, float ay, float az)
    {
        mMainCamera.transform.position = new Vector3(x, y, z);
        mMainCamera.transform.eulerAngles = new Vector3(ax, ay, az);
    }
    
    private void CreateCube(float x, float y, float z, float L)
    {
        var aCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        aCube.transform.position = new Vector3(x, y, z);
        aCube.transform.localScale = new Vector3(L, L, L);
        aCube.name = "TestingCube";
    }

    private void DestroyCube(float x, float y, float z)
    {
        var gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach (var gameObject in gameObjects)
        {

            if (gameObject.name == "TestingCube" && gameObject.transform.position == new Vector3(x, y, z))
            {
                Destroy(gameObject, 5);
            }

        }

    }

    private void DestroyAllCubes()
    {
        var gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach (var gameObject in gameObjects)
        {

            if (gameObject.name == "TestingCube")
            {
                Destroy(gameObject, 10);
            }

        }
    }
    private IEnumerator CubeAngleTest(float distance, float seconds)
    {
        var angle = 0;
        var xValue = 0f;
        var zValue = 0f;

        CreateCube(0, 0, 0, 1);
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
                TeleportPlayer(xValue, 0, zValue, 0, -angle + 90, 0);
                var timeOfTeleport = DateTime.UtcNow;
                var timeOfTeleportMili = new DateTimeOffset(timeOfTeleport).ToUnixTimeMilliseconds();
                angle += 5;
                while (true)
                {
                    var current = DateTime.UtcNow;
                    var currentTimeSeconds = new DateTimeOffset(current).ToUnixTimeMilliseconds();
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

    private IEnumerator DistanceTest(float time_change)
    {
        System.Random rng = new System.Random();

        CreateCube(0, 0, 0, 1);
        var dist = 1;
        TeleportPlayer(dist, 0, 0, 0, 270, 0);
        var certainty = rng.Next(10);
        while (true)
        {
            var start = DateTime.UtcNow;
            var startTimeMilliseconds = new DateTimeOffset(start).ToUnixTimeMilliseconds();
            TeleportPlayer(dist*=2, 0, 0, 0, 270, 0);
            while (true)
            {
                UnityEngine.Debug.LogFormat("{0}, {1}", dist, certainty);
                var now = DateTime.UtcNow;
                var unixTimeMilliseconds = new DateTimeOffset(now).ToUnixTimeMilliseconds();
                yield return new WaitForSeconds(.1f);
                if (unixTimeMilliseconds - startTimeMilliseconds > time_change)
                {
                    break;
                }
            }
        }
    }
}

