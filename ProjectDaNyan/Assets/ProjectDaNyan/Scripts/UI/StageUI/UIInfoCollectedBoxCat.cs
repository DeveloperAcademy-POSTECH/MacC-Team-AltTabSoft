using TMPro;
using UnityEngine;

public class UIInfoCollectedBoxCat: MonoBehaviour
{
    private TextMeshProUGUI _text;
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        _text.text = GameManager.Instance.collectedCatBox.ToString();
    }
}
