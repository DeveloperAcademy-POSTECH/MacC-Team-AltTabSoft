using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenSkillUI : MonoBehaviour
{
    [SerializeField] private MonsterData _monsterNormalShort;
    [SerializeField] private MonsterData _monsterNormalLong;
    [SerializeField] private MonsterData _monsterEliteShort;
    [SerializeField] private MonsterData _monsterEliteLong;
    [SerializeField] private MonsterData _monsterBoss;

    private void Start()
    {
        //아직
    }

    public IEnumerator activeHiddenSkill(GameObject hiddenUI, GameObject mainUI)
    {
        hiddenUI.SetActive(true);
        //mainUI.SetActive(false);
        GameManager.Inst.PauseGame();
        yield return new WaitForSeconds(2f);
        hiddenUI.SetActive(false);
        //mainUI.SetActive(true);
        GameManager.Inst.ResumeGame();

    }

    private void OnDisable()
    {
        
    }
}
