using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

using static BCIManager;

public class BasicEEGTest : MonoBehaviour
{
    public uint _selectedClass = 0;

    // Start is called before the first frame update
    void Start()
    {
        Instance.ClassSelectionAvailable += OnClassSelectionAvailable;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnClassSelectionAvailable(object sender, EventArgs e)
    {
        ClassSelectionAvailableEventArgs ea = (ClassSelectionAvailableEventArgs)e;
        if (ea.Class != _selectedClass)
        {
            _selectedClass = ea.Class;
            // _update = true;
            Debug.LogWarning(string.Format("Selected class: {0}", ea.Class));
        }
    }
}
