using System;
using Cysharp.Threading.Tasks;

using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;

    public bool IsDrawed { get; private set; }

    private static readonly int MovingHash = Animator.StringToHash("Moving");
    private static readonly int GuardHash = Animator.StringToHash("Guard");
    private static readonly int DodgeHash = Animator.StringToHash("Dodge");
    private static readonly int DieHash = Animator.StringToHash("Die");
    private static readonly int DieTriggerHash = Animator.StringToHash("DieTrigger");
    private static readonly int DamagedHash = Animator.StringToHash("Damaged");
    private static readonly int RecallingHash = Animator.StringToHash("Recalling");
    private static readonly int ReverseRecallingHash = Animator.StringToHash("ReverseRecalling");
    private static readonly int RecallingStartHash = Animator.StringToHash("RecallingStart");
    private static readonly int StunHash = Animator.StringToHash("Stun");

    private void Awake()
    {
        if (playerAnimator == null) 
            playerAnimator = GetComponentInChildren<Animator>();
    }

    public void HandleStateChanged(PlayerState state)
    {
        playerAnimator.SetBool(MovingHash, state == PlayerState.Running);
    }

    public void GuardAnim(bool guard) => playerAnimator.SetBool(GuardHash, guard);
    
    public void DodgeAnim() => playerAnimator.SetTrigger(DodgeHash);


    public void DeathEventCall()
    {
        DieAnim(true);
    }
    public void DieAnim(bool die)
    {
        playerAnimator.SetBool(DieHash, die);
        if (!die) return;
        
        playerAnimator.SetTrigger(DieTriggerHash);
    }

    [Button]
    public void DamagedAnim() => playerAnimator.SetTrigger(DamagedHash);

    public void RecallAnim(bool recalling, bool reverseRecalling = false)
    {
        playerAnimator.SetBool(RecallingHash, recalling);
        playerAnimator.SetBool(ReverseRecallingHash, reverseRecalling);
    }

    public void RecallAnimTrigger() => playerAnimator.SetTrigger(RecallingStartHash);

    public async UniTaskVoid DebuffStun(float time)
    {
        playerAnimator.SetBool(StunHash, true);
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        if (this == null) return; 
        playerAnimator.SetBool(StunHash, false);
    }


}