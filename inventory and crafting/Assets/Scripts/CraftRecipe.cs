using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftRecipe
{
    public int[] RequiredItems;
    public int itemToCraft;

    public CraftRecipe(int itemToCraft,int[] requiredItems)
    {
        this.RequiredItems = requiredItems;
        this.itemToCraft = itemToCraft;
    }
}
