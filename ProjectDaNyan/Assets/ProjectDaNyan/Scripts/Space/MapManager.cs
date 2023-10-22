using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.AI.Navigation;
using UnityEditor.VersionControl;
using System.Threading.Tasks;
using System.Threading;
using UnityEditor.AI;
using UnityEngine;
using Task = System.Threading.Tasks.Task;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class MapManager : MonoBehaviour
{   
    [SerializeField] public GameObject[] tilePrefabs;
    [SerializeField] public GameObject player;
    [SerializeField] private GameObject boxCatPrefab;
    [SerializeField] public float UnitSize;
    [SerializeField] public GameObject restrictionPrefab;
    [SerializeField] public GameObject dustEffect;
    [SerializeField] public GameObject transparentObject;
    public int rowCount;
    public int columnCount;
    public Vector3 startPosition;
    public GameObject[] tiles;
    public int collectedCats = 0;
    Vector3[] border;
    Vector3 restrictionPosition;
    bool isFinalStarted = false;
    bool isMapRestricted = false;
    
    NavMeshSurface myNavMeshSurface; // (개선 필요) Floor 갱신시 Re-Bake 하기 위한 컴포넌트 
    
    private void Awake()
    {
        myNavMeshSurface = GetComponent<NavMeshSurface>();
    }
    
    void Start()
    {
        border = new Vector3[]
        {
            new Vector3(-UnitSize * rowCount / 2, 0, UnitSize * rowCount / 2),
            new Vector3(UnitSize * columnCount / 2, 0,  -UnitSize * columnCount / 2),
        };

        tiles = new GameObject[rowCount * columnCount];
        startPosition = new Vector3(UnitSize * -1.5f, 0, UnitSize * -1.5f);

        ShuffleTilePrefabs();

        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                int tilePrefabIndex = (row * columnCount + column) % tilePrefabs.Length;
                Vector3 position = startPosition + new Vector3(column * UnitSize, 0, row * UnitSize);
                tiles[row * columnCount + column] = Instantiate(tilePrefabs[tilePrefabIndex], position, Quaternion.identity);
                tiles[row * columnCount + column].transform.parent = transform;
            }
        }

        SpawnItemOntoMap();
        myNavMeshSurface.UpdateNavMesh(myNavMeshSurface.navMeshData);
    }

    void Update()
    {
        GameState currentGameState = GameManager.Inst.CurrentGameState;

        if (currentGameState == GameState.bossReady && !isFinalStarted)
        {
            isFinalStarted = true;
            restrictionPosition = player.transform.position;

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

        // 보스 킬 -> 제한 해제 추가 필요

        CheckBoundary();
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

    void ShuffleTilePrefabs()
    {
        for (int i = 0; i < tilePrefabs.Length; i++)
        {
            int randomIndex = Random.Range(i, tilePrefabs.Length);
            GameObject temp = tilePrefabs[i];
            tilePrefabs[i] = tilePrefabs[randomIndex];
            tilePrefabs[randomIndex] = temp;
        }
    }

    void ShuffleMovingTiles(List<int> movingTiles, GameObject[] tiles)
    {
        int n = movingTiles.Count;

        for (int i = 0; i < n; i++)
        {
            int randomIdx = Random.Range(i, n);
        
            Vector3 tempPosition = tiles[movingTiles[i]].transform.position;
            GameObject temp = tiles[movingTiles[i]];
            
            tiles[movingTiles[i]].transform.position = tiles[movingTiles[randomIdx]].transform.position;
            tiles[movingTiles[randomIdx]].transform.position = tempPosition;

            tiles[movingTiles[i]] = tiles[movingTiles[randomIdx]];
            tiles[movingTiles[randomIdx]] = temp;
        }
    }

    void SpawnItemOntoMap()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            SpawnItem(i);
        }
    }

    void SpawnItem(int index)
    {
        for (int j = 0; j < 5; j++)
        {
            float randomX = Random.Range(tiles[index].transform.position.x - UnitSize / 2, tiles[index].transform.position.x + UnitSize / 2);
            float randomZ = Random.Range(tiles[index].transform.position.z - UnitSize / 2, tiles[index].transform.position.z + UnitSize / 2); 
        
            Vector3 randomPosition = new Vector3(randomX, 1, randomZ);

            GameObject _boxCat = Instantiate(boxCatPrefab, randomPosition, Quaternion.identity);
            _boxCat.transform.parent = tiles[index].transform;
        }
    }

    void RemoveItem(int index)
    {
        foreach (Transform child in tilePrefabs[index].transform)
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
            MoveWorld(3);
        }

        // 왼쪽에 타일이 없을 경우
        else if (border[0].x > player.transform.position.x - UnitSize)
        {
            border[0] -= Vector3.right * UnitSize;
            border[1] -= Vector3.right * UnitSize;

            MoveWorld(1);
        }

        // 위쪽에 타일이 없을 경우
        else if (border[0].z < player.transform.position.z + UnitSize)
        {
            border[0] += Vector3.forward * UnitSize;
            border[1] += Vector3.forward * UnitSize;

            MoveWorld(0);
        }

        // 아래쪽에 타일이 없을 경우
        else if (border[1].z > player.transform.position.z - UnitSize)
        {
            border[0] -= Vector3.forward * UnitSize;
            border[1] -= Vector3.forward * UnitSize;

            MoveWorld(2);
        }
    }

    void MoveWorld(int dir)
    {
        // 기존 배열 복사
        GameObject[] _tiles = new GameObject[tilePrefabs.Length];
        System.Array.Copy(tiles, _tiles, tilePrefabs.Length);
        List<int> temp = new List<int>();

       switch (dir)
        {
            case 0:
                for(int i = 0; i < tiles.Length; i++)
                {
                    int revise = i - rowCount;

                    if (revise < 0)
                    {
                        RemoveItem(i);
                        SpawnItem(i);
                        
                        tiles[tiles.Length + revise] = _tiles[i];
                        tiles[tiles.Length + revise].transform.position += Vector3.forward * UnitSize * rowCount;

                        temp.Add(tiles.Length + revise);
                    }
                    else 
                        tiles[revise] = _tiles[i];
                }

                ShuffleMovingTiles(temp, tiles);

                break;
            
            case 1:
                for(int i = 0; i < 16; i++)
                {
                    int revise = i % 4;

                    if (revise == 3)
                    {
                        RemoveItem(i);
                        SpawnItem(i);

                        tiles[i - 3] = _tiles[i];
                        tiles[i - 3].transform.position -= Vector3.right * UnitSize * 4;

                        temp.Add(i - 3);
                    }
                    else   
                        tiles[i + 1] = _tiles[i];
                }

                ShuffleMovingTiles(temp, tiles);

                break;
            
            case 2:
                for (int i = 0; i < tiles.Length; i++)
                {
                    int revise = i + rowCount;

                    if (revise > (tiles.Length - 1))
                    {
                        RemoveItem(i);
                        SpawnItem(i);

                        tiles[revise - tiles.Length] = _tiles[i];
                        tiles[revise - tiles.Length].transform.position -= Vector3.forward * UnitSize * rowCount; 

                        temp.Add(revise - tiles.Length);
                    }
                    else
                        tiles[revise] = _tiles[i];
                }

                ShuffleMovingTiles(temp, tiles);

                break;

            case 3:
                for (int i = 0; i < 16; i++)
                {
                    int revise = i % 4;

                    if (revise == 0)
                    {
                        RemoveItem(i);
                        SpawnItem(i);

                        tiles[i + 3] = _tiles[i];
                        tiles[i + 3].transform.position += Vector3.right * UnitSize * 4;

                        temp.Add(i + 3);
                    } 
                    else 
                        tiles[i - 1] = _tiles[i];
                }

                ShuffleMovingTiles(temp, tiles);

                break;
        }

        myNavMeshSurface.UpdateNavMesh(myNavMeshSurface.navMeshData);
    }
}

