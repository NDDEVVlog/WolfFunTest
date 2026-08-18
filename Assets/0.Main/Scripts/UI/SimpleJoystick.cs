using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class SimpleJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Title("References")]
    [SerializeField, Required] private RectTransform joystickBase;
    [SerializeField, Required] private RectTransform knob;

    [Title("Settings")]
    public float radius = 100f;
    [SerializeField] private float gravity = 25f;
    [SerializeField] private bool useSquareBoundary = true;

    [Title("Events")]
    public UnityEvent OnPointerDownEvent;
    public UnityEvent OnPointerUpEvent;

    [Title("Debug Values")]
    [ReadOnly, ShowInInspector] private Vector2 inputVector;
    private Vector2 dynamicCenter; 
    private bool isReturning = false;
    private bool isInteracting = false;

    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;

    private void Awake()
    {
        knob.pivot = new Vector2(0.5f, 0.5f);
        knob.anchorMin = new Vector2(0.5f, 0.5f);
        knob.anchorMax = new Vector2(0.5f, 0.5f);
    }

    // --- RECREATED ULTIMATE JOYSTICK METHODS ---
    public float GetDistance()
    {
        // Distance between knob and the dynamic center, normalized by radius
        return Vector2.Distance(knob.anchoredPosition, dynamicCenter) / radius;
    }

    public Vector2 GetDirection()
    {
        return inputVector.normalized;
    }

    public bool GetJoystickState() => isInteracting;

    private void Update()
    {
        if (isReturning)
        {
            knob.anchoredPosition = Vector2.Lerp(knob.anchoredPosition, dynamicCenter, Time.deltaTime * gravity);
            
            Vector2 delta = knob.anchoredPosition - dynamicCenter;
            inputVector = delta / radius;

            if (delta.sqrMagnitude < 0.01f)
            {
                knob.anchoredPosition = dynamicCenter;
                inputVector = Vector2.zero;
                isReturning = false;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isInteracting = true;
        isReturning = false;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBase, 
            eventData.position, 
            eventData.pressEventCamera, 
            out Vector2 localPoint);

        dynamicCenter = localPoint;
        knob.anchoredPosition = localPoint;
        inputVector = Vector2.zero;

        OnPointerDownEvent?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBase, 
            eventData.position, 
            eventData.pressEventCamera, 
            out Vector2 currentLocalPos);

        Vector2 delta = currentLocalPos - dynamicCenter;

        if (useSquareBoundary)
        {
            delta.x = Mathf.Clamp(delta.x, -radius, radius);
            delta.y = Mathf.Clamp(delta.y, -radius, radius);
        }
        else
        {
            delta = Vector2.ClampMagnitude(delta, radius);
        }

        knob.anchoredPosition = dynamicCenter + delta;
        inputVector = delta / radius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isInteracting = false;
        isReturning = true;
        OnPointerUpEvent?.Invoke();
    }
}