using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;


public class DataBase : MonoBehaviour
{
    [SerializeField]internal List<Item> items = new List<Item>();
}

[System.Serializable]

internal class Item
{
    public int id;
    public string name;
    public Sprite img;
}
