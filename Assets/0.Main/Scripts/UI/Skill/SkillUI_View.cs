using UnityEngine;
using UnityEngine.UI;

public class SkillUI_View : MonoBehaviour
{   
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image mainImage;
    public void InitImage(Sprite image)
    {
        backgroundImage.sprite = image;
        mainImage.sprite = image;
    }
    public void UpdateCooldownFill(float progress)
    {
        if (mainImage != null)
        {
            mainImage.fillAmount = 1- progress;
        }
    }
}