using UnityEngine;

public class FeedbackCanvasController : MonoBehaviour
{
    Animator _spriteFeedbackCanvasAnimator;
    private int _currentFireAnimIndex = 1;
    private int _currentWaterAnimIndex = 1;

    private void Awake()
    {
        _spriteFeedbackCanvasAnimator = GetComponent<Animator>();

        EventManager.AddListener("TriggerSymbol", OnTriggerSymbol);
        EventManager.AddListener("TriggerShield", OnTriggerShield);
    }

    private void OnTriggerSymbol(object data)
    {
        string element = (string)data;
        if (element == "Fire")
        {
            _currentFireAnimIndex++;
            if (_currentFireAnimIndex > 3) _currentFireAnimIndex = 1;
            _spriteFeedbackCanvasAnimator.SetTrigger("FireSymbol_" + _currentFireAnimIndex);
        }
        else if (element == "Water")
        {
            _currentWaterAnimIndex++;
            if (_currentWaterAnimIndex > 2) _currentWaterAnimIndex = 1;
            _spriteFeedbackCanvasAnimator.SetTrigger("WaterSymbol_" + _currentWaterAnimIndex);
        }
    }

    private void OnTriggerShield(object data) {
        string part = data as string;

        if (part == "Fill") {
            _spriteFeedbackCanvasAnimator.SetTrigger("FillShieldCooldown");
        } else {
            _spriteFeedbackCanvasAnimator.SetTrigger("DeplateShield");
        }
    }
}