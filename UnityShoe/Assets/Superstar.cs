using System;
using UnityEngine;

public class Superstar : Shoe
{
    public Superstar(string name, GameObject gameObject) : base(name, gameObject) { }

    public Tuple<Vector3, Vector3> GetFrontCoords(CapsuleCollider collider)
    {
        Vector3 tl = new Vector3(gameObject.transform.position.x - (collider.height * gameObject.transform.localScale.y / 2) + collider.center.x, gameObject.transform.position.y + (collider.radius * gameObject.transform.localScale.x / 2) + Mathf.Abs(collider.center.y * gameObject.transform.localScale.y), gameObject.transform.position.z) + new Vector3(80f, 15f, 0);
        Vector3 br = new Vector3(gameObject.transform.position.x + (collider.height * gameObject.transform.localScale.y / 2) + collider.center.x, gameObject.transform.position.y - (collider.radius * gameObject.transform.localScale.y / 2) - Mathf.Abs(collider.center.y * gameObject.transform.localScale.y), gameObject.transform.position.z) + new Vector3(80f, 15f, 0);
        return new Tuple<Vector3, Vector3>(tl, br);
    }

    public Tuple<Vector3, Vector3> GetBackCoords(CapsuleCollider collider)
    {
        return new Tuple<Vector3, Vector3>(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
    }

    public Tuple<Vector3, Vector3> GetTopCoords(CapsuleCollider collider)
    {
        return new Tuple<Vector3, Vector3>(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
    }

    public Tuple<Vector3, Vector3> GetDownCoords(CapsuleCollider collider)
    {
        return new Tuple<Vector3, Vector3>(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
    }


    public Tuple<Vector3, Vector3> GetTopLeftCoords(CapsuleCollider collider)
    {
        Vector3 tl = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (collider.radius * gameObject.transform.localScale.y * 1.75f), gameObject.transform.position.z + (collider.radius * gameObject.transform.localScale.y * 1.25f));
        Vector3 br = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (collider.radius * gameObject.transform.localScale.y * 0.5f), gameObject.transform.position.z - (collider.radius * gameObject.transform.localScale.y * 0.5f));
        return new Tuple<Vector3, Vector3>(tl, br);
    }

    public Tuple<Vector3, Vector3> GetTopRightCoords(CapsuleCollider collider)
    {
        Vector3 tl = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (collider.radius * gameObject.transform.localScale.y * 1.75f), gameObject.transform.position.z + (collider.radius * gameObject.transform.localScale.y * 1.25f));
        Vector3 br = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (collider.radius * gameObject.transform.localScale.y * 0.5f), gameObject.transform.position.z - (collider.radius * gameObject.transform.localScale.y * 0.5f));
        return new Tuple<Vector3, Vector3>(tl, br);
    }

    public Quaternion ResolveShoeQuaternion()
    {
        return Quaternion.Euler(0f, 90f, 0f);
    }
}
