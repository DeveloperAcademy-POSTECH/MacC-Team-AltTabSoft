using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace deko
{

public class ChangeCharacter : MonoBehaviour
{
    public GameObject[] mCharacters;

    int current = 0;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.gameObject.GetComponent<Button>();
		btn.onClick.AddListener(OnClickChangeCharacter);

        for (int i = 0; i < mCharacters.Length; i++) mCharacters[i].SetActive(false);
        mCharacters[current].SetActive(true);
    }

    public void OnClickChangeCharacter()
    {
        current = (current + 1) % mCharacters.Length;

        for (int i = 0; i < mCharacters.Length; i++) mCharacters[i].SetActive(false);
        mCharacters[current].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

}