using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using Sirenix.OdinInspector;
using System;

public class SkillUI_Button : OnScreenControl, IPointerDownHandler, IPointerUpHandler
{   
    [SerializeField] private bool UsingCrossPlatformInput;

    [FoldoutGroup("Skill Info")]
    [InputControl(layout = "Button")]
    public string _ControlPath;

    protected override string controlPathInternal { get => _ControlPath; set => _ControlPath = value; }

    public event Action OnSkillButtonPressed;
    public event Action OnSkillButtonReleased;


    public void OnPointerDown(PointerEventData eventData)
    {
       if (UsingCrossPlatformInput)
       {
            SendValueToControl(1.0f);
            return;
       }

        OnSkillButtonPressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (UsingCrossPlatformInput)
        {
            SendValueToControl(0.0f);
            return;
        }
        OnSkillButtonReleased?.Invoke();
    }   
}
