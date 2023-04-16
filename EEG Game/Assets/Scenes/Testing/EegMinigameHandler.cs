using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

using static BCIManager;

public class EegMinigameHandler : MonoBehaviour
{
    // Minigame variables - not relevant for this test
    public int numCubes = 2;
    public float growTime = 1.02f;
    public bool playGame = false;
    private KeyCode[] alphaNums = new[]
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
    };

    public GameObject[] cubes;
    public float[] cubeTimes;

    public int _selectedClass = 0;
    public int _trueSelectedClass = 0;
    private long _selectedTime = 0;
    public long timeThreshold = 1000;

    
    // Start is called before the first frame update
    void Start()
    {
        BCIManager.Instance.ClassSelectionAvailable += OnClassSelectionAvailable;

        StartCoroutine(PlayGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > _selectedTime + timeThreshold)
        {
            if (_trueSelectedClass != _selectedClass)
            {
                _trueSelectedClass = _selectedClass;
                // Debug.LogWarning(string.Format("Selected class: {0}", _trueSelectedClass));
            }
        } 
        else
        {
            _trueSelectedClass = 0;
        }
    }

    private void OnClassSelectionAvailable(object sender, EventArgs e)
    {
        ClassSelectionAvailableEventArgs ea = (ClassSelectionAvailableEventArgs)e;
        if (ea.Class != _selectedClass)
        {
            _selectedClass = (int)ea.Class;
            _selectedTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            // _update = true;
        }
    }

    // test
    public ERPFlashController3D flashController;
    // Keeps track of whether class 3 points to cube 3 or cube 4
    public bool class3IsCube3 = true;

    IEnumerator PlayGame()
    {
        while (true)
        {
            if (playGame)
            {
                if (class3IsCube3)
                {
                    // make class 3 point to cube 4 instead
                    var newObj = new ERPFlashObject3D();
                    newObj.ClassId = flashController.ApplicationObjects[2].ClassId;
                    newObj.FlashMaterial = flashController.ApplicationObjects[2].FlashMaterial;
                    newObj.DarkMaterial = flashController.ApplicationObjects[2].DarkMaterial;
                    newObj.GameObject = cubes[3];
                    flashController.ApplicationObjects[2] = newObj;

                    // call some method here to restart the flash controller?

                    Debug.Log("Cube 4 should now be flashing");

                    class3IsCube3 = false;
                }
                else
                {
                    // make class 3 point to cube 3
                    var newObj = new ERPFlashObject3D();
                    newObj.ClassId = flashController.ApplicationObjects[2].ClassId;
                    newObj.FlashMaterial = flashController.ApplicationObjects[2].FlashMaterial;
                    newObj.DarkMaterial = flashController.ApplicationObjects[2].DarkMaterial;
                    newObj.GameObject = cubes[2];
                    flashController.ApplicationObjects[2] = newObj;

                    // call some method here to restart the flash controller?

                    Debug.Log("Cube 3 should now be flashing");

                    class3IsCube3 = true;
                }

                playGame = false;
                /* Minigame - not relevant for test
                for (var i = 0; i < numCubes; i++)
                {
                    if (Input.GetKey(alphaNums[i]) || i + 1 == _trueSelectedClass)
                    {
                        cubes[i].transform.localScale = new Vector3(
                            cubes[i].transform.localScale.x * growTime,
                            cubes[i].transform.localScale.y * growTime,
                            cubes[i].transform.localScale.z * growTime);
                    }
                    else
                    {
                        cubes[i].transform.localScale = new Vector3(
                            cubes[i].transform.localScale.x * cubeTimes[i],
                            cubes[i].transform.localScale.y * cubeTimes[i],
                            cubes[i].transform.localScale.z * cubeTimes[i]);
                    }
                } */
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
