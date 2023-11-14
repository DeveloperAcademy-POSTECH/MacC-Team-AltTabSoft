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

    private float _hiddenSkillRate = 3f;
    private float _hiddenSkillDelay = 10f;
    private bool _isHiddenReady;
    public int _hiddenSkillCount = 0;

    private void OnEnable()
    {
        //Debug.Log($"present HP: {_monsterBoss.HP}");
        _playerAttackPosition = GameObject.Find("PlayerAttackPosition");
        _enemyColliders = Physics.OverlapSphere(_playerAttackPosition.transform.position, 100, layer);

    }

    //public void UseHiddenSkill(GameObject hiddenUI)
    //{
    //    _isHiddenReady = _hiddenSkillRate < _hiddenSkillDelay;
    //    _hiddenSkillDelay += Time.deltaTime;
    //    if (_isHiddenReady)
    //    {
    //        StartCoroutine(activeHiddenSkill(hiddenUI));
    //        _hiddenSkillDelay = 0;
    //    }

    //}

    public IEnumerator activeHiddenSkill(GameObject hiddenUI)
    {
        //Debug.Log($"hidden SKill Count :{_hiddenSkillCount}");
        hiddenUI.SetActive(true);
        //mainUI.SetActive(false);
        if (Time.timeScale != 0)
            GameManager.Inst.PauseGame();
        yield return new WaitForSecondsRealtime(0.5f);
        hiddenUI.SetActive(false);
        //mainUI.SetActive(true);
        if (Time.timeScale == 0)
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
                    Debug.Log($"{enemyCollider.gameObject.GetComponent<MonsterNormal>().monsterHP}");
                    break;
                case MonsterType.Elite:
                    enemyCollider.gameObject.GetComponent<MonsterNormal>().monsterHP -= _monsterEliteLong.hp;
                    Debug.Log($"{enemyCollider.gameObject.GetComponent<MonsterNormal>().monsterHP}");
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

