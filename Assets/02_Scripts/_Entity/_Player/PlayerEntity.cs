using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerEntity : EntityBase
{
    [TabGroup("Controller")][Tooltip("플레이어 이동 컨트롤러")][SerializeField] private PlayerMoveController _playerMoveController;
    public PlayerMoveController PlayerMoveController { get { return _playerMoveController; } }

    void Start()
    {
        
    }

    
    void Update()
    {
        if (_playerMoveController.MoveDirection.x <= -1)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (_playerMoveController.MoveDirection.x >= 1)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }

    }
}
