using DG.Tweening;
using UnityEngine;

public class GameObjectTouch : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _strength;
    [SerializeField] int _vibrato;
    [SerializeField] float _randomness;
    [SerializeField] bool _fadeOut;
    [SerializeField] private ShakeRandomnessMode _randomnessMode;
    
    
    void Update()
    {
        // 사용자의 입력을 감지합니다.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            // 터치가 시작되었을 때
            if (touch.phase == TouchPhase.Began)
            {
                // 터치한 위치에서 Ray를 발사합니다.
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Ray가 게임 오브젝트에 충돌했는지 확인합니다.
                if (Physics.Raycast(ray, out hit))
                {
                    // 충돌한 게임 오브젝트에 대한 처리를 수행합니다.
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        // 게임 오브젝트가 터치되었을 때의 동작을 정의합니다.
                        transform.localScale = new Vector3(1f, 1f, 1f);
                        transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.1f).OnComplete(() =>
                        {
                            transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
                        });
                    }
                }
            }
        }
    }
}