using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageUIScript : MonoBehaviour
{
    private Button _buttonGoToShelter;

    // Start is called before the first frame update
    void Start()
    {
        var buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            Debug.Log(button.transform.name);
            var buttonName = button.transform.name;
            if (buttonName == "Button_GoTo_Shelter")
            {
                _buttonGoToShelter = button;
            }
        }
        
        _buttonGoToShelter.onClick.AddListener(() =>
        {
            Debug.Log("ㅗ도도도");
            SceneManager.LoadScene("ShelterScene");
        });
    }
}
