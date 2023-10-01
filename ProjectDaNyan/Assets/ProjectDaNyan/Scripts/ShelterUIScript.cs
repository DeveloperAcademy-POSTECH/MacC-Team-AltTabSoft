using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShelterUIScript : MonoBehaviour
{
    private Button _buttonExit;
    private Button _buttonStage;

    // Start is called before the first frame update
    void Start()
    {
        var buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            Debug.Log(button.transform.name);
            var buttonName = button.transform.name; 
            if (buttonName == "Button_Exit")
            {
                _buttonExit = button;
            } else if (buttonName == "Button_Stage")
            {
                _buttonStage = button;
            }
        }
        
        _buttonStage.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("StageScene");
        });
        
        _buttonExit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
