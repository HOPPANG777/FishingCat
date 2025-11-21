using Sirenix.OdinInspector;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class ParallaxBackgroundController : MonoBehaviour, IMoveable
{
    [TabGroup("Direction")][Tooltip("이동 방향")][SerializeField] private Vector3 _moveDirection;
    public Vector3 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }

    [TabGroup("Background")][Tooltip("원본 레이어")][SerializeField] private Transform[] _bgLayer;            // 원본 레이어
    public Transform[] BackgroundLayer { get { return _bgLayer; } set { _bgLayer = value; } }

    [TabGroup("Background")][Tooltip("복제 레이어")][SerializeField] private Transform[] _bgLayerDup;         // 복제 레이어
    public Transform[] BackgroundLayerDump { get { return _bgLayerDup; } set { _bgLayerDup = value; } }

    [TabGroup("Background")][Tooltip("원본 초기 위치")][SerializeField] private Vector3[] _bgOrigin;          // 원본 초기 위치
    public Vector3[] BackgroundOrigin { get { return _bgOrigin; } set { _bgOrigin = value; } }

    [TabGroup("Background")][Tooltip("복제 초기 위치")][SerializeField] private Vector3[] _bgOriginDup;       // 복제 초기 위치
    public Vector3[] BackgroundOriginDump { get { return _bgOriginDup; } set { _bgOriginDup = value; } }

    [TabGroup("Background")][Tooltip("레이어 개수")][SerializeField] private int _layerCount = 0;             // 레이어 개수
    public int LayerCount { get { return _layerCount; } set { _layerCount = value; } }

    [TabGroup("Camera")][Tooltip("메인 카메라")][SerializeField] private Camera _mainCamera;                  // 메인 카메라 참조

    public void Move()
    {
        // 카메라 위치를 기준으로 패럴럭스 효과 적용
        Vector3 camPos = _mainCamera.transform.position;

        for (int i = 0; i < _layerCount; i++)
        {
            if (_bgLayer[i] != null && _bgLayerDup[i] != null)
            {
                // 레이어별 패럴럭스 팩터 (예: i=0일 때 0.2, i=layerCount-1일 때 1.0)
                float factor = 0.2f + (i * (0.8f / (_layerCount - 1)));

                // 카메라 이동에 따른 X, Y축 오프셋 계산
                float offsetX = camPos.x * factor;
                float offsetY = camPos.y * factor;

                // 원본 및 복제 배경 위치 계산
                Vector3 newBgPos = _bgOrigin[i] + new Vector3(offsetX, offsetY, 0);
                Vector3 newBgDupPos = _bgOriginDup[i] + new Vector3(offsetX, offsetY, 0);

                _bgLayer[i].position = newBgPos;
                _bgLayerDup[i].position = newBgDupPos;

                // 무한 X 스크롤 처리 (배경 너비는 카메라 뷰포트 기준으로 계산)
                float bgWidth = _mainCamera.orthographicSize * 2.0f * _mainCamera.aspect * 1.2f;

                // 왼쪽 경계 넘김
                while (newBgPos.x + bgWidth * 0.5f < camPos.x)
                {
                    _bgLayer[i].position = _bgLayerDup[i].position + new Vector3(bgWidth, 0, 0);
                    // swap 대신 참조 교환
                    Transform temp = _bgLayer[i];
                    _bgLayer[i] = _bgLayerDup[i];
                    _bgLayerDup[i] = temp;

                    newBgPos = _bgLayer[i].position;
                }

                // 오른쪽 경계 넘김
                float rightBound = camPos.x + bgWidth - bgWidth * 0.5f;
                while (newBgPos.x > rightBound)
                {
                    _bgLayer[i].position = _bgLayerDup[i].position - new Vector3(bgWidth, 0, 0);
                    Transform temp = _bgLayer[i];
                    _bgLayer[i] = _bgLayerDup[i];
                    _bgLayerDup[i] = temp;

                    newBgPos = _bgLayer[i].position;
                }
            }
        }
    }
}

