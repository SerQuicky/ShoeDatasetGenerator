using System;
using UnityEngine;

public class Shoe
{
    public string name;
    public GameObject gameObject;

    public Shoe(string name, GameObject gameObject)
    {
        this.name = name;
        this.gameObject = gameObject;
    }

    public Tuple<Vector3, Vector3> GetBackCoords(CapsuleCollider collider)
    {
        throw new NotImplementedException();
    }

    public Tuple<Vector3, Vector3> GetDownCoords(CapsuleCollider collider)
    {
        throw new NotImplementedException();
    }

    public Tuple<Vector3, Vector3> GetFrontCoords(CapsuleCollider collider)
    {
        throw new NotImplementedException();
    }

    public Tuple<Vector3, Vector3> GetTopCoords(CapsuleCollider collider)
    {
        throw new NotImplementedException();
    }

    public Tuple<Vector3, Vector3> GetTopLeftCoords(CapsuleCollider collider)
    {
        throw new NotImplementedException();
    }

    public Tuple<Vector3, Vector3> GetTopRightCoords(CapsuleCollider collider)
    {
        throw new NotImplementedException();
    }

    public Quaternion ResolveShoeQuaternion()
    {
        throw new NotImplementedException();
    }
}
