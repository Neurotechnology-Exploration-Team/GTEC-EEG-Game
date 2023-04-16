using Gtec.Chain.Common.Nodes.Utilities.LDA;
using Gtec.Chain.Common.SignalProcessingPipelines;
using Gtec.Chain.Common.Templates.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static ERPBCIManager;
using static Gtec.Chain.Common.Templates.DataAcquisitionUnit.DataAcquisitionUnit;

public class BCIManager3D : MonoBehaviour
{
    private ERPFlashController3D _flashController;
    private Canvas _cvTraining;
    private Canvas _cvConnectionDialog;
    private Canvas _cvTrainingCompletedDialog;
    private ConnectionDialog _connectionDialog;
    private TrainingDialog _trainingDialog;
    private TrainingCompletedDialog _trainingCompletedDialog;
    private States _currentState;
    private ERPPipeline.Mode _currentMode;
    private bool _connectionStateChanged;
    private bool _modeChanged;
    private bool _classifierCalculated;
    private bool _calculatingClassifier;
    private bool _classifierCalculationFailed;

    void Start()
    {
        //get bci dialogs
        Canvas[] dialogs = gameObject.GetComponentsInChildren<Canvas>();
        foreach (Canvas dialog in dialogs)
        {
            if (dialog.name.Equals("dlgConnection"))
                _cvConnectionDialog = dialog;
            else if (dialog.name.Equals("dlgTraining"))
                _cvTraining = dialog;
            else if (dialog.name.Equals("dlgTrainingCompleted"))
                _cvTrainingCompletedDialog = dialog;
        }

        //connection dialog
        _connectionDialog = gameObject.GetComponentInChildren<ConnectionDialog>();
        _connectionDialog.BtnConnect_Click += OnBtnConnectClick;
        _connectionStateChanged = false;
        _modeChanged = false;

        //training dialog
        _trainingDialog = gameObject.GetComponentInChildren<TrainingDialog>();
        _trainingDialog.BtnStartStopFlashing_Click += OnBtnStartStopFlashing_Click;

        //training completed dialog
        _trainingCompletedDialog = gameObject.GetComponentInChildren<TrainingCompletedDialog>();
        _trainingCompletedDialog.BtnContinueFlashing_Click += OnBtnContinue_Click;
        _trainingCompletedDialog.BtnRetrain_Click += OnBtnRetrain_Click;

        //set dialog visibility
        _cvConnectionDialog.gameObject.SetActive(true);
        _cvTraining.gameObject.SetActive(false);
        _cvTrainingCompletedDialog.gameObject.SetActive(false);

        //flash controller
        _flashController = gameObject.GetComponent<ERPFlashController3D>();
        _flashController.FlashingStarted += OnFlashingStarted;
        _flashController.FlashingStopped += OnFlashingStopped;
        _flashController.Trigger += OnTrigger;

        BCIManager.Instance.Initialize(_flashController.NumberOfClasses);

        //bci manager
        _currentMode = ERPPipeline.Mode.Training;
        ERPBCIManager.Instance.RuntimeExceptionOccured += OnRuntimeExceptionOccured;
        ERPBCIManager.Instance.ModeChanged += OnModeChanged;
        ERPBCIManager.Instance.ClassifierCalculated += OnClassifierAvailable;
        ERPBCIManager.Instance.ClassifierCalculationFailed += OnClassifierCalculationFailed;
        ERPBCIManager.Instance.StateChanged += OnBCIStateChanged;
        ERPBCIManager.Instance.NumberOfAverages = _flashController.ScoreAverages;

        _classifierCalculated = false;
        _calculatingClassifier = false;
    }

    private void OnTrigger(object sender, EventArgs e)
    {
        ERPTriggerEventArgs ea = (ERPTriggerEventArgs)e;
        if (ERPBCIManager.Instance.Initialized)
            ERPBCIManager.Instance.SetTrigger(ea.IsTarget, ea.Id, ea.Trial, ea.IsLastOfTrial);
    }

    private void OnClassifierAvailable(object sender, EventArgs e)
    {
        _calculatingClassifier = false;
        _classifierCalculated = true;
        Dictionary<int, Accuracy> accuracy = ERPBCIManager.Instance.Accuracy();

        string classifierAccuracy = string.Empty;
        foreach (KeyValuePair<int, Accuracy> kvp in accuracy)
            classifierAccuracy += string.Format("Averages: {0}, Accuracy {1}\n", kvp.Key, kvp.Value.Mean);
        UnityEngine.Debug.Log(string.Format("Classifier calculated.\n{0}", classifierAccuracy));
    }

    private void OnClassifierCalculationFailed(object sender, EventArgs e)
    {
        _calculatingClassifier = false;
        _classifierCalculated = false;
        _classifierCalculationFailed = true;

        Debug.Log("Could not calculate classifier.");
    }

    private void OnFlashingStarted(object sender, EventArgs e)
    {
        Debug.Log("Flashing started");
    }

    private void OnFlashingStopped(object sender, EventArgs e)
    {
        Debug.Log("Flashing stopped");
        if (_currentMode == ERPPipeline.Mode.Training)
        {
            _calculatingClassifier = true;
            _classifierCalculated = false;
            ERPBCIManager.Instance.Train();
        }
    }

    private void OnModeChanged(object sender, EventArgs e)
    {
        ModeChangedEventArgs ea = (ModeChangedEventArgs)e;
        _currentMode = ea.Mode;
        _modeChanged = true;
        Debug.Log(String.Format("Mode Changed to '{0}'", _currentMode.ToString()));
    }

