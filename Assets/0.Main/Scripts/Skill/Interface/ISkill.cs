using System;
using UnityEngine;

public interface ISkill
{  

   public event Action<float> OnCooldownProgressChanged;
   void Initialize(GameObject caster,Skill_InfoSO skill_InfoSO,StatsManager characterStats);
   void Execute();
   void EndExecute();
   void UpdateSkill(float deltaTime);
   
}
