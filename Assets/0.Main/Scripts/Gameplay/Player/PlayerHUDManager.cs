using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] private HealthController _playerHealth;
    [SerializeField] private StatsManager _playerStats;

    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _healthText;
    
    [SerializeField] private Slider _expSlider;
    [SerializeField] private TextMeshProUGUI _levelText;

    private void OnEnable()
    {
        _playerHealth.OnHealthChanged += UpdateHealthUI;
        _playerStats.OnLevelUp += UpdateLevelUI;
        _playerStats.OnExpChanged += UpdateExpUI;
    }

    private void OnDisable()
    {
        _playerHealth.OnHealthChanged -= UpdateHealthUI;
        _playerStats.OnLevelUp -= UpdateLevelUI;
        _playerStats.OnExpChanged -= UpdateExpUI;
    }

    private void Start()
    {
        UpdateLevelUI(_playerStats.CurrentLevel);
        UpdateExpUI(_playerStats.CurrentExp, _playerStats.CurrentStats.ExpPerLevel); 
    }

    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {   
        float displayHealth = Mathf.Max(0, currentHealth);
        float displayMax = Mathf.Max(1, maxHealth);

        _healthSlider.maxValue = displayMax;
        _healthSlider.value = displayHealth;
        _healthText.text = $"{Mathf.CeilToInt(displayHealth)} / {Mathf.CeilToInt(displayMax)}";
    }

    private void UpdateLevelUI(int newLevel)
    {
        if (_levelText != null)
        {
            _levelText.text = $"Lv.{newLevel}";
        }
    }

    private void UpdateExpUI(float currentExp, float expNeeded)
    {
        if (_expSlider != null)
        {
            _expSlider.maxValue = expNeeded;
            _expSlider.value = Mathf.Max(0, currentExp);
        }
    }
}