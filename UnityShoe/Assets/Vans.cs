using System;
using UnityEngine;

public class Vans : IShoeInterface
{

    public Vans(string name, GameObject gameObject){
        Name = name;
        ShoeType = gameObject;
    }

    public string Name { get; set; }
    public GameObject ShoeType { get; set; }

    Tuple<Vector3, Vector3> IShoeInterface.GetFrontCoords(CapsuleCollider collider) {
        Vector3 tl = new Vector3(ShoeType.transform.position.x + (collider.height * ShoeType.transform.localScale.x * 0.5f), ShoeType.transform.position.y + (collider.radius * ShoeType.transform.localScale.y), ShoeType.transform.position.z);
        Vector3 br = new Vector3(ShoeType.transform.position.x - (collider.height * ShoeType.transform.localScale.x * 0.5f), ShoeType.transform.position.y - (collider.radius * ShoeType.transform.localScale.y), ShoeType.transform.position.z);
        return new Tuple<Vector3, Vector3>(tl, br);
    }


    Tuple<Vector3, Vector3> IShoeInterface.GetTopCoords(CapsuleCollider collider) {
        Vector3 tl = new Vector3(ShoeType.transform.position.x + (collider.radius * ShoeType.transform.localScale.x * 3f), ShoeType.transform.position.y, ShoeType.transform.position.z + (collider.radius * ShoeType.transform.localScale.z));
        Vector3 br = new Vector3(ShoeType.transform.position.x - (collider.radius * ShoeType.transform.localScale.x * 3f), ShoeType.transform.position.y, ShoeType.transform.position.z - (collider.radius * ShoeType.transform.localScale.z));
        return new Tuple<Vector3, Vector3>(tl, br);
    }


    Tuple<Vector3, Vector3> IShoeInterface.GetTopLeftCoords(CapsuleCollider collider) {
       Vector3 tl = new Vector3(ShoeType.transform.position.x + (collider.radius * ShoeType.transform.localScale.x * 2.75f), ShoeType.transform.position.y, ShoeType.transform.position.z + (collider.radius * ShoeType.transform.localScale.z * 1.5f));
       Vector3 br = new Vector3(ShoeType.transform.position.x - (collider.radius * ShoeType.transform.localScale.x * 2.75f), ShoeType.transform.position.y, ShoeType.transform.position.z - (collider.radius * ShoeType.transform.localScale.z * 1.5f));
       return new Tuple<Vector3, Vector3>(tl, br);
    }

    Tuple<Vector3, Vector3> IShoeInterface.GetTopRightCoords(CapsuleCollider collider) {
        Vector3 tl = new Vector3(ShoeType.transform.position.x + (collider.radius * ShoeType.transform.localScale.x * 3f), ShoeType.transform.position.y, ShoeType.transform.position.z + (collider.radius * ShoeType.transform.localScale.z * 1.5f));
        Vector3 br = new Vector3(ShoeType.transform.position.x - (collider.radius * ShoeType.transform.localScale.x * 3f), ShoeType.transform.position.y, ShoeType.transform.position.z - (collider.radius * ShoeType.transform.localScale.z * 1.5f));
        return new Tuple<Vector3, Vector3>(tl, br);
    }

    Quaternion IShoeInterface.ResolveShoeQuaternion(bool rotate) {
        return Quaternion.Euler(0f, rotate ? 0 : 180f, 0f);
    }
}
