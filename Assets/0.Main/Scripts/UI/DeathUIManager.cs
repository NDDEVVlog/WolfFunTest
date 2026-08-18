using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DeathUIManager : MonoBehaviour
{
    [SerializeField] private Button _retryButton;
    
    [SerializeField] private CanvasGroup _canvasGroup;

    private void OnEnable()
    {
        if (_retryButton != null)
        {
            _retryButton.onClick.AddListener(ReloadCurrentScene);
        }
    }

    private void OnDisable()
    {
        if (_retryButton != null)
        {
            _retryButton.onClick.RemoveListener(ReloadCurrentScene);
        }
    }

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}