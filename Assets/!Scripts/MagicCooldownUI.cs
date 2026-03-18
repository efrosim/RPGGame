using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MagicCooldownUI : MonoBehaviour
{
    private Image _cooldownImage;
    
    [SerializeField] private PlayerCombat _playerCombat; 

    private void Awake()
    {
        _cooldownImage = GetComponent<Image>();
        _cooldownImage.type = Image.Type.Filled;
    }

    private void OnEnable()
    {
        if (_playerCombat != null)
            _playerCombat.OnMagicCooldownChanged += UpdateCooldownUI;
    }

    private void OnDisable()
    {
        if (_playerCombat != null)
            _playerCombat.OnMagicCooldownChanged -= UpdateCooldownUI;
    }

    private void UpdateCooldownUI(float fillAmount)
    {
        _cooldownImage.fillAmount = fillAmount;
    }
}