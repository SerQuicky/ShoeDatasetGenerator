using System;
using UnityEngine;

public interface IShoeInterface
{
    Tuple<Vector3, Vector3> GetFrontCoords(CapsuleCollider collider);
    Tuple<Vector3, Vector3> GetBackCoords(CapsuleCollider collider);
    Tuple<Vector3, Vector3> GetTopCoords(CapsuleCollider collider);
    Tuple<Vector3, Vector3> GetDownCoords(CapsuleCollider collider);
    Tuple<Vector3, Vector3> GetTopLeftCoords(CapsuleCollider collider);
    Tuple<Vector3, Vector3> GetTopRightCoords(CapsuleCollider collider);
    Quaternion ResolveShoeQuaternion();

}
