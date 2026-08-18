using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class SkillSlot
{
    [SerializeReference] public ISkill Skill;
    public Skill_InfoSO SkillInfo;
    public SkillUI_Button SkillButton;
    public SkillUI_View SkillView;

    public void BindAndInitialize(GameObject caster,StatsManager characterStats)
    {
        if (Skill == null) return;

        Skill.Initialize(caster, SkillInfo,characterStats);

        if (SkillView != null)
        {
            Skill.OnCooldownProgressChanged += SkillView.UpdateCooldownFill;
            SkillView.UpdateCooldownFill(0f);
            SkillView.InitImage(SkillInfo.SkillIcon);
        }

        if (SkillButton != null)
        {
            SkillButton.OnSkillButtonPressed += Skill.Execute;
            SkillButton.OnSkillButtonReleased += Skill.EndExecute;
        }
    }

    public void ProcessUpdate(float deltaTime)
    {
        Skill?.UpdateSkill(deltaTime);
    }

    public void Unbind()
    {
        if (Skill == null) return;

        if (SkillView != null)
        {
            Skill.OnCooldownProgressChanged -= SkillView.UpdateCooldownFill;
            
        }

        if (SkillButton != null)
        {
            SkillButton.OnSkillButtonPressed -= Skill.Execute;
            SkillButton.OnSkillButtonReleased -= Skill.EndExecute;
        }
    }
}

public class SkillHolderManager : MonoBehaviour
{   

    public List<SkillSlot> skillSlots = new List<SkillSlot>();

    public void Init(StatsManager characterStats)
    {
        foreach (SkillSlot slot in skillSlots)
        {
            slot.BindAndInitialize(gameObject,characterStats);
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        
        foreach (SkillSlot slot in skillSlots)
        {
            slot.ProcessUpdate(deltaTime);
        }
    }

    private void OnDestroy()
    {
        foreach (SkillSlot slot in skillSlots)
        {
            slot.Unbind();
        }
    }
}