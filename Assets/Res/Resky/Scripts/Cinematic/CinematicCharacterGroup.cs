using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class CinematicCharacterGroup : MonoBehaviour
{ 
    [SerializeField] private string charName;
    [SerializeField] private GameObject[] characters;

    private void OnEnable()
    {
        characters.ToList().ForEach(character => character.SetActive(true));
    }
    [Button("Set Active False")]
    public void SetActiveFalse()
    {
        characters.ToList().ForEach(character => character.SetActive(false));
    }
    [Button("Find Characters")]
    public void FindCharacters()
    {
        GameObject[] allCharacters = GameObject.FindGameObjectsWithTag($"Character");
        List<GameObject> specificCharacters = allCharacters.Where(t => t.name == charName).ToList();
        characters = new GameObject[specificCharacters.Count];
        for (int i = 0; i < specificCharacters.Count; i++)
        {
            characters[i] = specificCharacters[i];
        }
    }
}
