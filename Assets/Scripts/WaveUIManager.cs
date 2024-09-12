using UnityEngine;
using TMPro;
using Behaviours; 
public class WaveUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveInfoText; 
    private WaveController _waveController;

    private void Start()
    {
        _waveController = FindObjectOfType<WaveController>();
        if (_waveController != null)
        {
            _waveController.OnLastWaveEnded += HandleLastWaveEnded;
            UpdateWaveInfo();
        }
    }
    private void Update()
    {
        UpdateWaveInfo();
    }
    private void UpdateWaveInfo()
    {
        if (_waveController != null)
        {
            var data = _waveController.Data;
            waveInfoText.text = $"Wave completed: {data.Wave + 1} / {_waveController.WaveCount}\n";
            //$"Pack: {data.Pack + 1}\n" +
            // $"Unit: {data.Unit}";
        }
    }
    private void HandleLastWaveEnded()
    {
        waveInfoText.text = "You completed all waves!";
    }
}