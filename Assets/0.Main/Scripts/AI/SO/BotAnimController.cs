using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class BotAnimController : MonoBehaviour
{
    public Animator botAnimator;
    
    private void Awake()
    {
        if(botAnimator == null) botAnimator = GetComponentInChildren<Animator>();
    }

    public void ResetAnimations()
    {
        if (botAnimator == null) return;
        botAnimator.Rebind();
        botAnimator.Update(0f);
    }

    public void UpdateRunInput(bool moving)
    {
        botAnimator.SetBool("Moving", moving);
    }
    
    public void GuardAnim(bool guard)
    {
        botAnimator.SetBool("Guard", guard);
    }
    
    [Button]
    public async UniTaskVoid DamagedAnim()
    {
        botAnimator.Play("Damaged");
    }
    
    [Button("Attack")]
    public void AttackAnim(bool Attacking)
    {
        if(Attacking) botAnimator.SetTrigger("AttackStart");
        botAnimator.SetBool("Attacking", Attacking);
    }
    
    [Button("Special Attack")]
    public void SpecialAttackAnim(bool Attacking, int SpecialType = 0)
    {
        botAnimator.SetInteger("AttackType", SpecialType);
        
        if(Attacking) botAnimator.SetTrigger("SpecialAttackStart");
        botAnimator.SetBool("Attacking", Attacking);
    }
    
    public async UniTaskVoid DebuffStun(float time)
    {
        botAnimator.SetBool("Stun", true);
        botAnimator.SetTrigger("StunStart");
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        botAnimator.SetBool("Stun", false);
    }

    public void Die()
    {
        botAnimator.SetTrigger("Die");
    }
}