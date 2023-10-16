using TMPro;
using UnityEngine;

public class ClearCollectedBoxCat: MonoBehaviour
{
    private TextMeshProUGUI _text;
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
         _text.text = string.Concat(GameManager.Inst.collectedCatBox.ToString(),  " 고양이 구출");
    }
}
