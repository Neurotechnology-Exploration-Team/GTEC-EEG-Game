using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class EegBenchmark : MonoBehaviour
{
    private Camera mMainCamera;
    private Transform mMainCameraTransform;

    private List<GameObject> cubes;
    
    // Start is called before the first frame update
    private void Start()
    {
        mMainCamera = Camera.main;
        if (!mMainCamera)
        {
            Debug.LogError("No object with name 'Main Camera' found. Aborting tests.");
            return;
        }
        mMainCameraTransform = mMainCamera.transform;
        // StartCoroutine(AngleTest(-5.125f, 5));
        /*createCube(0, 0, 0, 10);
        createCube(0, 0, 1, 5);
        createCube(0, 0, 0, 1);
        createCube(0, 0, 10, 5);
        destroyCube(0, 0, 0);
        destroyAllCubes();*/
        // StartCoroutine(DistanceTest(3000));
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
        mMainCameraTransform.position = new Vector3(x, y, z);
        mMainCameraTransform.eulerAngles = new Vector3(ax, ay, az);
    }

    private void CreateCube(float x, float y, float z, float l)
    {
        var aCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        aCube.transform.position = new Vector3(x, y, z);
        aCube.transform.localScale = new Vector3(l, l, l);
        aCube.name = "TestingCube";

        cubes.Add(aCube);
    }

    private void DestroyCube(float x, float y, float z)
    {
        foreach (var cubeGameObject in cubes)
        {
            if (cubeGameObject.transform.position == new Vector3(x, y, z))
            {
                Destroy(cubeGameObject, 5);
            }
        }

    }

    private void DestroyAllCubes()
    {
        foreach (var cubeGameObject in cubes) 
        {
            Destroy(cubeGameObject, 10);
        }
    }
    
    private IEnumerator AngleTest(float distance, float seconds)
    {
        var angle = 0;

        CreateCube(0, 0, 0, 1);
        while (true)
        {
            if (angle > 90) break;
            
            var xValue = Mathf.Cos((float)((Math.PI / 180) * angle)) * distance;
            var zValue = Mathf.Sin((float)((Math.PI / 180) * angle)) * distance;
            
            TeleportPlayer(xValue, 0, zValue, 0, -angle + 90, 0);
            var timeOfTeleport = DateTime.UtcNow;
            var timeOfTeleportMili = new DateTimeOffset(timeOfTeleport).ToUnixTimeMilliseconds();
            
            angle += 5;
            
            while (true)
            {
                var current = DateTime.UtcNow;
                var currentTimeSeconds = new DateTimeOffset(current).ToUnixTimeMilliseconds();
                if (currentTimeSeconds - timeOfTeleportMili > seconds * 1000)
                {
                    break;
                }

                yield return new WaitForSeconds(0.1f);
                Debug.Log(angle + ", " + (currentTimeSeconds - timeOfTeleportMili) + ", " + 1);
            }
        }

        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator DistanceTest(float timeChange)
    {
        var rng = new System.Random();

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
                if (unixTimeMilliseconds - startTimeMilliseconds > timeChange)
                {
                    break;
                }
            }
        }
    }
}

