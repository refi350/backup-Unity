using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        BuildItemDatabase();
    }

    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    public Item GetItem(string title)
    {
        return items.Find(item => item.title == title);
    }

    void BuildItemDatabase()
    {
        items = new List<Item>()
        {
            new Item(1,"Diamond Pickaxe","A sword made of diamond.",
            new Dictionary<string,int> {
                {"Power", 15},
                {"Defence",7 }
            }),
            new Item(2,"Diamond Ore","A shiny diamond.",
            new Dictionary<string,int> {
                {"Value", 2500}
            }),
            new Item(3,"Steel Axe","A sword made of steel.",
            new Dictionary<string,int> {
                {"Power", 10},
                {"Defence",5 }
            })
        };
    }
}
