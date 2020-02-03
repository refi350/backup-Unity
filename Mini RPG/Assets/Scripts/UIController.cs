﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI inventoryText;
    public TextMeshProUGUI interactText;
    public Image healthBarFill;
    public Image xpBarFill;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public void UpdateLevelText()
    {
        levelText.text = "LVL\n" + player.curLevel;
    }

    public void UpdateHealthBar()
    {
        healthBarFill.fillAmount = (float)player.curHP / (float)player.maxHP;
    }

    public void UpdateXPBar()
    {
        xpBarFill.fillAmount = (float)player.curXP / (float)player.xpToNextLevel;
    }

    public void SetInteractText(Vector3 pos, string text)
    {
        interactText.gameObject.SetActive(true);
        interactText.text = text;

        interactText.transform.position = Camera.main.WorldToScreenPoint(pos + Vector3.up);
    }

    public void DisableInteractText()
    {
        if (interactText.gameObject.activeInHierarchy)
        {
            interactText.gameObject.SetActive(false);
        }
    }

    public void UpdateInventoryText()
    {
        inventoryText.text = "";

        foreach(string item in player.inventory)
        {
            inventoryText.text += item + "\n";
        }
    }

}
