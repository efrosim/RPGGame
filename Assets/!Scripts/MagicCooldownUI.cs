using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MagicCooldownUI : MonoBehaviour
{
    private Image _cooldownImage;
    [SerializeField] private CooldownTimer _cooldownTimer; 

    private void Awake()
    {
        _cooldownImage = GetComponent<Image>();
        _cooldownImage.type = Image.Type.Filled;
    }

    private void OnEnable()
    {
        if (_cooldownTimer != null)
            _cooldownTimer.OnCooldownProgress += UpdateCooldownUI;
    }

    private void OnDisable()
    {
        if (_cooldownTimer != null)
            _cooldownTimer.OnCooldownProgress -= UpdateCooldownUI;
    }

    private void UpdateCooldownUI(float fillAmount)
    {
        _cooldownImage.fillAmount = fillAmount;
    }
}