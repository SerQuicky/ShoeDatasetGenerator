using System;
using UnityEngine;

public class Business: IShoeInterface
{

    public Business(string name, GameObject gameObject)
    {
        Name = name;
        ShoeType = gameObject;
    }

    public string Name { get; set; }
    public GameObject ShoeType { get; set; }

    Tuple<Vector3, Vector3> IShoeInterface.GetFrontCoords(CapsuleCollider collider)
    {
        Debug.Log("testB");
        Vector3 tl = new Vector3(ShoeType.transform.position.x - (collider.height * ShoeType.transform.localScale.y / 2) + collider.center.x, ShoeType.transform.position.y + (collider.radius * ShoeType.transform.localScale.x / 2) + Mathf.Abs(collider.center.y * ShoeType.transform.localScale.y), ShoeType.transform.position.z) + new Vector3(-5f, 20f, 0);
        Vector3 br = new Vector3(ShoeType.transform.position.x + (collider.height * ShoeType.transform.localScale.y / 2) + collider.center.x, ShoeType.transform.position.y - (collider.radius * ShoeType.transform.localScale.y / 2) - Mathf.Abs(collider.center.y * ShoeType.transform.localScale.y), ShoeType.transform.position.z) + new Vector3(0f, 7.5f, 0);
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
        Vector3 tl = new Vector3(ShoeType.transform.position.x + (collider.radius * ShoeType.transform.localScale.x * 3.5f), ShoeType.transform.position.y, ShoeType.transform.position.z + (collider.radius * ShoeType.transform.localScale.z * 0.5f));
        Vector3 br = new Vector3(ShoeType.transform.position.x - (collider.radius * ShoeType.transform.localScale.x * 2.5f), ShoeType.transform.position.y, ShoeType.transform.position.z - (collider.radius * ShoeType.transform.localScale.z * 2f));
        return new Tuple<Vector3, Vector3>(tl, br);
    }

    Tuple<Vector3, Vector3> IShoeInterface.GetTopRightCoords(CapsuleCollider collider)
    {
        Vector3 tl = new Vector3(ShoeType.transform.position.x + (collider.radius * ShoeType.transform.localScale.x * 2.25f), ShoeType.transform.position.y, ShoeType.transform.position.z + (collider.radius * ShoeType.transform.localScale.z * 0.5f));
        Vector3 br = new Vector3(ShoeType.transform.position.x - (collider.radius * ShoeType.transform.localScale.x * 4f), ShoeType.transform.position.y, ShoeType.transform.position.z - (collider.radius * ShoeType.transform.localScale.z * 2f));
        return new Tuple<Vector3, Vector3>(tl, br);
    }

    Quaternion IShoeInterface.ResolveShoeQuaternion()
    {
        return Quaternion.Euler(270f, 90f, 0f);
    }
}
