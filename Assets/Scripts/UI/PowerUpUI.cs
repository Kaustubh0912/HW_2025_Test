using UnityEngine;
using TMPro;
using System.Text;
using System.Collections.Generic;

public class PowerUpUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI powerUpText;

    private void Update()
    {
        if (PowerUpManager.Instance == null || powerUpText == null) return;

        Dictionary<PowerUpType, float> activePowerUps = PowerUpManager.Instance.GetActivePowerUps();
        
        if (activePowerUps.Count == 0)
        {
            powerUpText.text = "";
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in activePowerUps)
            {
                sb.AppendLine($"{kvp.Key}: {kvp.Value:F1}s");
            }
            powerUpText.text = sb.ToString();
        }
    }
}
