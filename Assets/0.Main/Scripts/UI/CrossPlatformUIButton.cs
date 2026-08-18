using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
/// <summary>
///  Inherited from OnScreenControl to create a custom button for cross-platform UI input handling.
/// </summary>
public class CrossPlatformUIButton : OnScreenControl, IPointerClickHandler
{
    [InputControl(layout = "Button")]
    public string _ControlPath;

    protected override string controlPathInternal { get => _ControlPath; set => _ControlPath = value; }

    public void OnPointerClick(PointerEventData eventData)
    {   
        //Press button
        SendValueToControl(1.0f);
    }
}
