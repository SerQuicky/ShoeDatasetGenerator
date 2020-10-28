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
}
