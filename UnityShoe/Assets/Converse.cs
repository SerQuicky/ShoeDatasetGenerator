using System;
using UnityEngine;

public class Converse : IShoeInterface
{

    public Converse(string name, GameObject gameObject)
    {
        Name = name;
        ShoeType = gameObject;
    }

    public string Name { get; set; }
    public GameObject ShoeType { get; set; }

    Tuple<Vector3, Vector3> IShoeInterface.GetFrontCoords(CapsuleCollider collider)
    {
        Vector3 tl = new Vector3(ShoeType.transform.position.x - (collider.height * ShoeType.transform.localScale.y / 2) + collider.center.x, ShoeType.transform.position.y + (collider.radius * ShoeType.transform.localScale.x / 2) + Mathf.Abs(collider.center.y * ShoeType.transform.localScale.y), ShoeType.transform.position.z) + new Vector3(0, 15f, 0);
        Vector3 br = new Vector3(ShoeType.transform.position.x + (collider.height * ShoeType.transform.localScale.y / 2) + collider.center.x, ShoeType.transform.position.y - (collider.radius * ShoeType.transform.localScale.y / 2) - Mathf.Abs(collider.center.y * ShoeType.transform.localScale.y), ShoeType.transform.position.z) + new Vector3(0, 20f, 0);
        return new Tuple<Vector3, Vector3>(tl, br);
    }

    Tuple<Vector3, Vector3> IShoeInterface.GetBackCoords(CapsuleCollider collider)
    {
        return new Tuple<Vector3, Vector3>(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
    }

    Tuple<Vector3, Vector3> IShoeInterface.GetTopCoords(CapsuleCollider collider)
    {
        return new Tuple<Vector3, Vector3>(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
    }

    Tuple<Vector3, Vector3> IShoeInterface.GetDownCoords(CapsuleCollider collider)
    {
        return new Tuple<Vector3, Vector3>(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
    }


    Tuple<Vector3, Vector3> IShoeInterface.GetTopLeftCoords(CapsuleCollider collider)
    {
        Vector3 tl = new Vector3(ShoeType.transform.position.x + (collider.radius * ShoeType.transform.localScale.x * 2f), ShoeType.transform.position.y, ShoeType.transform.position.z);
        Vector3 br = new Vector3(ShoeType.transform.position.x - (collider.radius * ShoeType.transform.localScale.x * 1.5f), ShoeType.transform.position.y, ShoeType.transform.position.z - (collider.radius * ShoeType.transform.localScale.z * 1.25f));
        return new Tuple<Vector3, Vector3>(tl, br);
    }

    Tuple<Vector3, Vector3> IShoeInterface.GetTopRightCoords(CapsuleCollider collider)
    {
        Vector3 tl = new Vector3(ShoeType.transform.position.x + (collider.radius * ShoeType.transform.localScale.x * 1.5f), ShoeType.transform.position.y, ShoeType.transform.position.z);
        Vector3 br = new Vector3(ShoeType.transform.position.x - (collider.radius * ShoeType.transform.localScale.x * 2.5f), ShoeType.transform.position.y, ShoeType.transform.position.z - (collider.radius * ShoeType.transform.localScale.z * 1.25f));
        return new Tuple<Vector3, Vector3>(tl, br);
    }

    Quaternion IShoeInterface.ResolveShoeQuaternion()
    {
        return Quaternion.Euler(0f, 90f, 0f);
    }
}
