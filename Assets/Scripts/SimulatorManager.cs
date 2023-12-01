using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RDx.Platform;


public class SimulatorManager : MonoBehaviour
{
    private const string DATASET_URL = "http://18.228.190.198:8000/api/SCerevGrowth/";

    public static Dataset developmentDataset { get; private set; }

    void Start()
    {
        RDxRequest.Get<Dataset>(DATASET_URL, (response) =>
        {
            developmentDataset = response;
            Debug.Log(response);
        });

    }

}



























// [SerializeField] private Button startButton;
// [SerializeField] private Button stopButton;
// [SerializeField] private Button resetButton;

// void OnEnable()
// {
//     startButton.onClick.AddListener(StartSimulation);
//     stopButton.onClick.AddListener(StopSimulation);
//     resetButton.onClick.AddListener(ResetSimulation);

// }
// void OnDisable()
// {
//     startButton.onClick.RemoveListener(StartSimulation);
//     stopButton.onClick.RemoveListener(StopSimulation);
//     resetButton.onClick.RemoveListener(ResetSimulation);
// }

// void StartSimulation()
// {
//     startButton.gameObject.SetActive(false);
//     stopButton.gameObject.SetActive(true);
//     resetButton.gameObject.SetActive(true);
//     Debug.Log("Start simulation");
// }
// void StopSimulation()
// {
//     startButton.gameObject.SetActive(true);
//     stopButton.gameObject.SetActive(false);
//     resetButton.gameObject.SetActive(true);
//     Debug.Log("Stop simulation");
// }
// void ResetSimulation()
// {
//     startButton.gameObject.SetActive(true);
//     stopButton.gameObject.SetActive(false);
//     resetButton.gameObject.SetActive(false);
//     Debug.Log("Reset simulation");
// }