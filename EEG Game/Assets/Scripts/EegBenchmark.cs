using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

using static BCIManager;

public class EegBenchmark : MonoBehaviour
{
    private Camera mMainCamera;
    private Transform mMainCameraTransform;

    public Material darkMaterial;
    public Material flashMaterial;

    public List<GameObject> cubes;

    public ERPFlashController3D bciManager3D;

    private uint _selectedClass = 0;
    private bool _update = false;

    public enum TestType
    {
        Angle,
        Distance
    }

    public TestType testToRun;
    public bool runTest = false;

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
        
        // TeleportPlayer(0, 0, -20, 0, 0, 0);
        // CreateCube(0, 0, 0, 10);
        // bciManager3D.TrainingObject = bciManager3D.ApplicationObjects[0];

        // BCIManager.Instance.ClassSelectionAvailable += OnClassSelectionAvailable;

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
        if (runTest)
        {
            switch (testToRun)
            {
                case TestType.Angle:
                    StartCoroutine(AngleTest(-5.125f, 5));
                    break;
                case TestType.Distance:
                    StartCoroutine(DistanceTest(500, 3000));
                    break;
            }
            runTest = false;
        }
    }

    private void OnClassSelectionAvailable(object sender, EventArgs e)
    {
        ClassSelectionAvailableEventArgs ea = (ClassSelectionAvailableEventArgs)e;
        _selectedClass = ea.Class;
        _update = true;
        Debug.Log(string.Format("Selected class: {0}", ea.Class));
    }

    private void FixERPList()
    {
        // bciManager3D.NumberOfClasses = (uint)bciManager3D.ApplicationObjects.Count;
        for (var i = 0; i < bciManager3D.ApplicationObjects.Count; i++)
        {
            var erpFlashObject3D = bciManager3D.ApplicationObjects[i];
            erpFlashObject3D.ClassId = i + 1;
            bciManager3D.ApplicationObjects[i] = erpFlashObject3D;
        }
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

    private GameObject CreateCube(float x, float y, float z, float l)
    {
        var aCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        aCube.transform.position = new Vector3(x, y, z);
        aCube.transform.localScale = new Vector3(l, l, l);
        aCube.name = "TestingCube";

        var erpFlashObject3D = new ERPFlashObject3D
        {
            GameObject = aCube,
            FlashMaterial = flashMaterial,
            DarkMaterial = darkMaterial,
            ClassId = 2
        };

        cubes.Add(aCube);
        bciManager3D.ApplicationObjects.Add(erpFlashObject3D);
        FixERPList();

        return aCube;
    }

    private void DestroyCube(float x, float y, float z)
    {
        var cubesToDestroy = new List<int>();
        
        for (int cube = 0; cube < cubes.Count; cube++)
        {
            if (cubes[cube].transform.position == new Vector3(x, y, z))
            {
                cubesToDestroy.Add(cube);
                Destroy(cubes[cube], 5);
            }
        }

        foreach (var cube in cubesToDestroy)
        {
            Destroy(cubes[cube], 5);
            cubes.RemoveAt(cube);
            bciManager3D.ApplicationObjects.RemoveAt(cube);
        }

        FixERPList();
    }

    private void DestroyAllCubes()
    {
        foreach (var cubeGameObject in cubes) 
        {
            Destroy(cubeGameObject, 10);
        }

        bciManager3D.ApplicationObjects.Clear();
        FixERPList();
    }
    
    private IEnumerator AngleTest(float distance, float seconds)
    {
        // DestroyAllCubes();
        
        var angle = 0;

        // CreateCube(0, 0, 0, 1);
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

    private IEnumerator DistanceTest(int maxDistance, float timeChange)
    {
        // DestroyAllCubes();
        
        var rng = new System.Random();

        // CreateCube(0, 0, 0, 1);
        var dist = 1;
        TeleportPlayer(dist, 0, 0, 0, 270, 0);
        var certainty = rng.Next(10);
        while (dist < maxDistance)
        {
            var start = DateTime.UtcNow;
            var startTimeMilliseconds = new DateTimeOffset(start).ToUnixTimeMilliseconds();
            TeleportPlayer(dist*=2, 0, 0, 0, 270, 0);
            while (true)
            {
                Debug.LogFormat("{0}, {1}", dist, certainty);
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

