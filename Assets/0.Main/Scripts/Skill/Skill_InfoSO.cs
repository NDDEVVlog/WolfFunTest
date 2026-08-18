using UnityEngine;

[CreateAssetMenu(fileName = "Skill_Info", menuName = "ScriptableObjects/Skill_Info", order = 1)]
public class Skill_InfoSO : ScriptableObject
{
    public string SkillName;
    public Sprite SkillIcon;



    [Header("Skill Properties")]
    public float Cooldown;
    public float BaseDamage;
    public float Range;

}
