using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MagicCooldownUI : MonoBehaviour
{
    private Image _cooldownImage;
    private CooldownTimer _cooldownTimer; 

    private void Awake()
    {
        _cooldownImage = GetComponent<Image>();
        _cooldownImage.type = Image.Type.Filled;
    }

    // Метод для передачи зависимости
    public void Init(CooldownTimer timer)
    {
        _cooldownTimer = timer;
        _cooldownTimer.OnCooldownProgress += UpdateCooldownUI;
    }

    private void OnDestroy()
    {
        if (_cooldownTimer != null)
            _cooldownTimer.OnCooldownProgress -= UpdateCooldownUI;
    }

    private void UpdateCooldownUI(float fillAmount)
    {
        _cooldownImage.fillAmount = fillAmount;
    }
}