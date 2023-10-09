using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{   
    // 타일을 담을 배열 
    // [0][4][8][12]
    // [1][5][9][13]
    // [2][6][10][14]
    // [3][7][11][15]
    [SerializeField] public GameObject[] landArray;
    [SerializeField] public GameObject player;
    // 상자 고양이; 상자 고양이(5마리)는 초기에 맵 생성 시 랜덤하게 스폰
    [SerializeField] private GameObject boxCatPrefab;
    GameObject boxCat;
    // 수집한 상자 고양이 수 -> 다른 파일로 이동 고민 중
    public int collectedCats = 0;
    // 타일 하나의 크기
    [SerializeField] public float UnitSize;
    // 시야; 시야 밖에 타일이 없으면 타일을 갱신
    public float halfSight;
    // 전체 타일 크기; 순서대로 왼쪽-위 좌표, 오른쪽-아래 좌표를 담고 있음
    Vector3[] border;

    void Start()
    {
        boxCat = Instantiate(boxCatPrefab);

        // border 초기화, 플레이어의 시작 위치는 (0, 1, 0)이므로 왼쪽 끝 좌표는 -UnitSize * 1.5f, 오른쪽 끝 좌표는 UnitSize * 1.5f
        // 둘을 합하면 UnitSize * 3이며 이는 전체 타일의 크기와 같음
        border = new Vector3[]
        {
            new Vector3(-UnitSize * 1.5f, 0, UnitSize * 1.5f),
            new Vector3(UnitSize * 1.5f, 0,  -UnitSize * 1.5f),
        };

        SpawnItemOntoMap();
    }

    // Update is called once per frame
    void Update()
    {
        // 키 입력이 없으면 업데이트 하지 않음
        if (!Input.anyKey)
            return;

        // 시야 안에 타일이 없는 경우 체크
        BoundaryCheck();
    }

    void SpawnItemOntoMap()
    {
        for (int i = 0; i < 16; i++)
        {
            SpawnItem(i);
        }
    }

    void SpawnItem(int index)
    {
        for (int j = 0; j < 5; j++)
        {
            // 랜덤하게 위치값 생성
            float randomX = Random.Range(landArray[index].transform.position.x - halfSight, landArray[index].transform.position.x + halfSight);
            float randomZ = Random.Range(landArray[index].transform.position.z - halfSight, landArray[index].transform.position.z + halfSight); 
        
            Vector3 randomPosition = new Vector3(randomX, 1, randomZ);

            // 선택한 위치에 상자 고양이 생성
            GameObject _boxCat = Instantiate(boxCat, randomPosition, Quaternion.identity);
            // 상자 고양이를 자식으로 설정하여 타일과 같이 이동하도록 함
            _boxCat.transform.parent = landArray[index].transform;
        }
    }

    void RemoveItem(int index)
    {
        foreach (Transform child in landArray[index].transform)
        {
            if (child.CompareTag("boxCat"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    void BoundaryCheck()
    {
        // 오른쪽에 타일이 없을 경우
        if (border[1].x < player.transform.position.x + halfSight)
        {
            // 타일 영역을 타일 하나 사이즈만큼 오른쪽으로 이동 
            border[0] += Vector3.right * UnitSize;
            border[1] += Vector3.right * UnitSize;

            // 타일을 실제로 오른쪽으로 움직임
            MoveWorld(0);
        }

        // 왼쪽에 타일이 없을 경우
        else if (border[0].x > player.transform.position.x - halfSight)
        {
            border[0] -= Vector3.right * UnitSize;
            border[1] -= Vector3.right * UnitSize;

            MoveWorld(2);
        }

        // 위쪽에 타일이 없을 경우
        else if (border[0].z < player.transform.position.z + halfSight)
        {
            border[0] += Vector3.forward * UnitSize;
            border[1] += Vector3.forward * UnitSize;

            MoveWorld(1);
        }

        // 아래쪽에 타일이 없을 경우
        else if (border[1].z > player.transform.position.z- halfSight)
        {
            border[0] -= Vector3.forward * UnitSize;
            border[1] -= Vector3.forward * UnitSize;

            MoveWorld(3);
        }
    }

    void MoveWorld(int dir)
    {
        // 기존 배열 복사
        // landArray는 새로운 배열, _landArray는 기존 배열을 뜻함
        GameObject[] _landArray = new GameObject[16];
        System.Array.Copy(landArray, _landArray, 16);

        switch (dir)
        {
            case 0:
                for(int i = 0; i < 16; i++)
                {
                    int revise = i - 4;

                    if (revise < 0)
                    {
                        RemoveItem(i);
                        SpawnItem(i);
                        
                        landArray[16 + revise] = _landArray[i];
                        _landArray[i].transform.position += Vector3.right * UnitSize * 4;
                    }
                    else 
                        landArray[revise] = _landArray[i];
                }
                break;
            
            case 1:
                for(int i = 0; i < 16; i++)
                {
                    int revise = i % 4;

                    if (revise == 3)
                    {
                        RemoveItem(i);
                        SpawnItem(i);

                        landArray[i - 3] = _landArray[i];
                        _landArray[i].transform.position += Vector3.forward * UnitSize * 4;
                    }
                    else   
                        landArray[i + 1] = _landArray[i];
                }
                break;
            
            case 2:
                for (int i = 0; i < 16; i++)
                {
                    int revise = i + 4;

                    if (revise > 15)
                    {
                        RemoveItem(i);
                        SpawnItem(i);

                        landArray[revise - 16] = _landArray[i];
                        _landArray[i].transform.position -= Vector3.right * UnitSize * 4;
                    
                        
                    }
                    else
                        landArray[revise] = _landArray[i];
                }
                break;

            case 3:
                for (int i = 0; i < 16; i++)
                {
                    int revise = i % 4;

                    if (revise == 0)
                    {
                        RemoveItem(i);
                        SpawnItem(i);

                        landArray[i + 3] = _landArray[i];
                        _landArray[i].transform.position -= Vector3.forward * UnitSize * 4;
                    } 
                    else 
                        landArray[i - 1] = _landArray[i];
                }
                break;
        }
    }
}

