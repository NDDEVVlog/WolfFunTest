using UnityEngine;
using UnityEngine.UI;

public class EnemyHitboxVisualizer : MonoBehaviour
{
    [SerializeField] private AI_ScriptManager _aiManager;
    [SerializeField] private Image _rangeIndicator;

    private void Start()
    {
        SetupIndicator();
        // HideIndicator();
    }

    private void SetupIndicator()
    {
        if (_rangeIndicator == null || _aiManager == null) return;

        _rangeIndicator.type = Image.Type.Filled;
        _rangeIndicator.fillMethod = Image.FillMethod.Radial360;
        _rangeIndicator.fillOrigin = (int)Image.Origin360.Top;

        float range = _aiManager.Stats.TotalMeleeRange;
        _rangeIndicator.rectTransform.sizeDelta = new Vector2(range * 2f, range * 2f);

        float angle = _aiManager.Stats.BasicAttackSpreadAngle;
        _rangeIndicator.fillAmount = angle / 360f;

        _rangeIndicator.rectTransform.localEulerAngles = new Vector3(90f, 0f, angle / 2f);
    }

    public void ShowIndicator()
    {
        if (_rangeIndicator != null) _rangeIndicator.enabled = true;
    }

    public void HideIndicator()
    {
        if (_rangeIndicator != null) _rangeIndicator.enabled = false;
    }
}