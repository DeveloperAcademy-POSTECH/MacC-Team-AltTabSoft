using System.Collections;
using System.Collections.Generic;
using System.Numerics;
<<<<<<< Updated upstream
using Unity.AI.Navigation;
// using UnityEditor.VersionControl;
using System.Threading.Tasks;
using System.Threading;
// using UnityEditor.AI;
=======
using UnityEditor.VersionControl;
using System.Threading.Tasks;
using System.Threading;
>>>>>>> Stashed changes
using UnityEngine;
using UnityEngine.Serialization;
using Unity.AI.Navigation;
using Task = System.Threading.Tasks.Task;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class MapManager : MonoBehaviour
{   
    [SerializeField] private GameObject player;
    [SerializeField] private float tileSize;
    [SerializeField] private int tileRowCount;
    [SerializeField] private int tileColumnCount;
    [SerializeField] public GameObject[] tilePrefabsArray;
    [SerializeField] private GameObject boxCatPrefab;
    [SerializeField] private int boxCatCounts;
    [SerializeField] private GameObject restrictionPrefab;
    [SerializeField] private GameObject transparentRestrictionPrefab;
    private GameObject _mapTiles;
    private GameObject _transparentRestrictions;
    private GameObject _restrictions;
    private GameObject[] _tilesArray;
    private int _tileTotalCount;
    private Vector3 _tileSetPosition;
    private Vector3[] _tileBorderRange;
    private Vector3 _restrictionSetPosition;
    private bool _isMapRestricted = false;
    
    NavMeshSurface _myNavMeshSurface; // (개선 필요) Floor 갱신시 Re-Bake 하기 위한 컴포넌트 
    
    private void Awake()
    {
        SetBorderRange();
        _tileTotalCount = tileRowCount * tileColumnCount;
        _tilesArray = new GameObject[tileRowCount * tileColumnCount];
        _tileSetPosition = new Vector3(tileSize * -1.5f, 0, tileSize * -1.5f);
        
        _mapTiles = GameObject.Find("MapTiles");
        _transparentRestrictions = GameObject.Find("TransparentRestrictions");
        _restrictions = GameObject.Find("Restrictions");
        _myNavMeshSurface = GetComponent<NavMeshSurface>();
    }
    
    private void Start()
    {
        ShuffleTilePrefabs();

        SetRandomMap();

        SpawnBoxCatsOntoMap();

        StartCoroutine(MakeRestrictions());
        
        _myNavMeshSurface.UpdateNavMesh(_myNavMeshSurface.navMeshData);
    }

    private void Update()
    {
        // 보스 킬 -> 제한 해제 추가 필요

        CheckBoundary();
    }
    
    private void SetBorderRange()
    {
        _tileBorderRange = new Vector3[]
        {
            new Vector3(-tileSize * (tileRowCount / 2f), 0, tileSize * (tileRowCount / 2f)),
            new Vector3(tileSize * (tileColumnCount / 2f), 0,  -tileSize * (tileColumnCount / 2f)),
        };
    }
    
    private void ShuffleTilePrefabs()
    {
        for (var idx = 0; idx < _tileTotalCount; idx++)
        {
            var randomIndex = Random.Range(idx, _tileTotalCount);
            var tempTilePrefab = tilePrefabsArray[idx];
            
            tilePrefabsArray[idx] = tilePrefabsArray[randomIndex];
            tilePrefabsArray[randomIndex] = tempTilePrefab;
        }
    }
    
    private void SetRandomMap()
    {
        for (var row = 0; row < tileRowCount; row++)
        {
            for (var column = 0; column < tileColumnCount; column++)
            {
                int tilePrefabIndex = (row * tileColumnCount + column) % _tileTotalCount;
                Vector3 tilePrefabPosition = _tileSetPosition + new Vector3(column * tileSize, 0, row * tileSize);
                
                _tilesArray[row * tileColumnCount + column] = Instantiate(tilePrefabsArray[tilePrefabIndex], tilePrefabPosition, Quaternion.identity);
                _tilesArray[row * tileColumnCount + column].transform.parent = _mapTiles.transform;
            }
        }
    }

    IEnumerator MakeRestrictions()
    {
        while (true)
        {
            var currentGameState = GameManager.Inst.CurrentGameState;

            if (currentGameState == GameState.inGame && !_isMapRestricted)
            {
                _restrictionSetPosition = player.transform.position;
                yield return InstallRestrictions(false);
                yield return InstallRestrictions(true);

                _isMapRestricted = true;

                yield break;
            }
            
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator InstallRestrictions(bool hasDustEffect)
    {
        var radius = tileSize;
        var angleIncrement = 360f / 8;
        var currentAngle = 45f;

        for (var i = 0; i < 8; i++)
        {
            var angleInRadius = currentAngle * Mathf.Deg2Rad;

            var positionX = _restrictionSetPosition.x + radius * Mathf.Cos(angleInRadius);
            var positionZ = _restrictionSetPosition.z + radius * Mathf.Sin(angleInRadius);

            var wallPosition = new Vector3(positionX, 0, positionZ);
            var angleRotation = currentAngle;

            if (angleRotation % 90 == 0)
            {
                angleRotation += 90;
            }

            var rotationQuaternion = Quaternion.Euler(0, angleRotation, 0);

            if (hasDustEffect)
            {
                var restriction = Instantiate(restrictionPrefab, wallPosition, rotationQuaternion);
                restriction.transform.parent = _restrictions.transform;

                currentAngle += angleIncrement;

                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                var transparentRestriction = Instantiate(transparentRestrictionPrefab, wallPosition, rotationQuaternion);
                transparentRestriction.transform.parent = _transparentRestrictions.transform;
                
                yield return null;
            }
        }
    }

    private void SpawnBoxCatsOntoMap()
    {
        for (var idx = 0; idx < _tileTotalCount; idx++)
        {
            SpawnBoxCats(idx);
        }
    }

    private void SpawnBoxCats(int index)
    {
        for (var i = 0; i < boxCatCounts; i++)
        {
            var randomX = Random.Range(_tilesArray[index].transform.position.x - (tileSize / 2), _tilesArray[index].transform.position.x + (tileSize / 2));
            var randomZ = Random.Range(_tilesArray[index].transform.position.z - (tileSize / 2), _tilesArray[index].transform.position.z + (tileSize / 2)); 
        
            var randomPosition = new Vector3(randomX, 1, randomZ);

            var boxCat = Instantiate(boxCatPrefab, randomPosition, Quaternion.identity);
            boxCat.transform.parent = _tilesArray[index].transform;
        }
    }

    private void RemoveBoxCats(int index)
    {
<<<<<<< Updated upstream
        foreach (Transform child in tiles[index].transform)
=======
        foreach (Transform child in _tilesArray[index].transform)
>>>>>>> Stashed changes
        {
            if (child.CompareTag("boxCat"))
            {
                Destroy(child.gameObject);
            }
        }
    }
    
    private void ShuffleMovingTiles(List<int> movingTiles, GameObject[] tilesArray)
    {
        var movingTilesCount = movingTiles.Count;

        for (var idx = 0; idx < movingTilesCount; idx++)
        {
            var randomIdx = Random.Range(idx, movingTilesCount);
        
            var tempPosition = tilesArray[movingTiles[idx]].transform.position;
            var tempTile = tilesArray[movingTiles[idx]];
            
            tilesArray[movingTiles[idx]].transform.position = tilesArray[movingTiles[randomIdx]].transform.position;
            tilesArray[movingTiles[randomIdx]].transform.position = tempPosition;

            tilesArray[movingTiles[idx]] = tilesArray[movingTiles[randomIdx]];
            tilesArray[movingTiles[randomIdx]] = tempTile;
        }
    }

    private void CheckBoundary()
    {
        // 위쪽에 타일이 없을 경우
        if (_tileBorderRange[0].z < player.transform.position.z + tileSize)
        {
            _tileBorderRange[0] += Vector3.forward * tileSize;
            _tileBorderRange[1] += Vector3.forward * tileSize;

            MoveWorld(0);
        }
        
        // 아래쪽에 타일이 없을 경우
        else if (_tileBorderRange[1].z > player.transform.position.z - tileSize)
        {
            _tileBorderRange[0] -= Vector3.forward * tileSize;
            _tileBorderRange[1] -= Vector3.forward * tileSize;

            MoveWorld(1);
        }
        
        // 왼쪽에 타일이 없을 경우
        else if (_tileBorderRange[0].x > player.transform.position.x - tileSize)
        {
            _tileBorderRange[0] -= Vector3.right * tileSize;
            _tileBorderRange[1] -= Vector3.right * tileSize;

            MoveWorld(2);
        }
        
        // 오른쪽에 타일이 없을 경우
        else if (_tileBorderRange[1].x < player.transform.position.x + tileSize)
        {
            // 타일 영역을 타일 하나 사이즈만큼 오른쪽으로 이동 
            _tileBorderRange[0] += Vector3.right * tileSize;
            _tileBorderRange[1] += Vector3.right * tileSize;

            // 타일을 실제로 오른쪽으로 움직임
            MoveWorld(3);
        }
    }

    private void MoveWorld(int dir)
    {
        // 기존 배열 복사
        var tiles = new GameObject[tilePrefabsArray.Length];
        System.Array.Copy(_tilesArray, tiles, _tileTotalCount);
        
        // 랜덤 재배치해야 하는 타일들 임시 저장
        var movingTiles = new List<int>();

       switch (dir)
        {
            // 위쪽에 타일이 없을 경우
            case 0:
                for(var idx = 0; idx < _tileTotalCount; idx++)
                {
                    var revise = idx - tileRowCount;

                    if (revise < 0)
                    {
                        RemoveBoxCats(idx);
                        SpawnBoxCats(idx);
                        
                        _tilesArray[_tileTotalCount + revise] = tiles[idx];
                        _tilesArray[_tileTotalCount + revise].transform.position += Vector3.forward * (tileSize * tileRowCount);

                        movingTiles.Add(_tileTotalCount + revise);
                    }
                    else 
                        _tilesArray[revise] = tiles[idx];
                }

                ShuffleMovingTiles(movingTiles, _tilesArray);

                break;
            
            // 아래쪽에 타일이 없을 경우
            case 1:
                for (var idx = 0; idx < _tileTotalCount; idx++)
                {
                    var revise = idx + tileRowCount;

                    if (revise > (_tileTotalCount - 1))
                    {
                        RemoveBoxCats(idx);
                        SpawnBoxCats(idx);

                        _tilesArray[revise - _tileTotalCount] = tiles[idx];
                        _tilesArray[revise - _tileTotalCount].transform.position -= Vector3.forward * (tileSize * tileRowCount); 

                        movingTiles.Add(revise - _tileTotalCount);
                    }
                    else
                        _tilesArray[revise] = tiles[idx];
                }

                ShuffleMovingTiles(movingTiles, _tilesArray);

                break;
            
            // 왼쪽에 타일이 없을 경우
            case 2:
                for(var idx = 0; idx < _tileTotalCount; idx++)
                {
                    var revise = idx % tileColumnCount;

                    if (revise == tileColumnCount - 1)
                    {
                        RemoveBoxCats(idx);
                        SpawnBoxCats(idx);

                        _tilesArray[idx - (tileColumnCount - 1)] = tiles[idx];
                        _tilesArray[idx - (tileColumnCount - 1)].transform.position -= Vector3.right * (tileSize * tileColumnCount);

                        movingTiles.Add(idx - (tileColumnCount - 1));
                    }
                    else   
                        _tilesArray[idx + 1] = tiles[idx];
                }

                ShuffleMovingTiles(movingTiles, _tilesArray);

                break;
            
            // 오른쪽에 타일이 없을 경우
            case 3:
                for (var idx = 0; idx < _tileTotalCount; idx++)
                {
                    var revise = idx % tileColumnCount;

                    if (revise == 0)
                    {
                        RemoveBoxCats(idx);
                        SpawnBoxCats(idx);

                        _tilesArray[idx + (tileColumnCount - 1)] = tiles[idx];
                        _tilesArray[idx + (tileColumnCount - 1)].transform.position += Vector3.right * (tileSize * tileColumnCount);

                        movingTiles.Add(idx + (tileColumnCount - 1));
                    } 
                    else 
                        _tilesArray[idx - 1] = tiles[idx];
                }

                ShuffleMovingTiles(movingTiles, _tilesArray);

                break;
        }

        _myNavMeshSurface.UpdateNavMesh(_myNavMeshSurface.navMeshData);
    }
}

