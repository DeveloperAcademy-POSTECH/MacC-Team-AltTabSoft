using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenSkillUI : MonoBehaviour
{
    [SerializeField] private MonsterData _monsterNormalShort;
    [SerializeField] private MonsterData _monsterNormalLong;
    [SerializeField] private MonsterData _monsterEliteShort;
    [SerializeField] private MonsterData _monsterEliteLong;
    [SerializeField] private MonsterBossAData _monsterBossA;
    [SerializeField] private MonsterBossBData _monsterBossB;
    [SerializeField] private GameObject _playerAttackPosition;
    //[SerializeField] private EnemyScanner _enemyScanner;
    [SerializeField] private Monster _monster;
    [SerializeField] private float  _monsterHP;
    [SerializeField] private LayerMask layer;

    [SerializeField] private Collider[] _enemyColliders;

    private void Start()
    {
        //Debug.Log($"present HP: {_monsterBoss.HP}");
        _enemyColliders = Physics.OverlapSphere(_playerAttackPosition.transform.position, 100, layer);

    }

    public IEnumerator activeHiddenSkill(GameObject hiddenUI, GameObject mainUI)
    {
        hiddenUI.SetActive(true);
        //mainUI.SetActive(false);
        GameManager.Inst.PauseGame();
        yield return new WaitForSecondsRealtime(0.5f);
        hiddenUI.SetActive(false);
        //mainUI.SetActive(true);
        GameManager.Inst.ResumeGame();

    }

    private void OnDisable()
    {
        foreach (Collider enemyCollider in _enemyColliders)
        {
            _monster = enemyCollider.gameObject.GetComponent<Monster>();
            
            switch (_monster.myType)
            {
                case MonsterType.Normal:
                    enemyCollider.gameObject.GetComponent<MonsterNormal>().monsterHP -= _monsterNormalLong.hp;
                    break;
                case MonsterType.Elite:
                    enemyCollider.gameObject.GetComponent<MonsterNormal>().monsterHP -= _monsterEliteLong.hp * 0.5f;
                    break;
                case MonsterType.Boss:
                    if(enemyCollider.gameObject.TryGetComponent(out MonsterBossA monsterBossA))
                    {
                        monsterBossA.monsterHP -= _monsterBossA.HP * 0.2f;
                    }
                    else
                    {
                        enemyCollider.gameObject.GetComponent<MonsterBossB>().monsterHP -= _monsterBossB.HP * 0.2f;
                    }
                    break;
            }
        }
    }
}