    private void OnBCIStateChanged(object sender, EventArgs e)
    {
        StateChangedEventArgs ea = (StateChangedEventArgs)e;
        _currentState = ea.State;
        _connectionStateChanged = true;
        Debug.Log(String.Format("Device state changed to '{0}'", ea.State));
    }

    private void OnRuntimeExceptionOccured(object sender, EventArgs e)
    {
        RuntimeExceptionEventArgs ea = (RuntimeExceptionEventArgs)e;
        Debug.Log(String.Format("A runtime exception occured.\n{0}\n{1}", ea.Exception.Message, ea.Exception.StackTrace));
    }

    private void OnBtnConnectClick(object sender, EventArgs e)
    {
        new Thread(() =>
        {
            try
            {
                _currentState = States.Connecting;
                _connectionStateChanged = true;
                ERPBCIManager.Instance.Initialize(_connectionDialog.Serial);
                _currentState = States.Connected;
                _connectionStateChanged = true;
            }
            catch (Exception ex)
            {
                try
                {
                    ERPBCIManager.Instance.Uninitialize();
                }
                catch
                {
                    //DO NOTHING 
                }

                _currentState = States.Disconnected;
                _connectionStateChanged = true;
                Debug.Log(String.Format("Device initialization failed.\n{0}\n{1}", ex.Message, ex.StackTrace));
            }
        }).Start();
    }

    private void OnBtnStartStopFlashing_Click(object sender, EventArgs e)
    {
        if (_trainingDialog.IsFlashing)
        {
            _flashController.StartFlashing(ERPFlashController3D.Mode.Training);
        }
        else
        {
            _flashController.StopFlashing();
        }
    }

    private void OnBtnRetrain_Click(object sender, EventArgs e)
    {
        ERPBCIManager.Instance.Configure(ERPPipeline.Mode.Training);
        _trainingDialog.Reset();
    }

    private void OnBtnContinue_Click(object sender, EventArgs e)
    {
        ERPBCIManager.Instance.Configure(ERPPipeline.Mode.Application);
        _flashController.StartFlashing(ERPFlashController3D.Mode.Application);
    }

    void Update()
    {
        //show/hide connection dialog
        if (_connectionStateChanged || _modeChanged)
        {
            if (_currentState == States.Disconnected)
            {
                _cvConnectionDialog.gameObject.SetActive(true);
                _cvTraining.gameObject.SetActive(false);
                _cvTrainingCompletedDialog.gameObject.SetActive(false);
            }
            else if (_currentState == States.Connecting)
            {
                _cvConnectionDialog.gameObject.SetActive(false);
                _cvTraining.gameObject.SetActive(false);
                _cvTrainingCompletedDialog.gameObject.SetActive(false);
            }
            else
            {
                if (_currentMode == ERPPipeline.Mode.Training)
                {
                    _cvConnectionDialog.gameObject.SetActive(false);
                    _cvTraining.gameObject.SetActive(true);
                    _cvTrainingCompletedDialog.gameObject.SetActive(false);
                }
                else
                {
                    _cvConnectionDialog.gameObject.SetActive(false);
                    _cvTraining.gameObject.SetActive(false);
                    _cvTrainingCompletedDialog.gameObject.SetActive(false);
                }
            }
            _connectionStateChanged = false;
            _modeChanged = false;
        }

        if (_calculatingClassifier)
        {
            _cvConnectionDialog.gameObject.SetActive(false);
            _cvTraining.gameObject.SetActive(false);
            _cvTrainingCompletedDialog.gameObject.SetActive(false);
            _calculatingClassifier = false;
        }

        if (_classifierCalculated)
        {
            _cvConnectionDialog.gameObject.SetActive(false);
            _cvTraining.gameObject.SetActive(true);
            _cvTrainingCompletedDialog.gameObject.SetActive(true);
            _classifierCalculated = false;
        }

        if (_classifierCalculationFailed)
        {
            ERPBCIManager.Instance.Configure(ERPPipeline.Mode.Training);
            _trainingDialog.Reset();
            _classifierCalculationFailed = false;
        }
    }

    private void OnApplicationQuit()
    {
        _connectionDialog.BtnConnect_Click -= OnBtnConnectClick;

        _trainingDialog.BtnStartStopFlashing_Click -= OnBtnStartStopFlashing_Click;

        _trainingCompletedDialog.BtnContinueFlashing_Click -= OnBtnContinue_Click;
        _trainingCompletedDialog.BtnRetrain_Click -= OnBtnRetrain_Click;

        _flashController.FlashingStarted -= OnFlashingStarted;
        _flashController.FlashingStopped -= OnFlashingStopped;
        _flashController.Trigger -= OnTrigger;

        BCIManager.Instance.Uninitialize();

        ERPBCIManager.Instance.RuntimeExceptionOccured -= OnRuntimeExceptionOccured;
        ERPBCIManager.Instance.ModeChanged -= OnModeChanged;
        ERPBCIManager.Instance.ClassifierCalculated -= OnClassifierAvailable;
        ERPBCIManager.Instance.ClassifierCalculationFailed -= OnClassifierCalculationFailed;
        ERPBCIManager.Instance.StateChanged -= OnBCIStateChanged;

        ERPBCIManager.Instance.Uninitialize();
    }
}