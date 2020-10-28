using System;
using UnityEngine;

public interface IShoeInterface
{
    Tuple<Vector3, Vector3> GetFrontCoords();
    Tuple<Vector3, Vector3> GetBackCoords();
    Tuple<Vector3, Vector3> GetTopCoords();
    Tuple<Vector3, Vector3> GetDownCoords();
    Tuple<Vector3, Vector3> GetTopLeftCoords();
    Tuple<Vector3, Vector3> GetTopRightCoords();

}
