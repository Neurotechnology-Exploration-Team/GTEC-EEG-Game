using Gtec.UnityInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gtec.UnityInterface.BCIManager;

public class BasicEEGTest : MonoBehaviour
{
    public ERPFlashController3D eRPFlashController3D;
    public GameObject newGameObj;

    public bool runTest;

    private uint _selectedClass = 0;
    private bool _update = false;
    void Start()
    {
        //attach to class selection available event
        BCIManager.Instance.ClassSelectionAvailable += OnClassSelectionAvailable;
    }

    void OnApplicationQuit()
    {
        //detach from class selection available event
        BCIManager.Instance.ClassSelectionAvailable -= OnClassSelectionAvailable;
    }

    void Update()
    {
        if (runTest)
        {
            if (eRPFlashController3D.ApplicationObjects.Count == 2)
            {
                ERPFlashObject3D eRPFlashObject3D = new ERPFlashObject3D();
                eRPFlashObject3D.GameObject = newGameObj;
                eRPFlashObject3D.FlashMaterial = eRPFlashController3D.ApplicationObjects[1].FlashMaterial;
                eRPFlashObject3D.DarkMaterial = eRPFlashController3D.ApplicationObjects[1].DarkMaterial;
                eRPFlashObject3D.ClassId = eRPFlashController3D.ApplicationObjects[1].ClassId;

                eRPFlashController3D.ApplicationObjects.Add(eRPFlashObject3D);
            } else
            {
                eRPFlashController3D.ApplicationObjects.RemoveAt(2);
            }

            runTest = false;
        }
        //TODO ADD YOUR CODE HERE
        if (_update)
        {
            switch (_selectedClass)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;

            }
            _update = false;
        }
    }

    /// <summary>
    /// This event is called whenever a new class selection is available. Th
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnClassSelectionAvailable(object sender, System.EventArgs e)
    {
        ClassSelectionAvailableEventArgs ea = (ClassSelectionAvailableEventArgs)e;
        if (_selectedClass != ea.Class)
        {
            _selectedClass = ea.Class;
            _update = true;
            Debug.Log(string.Format("Selected class: {0}", ea.Class));
        }
    }
}
