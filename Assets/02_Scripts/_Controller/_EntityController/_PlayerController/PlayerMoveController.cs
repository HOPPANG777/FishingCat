using Sirenix.OdinInspector;
using UnityEngine;

//==============================================================================
// 기능: PlayerMoveController
// 설명: Player 이동 컨트롤러
//==============================================================================
// TODO: 플레이어 이동 구현
//       
//       
//        
//       
//==============================================================================

public class PlayerMoveController : MonoBehaviour, IMoveable
{
    [TabGroup("Target")][Tooltip("플레이어 활성화 확인")][SerializeField] private GameObject _player;
    public GameObject Player { get { return _player; } set { _player = value; } }

    [TabGroup("Target")][Tooltip("플레이어 이동 방향")][SerializeField] private Vector3 _moveDirection;
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

    [TabGroup("Target")][Tooltip("플레이어 이동 속도")][SerializeField] private float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        // 입력
        _moveDirection.x = Input.GetAxisRaw("Horizontal");

        // 방향 벡터
        Vector3 nextVector = _moveDirection.normalized * _moveSpeed * Time.deltaTime;

        this.transform.position = this.transform.position + nextVector;
    }
}
