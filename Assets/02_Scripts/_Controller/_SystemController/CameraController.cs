using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

//==============================================================================
// 기능: CameraController
// 설명: Camera의 이동을 담당하는 Controller
//==============================================================================
// TODO: 플레이어 추적 구현
//       
//       
//        
//       
//==============================================================================

public class CameraController : MonoBehaviour, IMoveable
{
    [TabGroup("Camera")][Tooltip("카메라")][SerializeField] private Camera _camera;
    public Camera MainCamera { get { return _camera; } }

    [TabGroup("Camera")][Tooltip("카메라가 따라다니는 속도")][SerializeField] private float _cameraFollowSpeed; // 기본이 1
    public float CameraFollowSpeed { get { return _cameraFollowSpeed; } }

    [TabGroup("Camera")][Tooltip("카메라가 따라다니는 속도")][SerializeField] private float _cameraFixedZ; // 기본이 10
    public float CameraFixedZ { get { return _cameraFixedZ; } }

    [TabGroup("Camera")][Tooltip("카메라가 움직일 범위 최소 비율")][SerializeField] private float _minRatio; // 기본이 0.5
    public float MinRatio { get { return _minRatio; } }

    [TabGroup("Camera")][Tooltip("카메라가 움직일 범위 최대 비율")][SerializeField] private float _maxRatio; // 기본이 1.5
    public float MaxRatio { get { return _maxRatio; } }

    [TabGroup("Target")][SerializeField] private GameObject _cameraTarget;
    public GameObject CameraTarget { get { return _cameraTarget; } }

    [TabGroup("Target")][SerializeField] private IPositionProvider _cameraTargetPosition;
    public IPositionProvider CameraTargetPosition { get { return _cameraTargetPosition; } }

    [TabGroup("Direction")][SerializeField] private Vector3 _moveDirection;
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

    // 카메라 사이즈 변경
    public void SetOrthographicSize(float size)
    {
        if (MainCamera != null)
        {
            MainCamera.orthographicSize = size;
        }
    }

    //public void Init()
    //{
    //    _cameraTarget = AllManager.instance.GamemodeManager.Player;
    //}

    void Start()
    {
        CheckCamera();
    }

    void Update()
    {
        Move();
    }

    private bool CheckCamera()
    {
        if (_cameraTarget.TryGetComponent<IPositionProvider>(out var provider))
        {
            _cameraTargetPosition = provider;
            return true;
        }
        else
        {
            Debug.LogError("IPositionProvider 컴포넌트를 찾을 수 없습니다!");
            return false;
        }
    }

    public void Move()
    {
        if (CheckCamera() == false) return;

        UpdateLerpScroll();
        //UpdateHorizontalScroll();
        //UpdateVerticalScroll();
    }

    private void UpdateLerpScroll()
    {
        // 타겟의 월드 좌표
        Vector3 targetPos = _cameraTargetPosition.Position;
        Vector3 viewportPos = MainCamera.WorldToViewportPoint(targetPos);

        // x와 y 모두 각각의 조건 범위 내에 있을 경우에만 업데이트
        if (viewportPos.x > _minRatio || viewportPos.x < _maxRatio || viewportPos.y > _minRatio || viewportPos.y < _maxRatio)
        {
            Vector3 targetCameraPos = new Vector3(targetPos.x, targetPos.y, MainCamera.transform.position.z);
            MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, targetCameraPos, Time.deltaTime * _cameraFollowSpeed);
        }
    }

    private void UpdateHorizontalScroll()
    {
        // 타겟의 월드 좌표
        Vector3 targetPos = _cameraTargetPosition.Position;
        Vector3 viewportPos = MainCamera.WorldToViewportPoint(targetPos);

        // x와 y 모두 각각의 조건 범위 내에 있을 경우에만 업데이트
        if (viewportPos.x > _minRatio && viewportPos.x < _maxRatio)
        {
            // target의 중앙이 화면 중앙에 오도록 카메라 위치 설정
            float newX = targetPos.x; // - viewportPos.x / 2.0f;
            //Vector3 targetCameraPos = new Vector3(targetPos.x, MainCamera.transform.position.y, MainCamera.transform.position.z);

            // 카메라 이동
            MainCamera.transform.position = new Vector3(newX, MainCamera.transform.position.y, MainCamera.transform.position.z);
            //MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, targetCameraPos, Time.deltaTime * _cameraFollowSpeed);
        }
    }

    private void UpdateVerticalScroll()
    {
        // 타겟의 월드 좌표
        Vector3 targetPos = _cameraTargetPosition.Position;
        Vector3 viewportPos = MainCamera.WorldToViewportPoint(targetPos);

        // x와 y 모두 각각의 조건 범위 내에 있을 경우에만 업데이트
        if (viewportPos.y > _minRatio && viewportPos.y < _maxRatio)
        {
            // target의 중앙이 화면 중앙에 오도록 카메라 위치 설정
            float newY = targetPos.y; // - viewportPos.y / 2.0f;
            //Vector3 targetCameraPos = new Vector3(MainCamera.transform.position.x, targetPos.y, MainCamera.transform.position.z);

            // 카메라 이동
            MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, newY, MainCamera.transform.position.z);
            //MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, targetCameraPos, Time.deltaTime * _cameraFollowSpeed);
        }
    }
}
