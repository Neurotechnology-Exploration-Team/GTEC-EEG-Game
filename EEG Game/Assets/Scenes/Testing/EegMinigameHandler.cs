using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EegMinigameHandler : MonoBehaviour
{
    public int numCubes = 2;
    public float growTime = 1.02f;
    public bool playGame = false;

    public GameObject[] cubes;
    public float[] cubeTimes;

    public int _selectedClass = 0;

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
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayGame()
    {
        while (true)
        {
            if (playGame)
            {
                for (var i = 0; i < numCubes; i++)
                {
                    if (Input.GetKey(alphaNums[i]) || i + 1 == _selectedClass)
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
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
