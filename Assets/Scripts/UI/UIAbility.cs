﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAbility : MonoBehaviour
{
    public Image greyedOutCircleImage;
    public Text abilityKey;    

    public void Initialize(string key)
    {
        abilityKey.text = key.ToString();
    }

    public void Refresh(float abilityCooldownPerc)
    {        
    }

    //A pseudo factory pattern, uses a static function to create a new one and do all the setup needed, and return reference
    static GameObject uiAbilityPrefab;
    public static UIAbility CreateAbilityUI(string key)    {
        uiAbilityPrefab = (uiAbilityPrefab) ? uiAbilityPrefab : Resources.Load<GameObject>(PrefabFileDir.UI_ABILITY_RESOURCE_PATH); //
        UIAbility newAbility = GameObject.Instantiate(uiAbilityPrefab, UILinks.instance.abilityGridParent).GetComponent<UIAbility>();
        newAbility.Initialize(key);
        return newAbility;
        //The above line is creating an object from a prefab, then getting the component from it caleld UIAbility, and returning it, remember this is a static function. 
    }
}
