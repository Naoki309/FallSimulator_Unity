using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class SpeedManager : MonoBehaviour
{
    public TMP_Dropdown moonAgeDropdown;
    private Dictionary<int, float> moonAgeToSpeed = new Dictionary<int, float>
    {
        // 実験により値の変更あり
        {3, 0.55f},  {6, 0.92f},  {9, 1.4f},  {12, 1.6f}, 
        {14, 1.8f}, {16, 2.0f}, {18, 0.8f}, {20, 2.4f}, {22, 2.6f}, {24, 2.8f},
        {26, 3.0f}, {28, 3.2f}, {30, 3.4f}, {32, 3.6f}, {34, 3.8f}, {36, 10.0f}
    };

    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        moonAgeDropdown.ClearOptions();
        List<string> moonAges = new List<string>();
        foreach (var moonAge in moonAgeToSpeed.Keys)
        {
            moonAges.Add(moonAge.ToString());
        }
        moonAgeDropdown.AddOptions(moonAges);
        moonAgeDropdown.onValueChanged.AddListener(OnMoonAgeChanged);

        // デフォルト値で速度を設定
        SetDefaultSpeed();
    }

    private void SetDefaultSpeed()
    {
        int defaultMoonAge = 3; // または他のデフォルト値
        if (moonAgeToSpeed.TryGetValue(defaultMoonAge, out var defaultSpeed))
        {
            playerMovement.SetSpeed(defaultSpeed);
        }
    }


    private void OnMoonAgeChanged(int moonAgeIndex)
    {
        int selectedMoonAge = int.Parse(moonAgeDropdown.options[moonAgeIndex].text);
        if (moonAgeToSpeed.TryGetValue(selectedMoonAge, out var speed))
        {
            playerMovement.SetSpeed(speed);
        }
    }
}