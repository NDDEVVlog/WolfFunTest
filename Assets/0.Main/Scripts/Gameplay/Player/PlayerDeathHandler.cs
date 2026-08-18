using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private HealthController _playerHealth;
    [SerializeField] private DeathUIManager _deathUI;

    private void OnEnable()
    {   
        _deathUI.Hide();
        if (_playerHealth != null)
        {
            _playerHealth.OnDeath += HandlePlayerDeath;
        }
    }

    private void OnDisable()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnDeath -= HandlePlayerDeath;
        }
    }

    private void HandlePlayerDeath()
    {
        if (_deathUI != null)
        {
            _deathUI.Show();
        }
    }
}