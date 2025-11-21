using Sirenix.OdinInspector;
using UnityEngine;

public class EntityBase : MonoBehaviour, IPositionProvider
{
    [Header("Entity Base")]
    [TabGroup("IPositionProvider")][Tooltip("객체의 포지션을 받는 인터페이스")][SerializeField] protected Vector3 _position => this.transform.position;
    public Vector3 Position { get { return _position; } }

}
