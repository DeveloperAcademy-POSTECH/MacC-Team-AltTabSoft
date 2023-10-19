using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
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
    [SerializeField] private GameObject boxCatPrefab;
    [SerializeField] public float UnitSize;
    [SerializeField] public GameObject restrictionPrefab;
    [SerializeField] public GameObject dustEffect;
    [SerializeField] public GameObject transparentObject;
    private GameObject boxCat;
    public int collectedCats = 0;
    Vector3[] border;
    int currentTileIndex;
    Vector3 currentPlayerPosition;
    Vector3 restrictionPosition;
    // 보스전 시작 여부
    bool isFinalStarted = false;
    // 보스전 -> 맵 제한 발동 여부
    bool isMapRestricted = false;
    
    NavMeshSurface myNavMeshSurface; // (개선 필요) Floor 갱신시 Re-Bake 하기 위한 컴포넌트 
    
    private void Awake()
    {
        myNavMeshSurface = GetComponent<NavMeshSurface>();
    }
    
    void Start()
    {
        boxCat = Instantiate(boxCatPrefab);

        // border 초기화, 플레이어의 시작 위치는 (0, 1, 0)이므로 왼쪽 끝 좌표는 -UnitSize * 2f, 오른쪽 끝 좌표는 UnitSize * 2f
        // 둘을 합하면 UnitSize * 4이며 이는 전체 타일의 크기와 같음
        border = new Vector3[]
        {
            new Vector3(-UnitSize * 2f, 0, UnitSize * 2f),
            new Vector3(UnitSize * 2f, 0,  -UnitSize * 2f),
        };

        SpawnItemOntoMap();
    }

    void Update()
    {
        float passedTime = GameManager.Inst.GameTime;
        int min = Mathf.FloorToInt(passedTime / 60);
        int sec = Mathf.FloorToInt(passedTime % 60);

        // 최종 보스가 7분에 등장할 수 있도록 타이밍 수정 필요
        if (min == 0 && sec == 5 && !isFinalStarted && !isMapRestricted) {
            isFinalStarted = true;

            currentPlayerPosition = player.transform.position;

            GetTileIndex();
            
            Debug.Log("보스 등장");
        }

        RestrictMap();

        // 보스 킬 -> 제한 해제 추가 필요

        // 시야 안에 타일이 없는 경우 체크
        CheckBoundary();
    }
    
    void GetTileIndex()
    {
        GameObject closestTile = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject tile in landArray)
        {
            float distance = Vector3.Distance(currentPlayerPosition, tile.transform.position);
            if (distance < closestDistance)
            {
                closestTile = tile;
                closestDistance = distance;
            }
        }
        // 플레이어의 위치와 가장 가까운 타일의 인덱스를 currentIndex에 저장
        currentTileIndex = System.Array.IndexOf(landArray, closestTile);
        restrictionPosition = GetCenterPosition();
    }

    Vector3 GetCenterPosition()
    {
        float playerX = currentPlayerPosition.x;
        float playerZ = currentPlayerPosition.z;

        Vector3 tilePos = landArray[currentTileIndex].transform.position;

        float tileX = landArray[currentTileIndex].transform.position.x;
        float tileZ = landArray[currentTileIndex].transform.position.z;

        if (playerX < tileX && playerZ >= tileZ)
            return tilePos - Vector3.right * (UnitSize / 4f) + Vector3.forward * (UnitSize / 4f);
        else if (playerX < tileX && playerZ < tileZ) 
            return tilePos - Vector3.right * (UnitSize / 4f) - Vector3.forward * (UnitSize / 4f);
        else if (playerX > tileX && playerZ >= tileZ)
            return tilePos + Vector3.right * (UnitSize / 4f) + Vector3.forward * (UnitSize / 4f);
        else
            return tilePos + Vector3.right * (UnitSize / 4f) - Vector3.forward * (UnitSize / 4f);
    }

    void RestrictMap()
    {
        if (isFinalStarted)
        {
            if (!isMapRestricted)
            {
                ReadyRestrictions();
                StartCoroutine(MakeResctrictions());

                isMapRestricted = true;
            }
            else 
                isFinalStarted = false;
        }
    }

    void ReadyRestrictions()
    {                                                                                                                       
        float radius = UnitSize;
        float angleIncrement = 360f / 8;
        float currentAngle = 45f;

        for (int i = 0; i < 8; i++)
        {
            float angleInRadius = currentAngle * Mathf.Deg2Rad;

            float x = restrictionPosition.x + radius * Mathf.Cos(angleInRadius);
            float z = restrictionPosition.z + radius * Mathf.Sin(angleInRadius);

            Vector3 wallPosition = new Vector3(x, 0, z);

            float angleRotation = currentAngle;
            if (angleRotation % 90 == 0)
            {
                angleRotation += 90;
            }

            Quaternion rotationQuaternion = Quaternion.Euler(0, angleRotation, 0);
            GameObject restriction = Instantiate(transparentObject, wallPosition, rotationQuaternion);

            currentAngle += angleIncrement;
        }
    }

    IEnumerator MakeResctrictions()
    {                                                                                                                     
        float radius = UnitSize;
        float angleIncrement = 360f / 8;
        float currentAngle = 45f;

        for (int i = 0; i < 8; i++)
        {
            float angleInRadius = currentAngle * Mathf.Deg2Rad;

            float x = restrictionPosition.x + radius * Mathf.Cos(angleInRadius);
            float z = restrictionPosition.z + radius * Mathf.Sin(angleInRadius);

            Vector3 wallPosition = new Vector3(x, 0, z);

            float angleRotation = currentAngle;
            if (angleRotation % 90 == 0)
            {
                angleRotation += 90;
            }

            InstallRestriction(wallPosition, angleRotation);

            currentAngle += angleIncrement;

            yield return new WaitForSeconds(0.2f);
        }
    }

    void InstallRestriction(Vector3 position, float rotation = 0)
    {
        Quaternion rotationQuaternion = Quaternion.Euler(0, rotation, 0);
        GameObject dust = Instantiate(dustEffect, position, rotationQuaternion);
        GameObject restriction = Instantiate(restrictionPrefab, position, rotationQuaternion);
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
            float randomX = Random.Range(landArray[index].transform.position.x, landArray[index].transform.position.x);
            float randomZ = Random.Range(landArray[index].transform.position.z, landArray[index].transform.position.z); 
        
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

    void CheckBoundary()
    {
        // 오른쪽에 타일이 없을 경우
        if (border[1].x < player.transform.position.x + UnitSize)
        {
            // 타일 영역을 타일 하나 사이즈만큼 오른쪽으로 이동 
            border[0] += Vector3.right * UnitSize;
            border[1] += Vector3.right * UnitSize;

            // 타일을 실제로 오른쪽으로 움직임
            MoveWorld(0);
        }

        // 왼쪽에 타일이 없을 경우
        else if (border[0].x > player.transform.position.x - UnitSize)
        {
            border[0] -= Vector3.right * UnitSize;
            border[1] -= Vector3.right * UnitSize;

            MoveWorld(2);
        }

        // 위쪽에 타일이 없을 경우
        else if (border[0].z < player.transform.position.z + UnitSize)
        {
            border[0] += Vector3.forward * UnitSize;
            border[1] += Vector3.forward * UnitSize;

            MoveWorld(1);
        }

        // 아래쪽에 타일이 없을 경우
        else if (border[1].z > player.transform.position.z - UnitSize)
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
        // (MapManager1 -> MapManager) 맵 이동 시 navmeshsurface bake
        myNavMeshSurface.BuildNavMesh();
    }
}

