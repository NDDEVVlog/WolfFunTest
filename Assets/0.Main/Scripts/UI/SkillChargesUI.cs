using UnityEngine;
using UnityEngine.UI;

public class SkillChargesUI : MonoBehaviour
{
    [SerializeField] private Image[] _chargeImages;
    [SerializeField] private Color _activeColor = Color.white;
    [SerializeField] private Color _inactiveColor = new Color(1f, 1f, 1f, 0.3f);

    private Skill_InfoSO _targetSkillInfo;

    public void Initialize(Skill_InfoSO skillInfo)
    {
        _targetSkillInfo = skillInfo;
        EventBus<SkillChargeChangedEvent>.OnEvent += HandleChargeEvent;
    }

    private void OnDestroy()
    {
        EventBus<SkillChargeChangedEvent>.OnEvent -= HandleChargeEvent;
    }

    private void HandleChargeEvent(SkillChargeChangedEvent evt)
    {
        if (evt.SkillInfo != _targetSkillInfo) return;

        for (int i = 0; i < _chargeImages.Length; i++)
        {
            if (_chargeImages[i] != null)
            {
                _chargeImages[i].color = i < evt.CurrentCharges ? _activeColor : _inactiveColor;
            }
        }
    }
}