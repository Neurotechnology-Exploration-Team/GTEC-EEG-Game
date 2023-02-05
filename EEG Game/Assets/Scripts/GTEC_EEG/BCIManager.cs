using Gtec.Chain.Common.Nodes.Utilities.MatrixLib;
using System;
using UnityEngine;
using static Gtec.Chain.Common.Nodes.InputNodes.ToWorkspace;

public sealed class BCIManager
{
    public event EventHandler ClassSelectionAvailable;

    public class ClassSelectionAvailableEventArgs : EventArgs
    {
        public uint Class { get; set; }
    };

    private static BCIManager _instance = null;
    private ERPSequenceManager _sequenceManager;
    private bool _initialized;

    public static BCIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BCIManager();
            }
            return _instance;
        }
    }

    private BCIManager()
    {
        _initialized = false;
    }

    public void Initialize(uint numberOfClasses)
    {
        if(!_initialized)
        {
            _sequenceManager = new ERPSequenceManager(numberOfClasses);
            ERPBCIManager.Instance.ScoreValueAvailable += OnScoreValueAvailable;
            _initialized = true;
        }      
    }

    public void Uninitialize()
    {
        if(_initialized)
        {
            ERPBCIManager.Instance.ScoreValueAvailable -= OnScoreValueAvailable;
            _initialized = false;
        }
    }

    private void OnScoreValueAvailable(object sender, EventArgs e)
    {
        ToWorkspaceEventArgs ea = (ToWorkspaceEventArgs)e;

        Matrix scoreMatrix = new Matrix(ea.Data);
        double[] scores = scoreMatrix.GetColumn(1);

        //find maxvalue
        bool[] sequence = new bool[scores.Length];
        int maxValPos = 0;
        double maxVal = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            if (i == 0)
            {
                maxVal = scores[i];
                maxValPos = i;
            }

            if (scores[i] > maxVal)
            {
                maxVal = scores[i];
                maxValPos = i;
            }
        }

        //convert to boolean array
        for (int i = 0; i < scores.Length; i++)
        {
            if (i == maxValPos)
                sequence[i] = true;
            else
                sequence[i] = false;
        }

        //get class
        int selectedClass = _sequenceManager.GetSequenceID(sequence);
        ClassSelectionAvailableEventArgs c = new ClassSelectionAvailableEventArgs();
        c.Class = (uint)selectedClass;
        ClassSelectionAvailable?.Invoke(this, c);
    }
}