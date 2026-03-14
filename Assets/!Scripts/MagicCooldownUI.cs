using UnityEngine;
using UnityEngine.UI;[RequireComponent(typeof(Image))]
public class MagicCooldownUI : MonoBehaviour
{
    private Image _cooldownImage;

    private void Awake()
    {
        _cooldownImage = GetComponent<Image>();
        _cooldownImage.type = Image.Type.Filled;
    }

    private void OnEnable()
    {
        if (PlayerController.Instance != null)
            PlayerController.Instance.OnMagicCooldownChanged += UpdateCooldownUI;
    }

    private void OnDisable()
    {
        if (PlayerController.Instance != null)
            PlayerController.Instance.OnMagicCooldownChanged -= UpdateCooldownUI;
    }

    private void UpdateCooldownUI(float fillAmount)
    {
        _cooldownImage.fillAmount = fillAmount;
    }
}