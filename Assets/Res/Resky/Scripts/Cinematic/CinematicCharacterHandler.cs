using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class CinematicCharacterHandler : MonoBehaviour
{
    public GameObject[] CharacterObjs { get { return characterObjs; } }
    [Header("Testing")]
    [SerializeField] private int characterIndex = 0;
    [Header("Settings")]
    [SerializeField] private GameObject[] characterObjs = new GameObject[0];
    [SerializeField] private CinematicCharacterGroup[] characterGroups = new CinematicCharacterGroup[0];
    [SerializeField] private Gender[] genders = new Gender[0];

    [Space]
    [Header("Events")]
    [SerializeField] private UnityEvent onMaleSelected = new UnityEvent();
    [SerializeField] private UnityEvent onFemaleSelected = new UnityEvent();

    private void Start()
    {
        if (characterIndex > 2) characterIndex = 1;

        SetActiveFalse();
        if (characterObjs.Length != 0) characterObjs[characterIndex].SetActive(true);
        onMaleSelected.Invoke();
    }

    [Button("Set Active False")]
    private void SetActiveFalse()
    {
        foreach (var t in characterObjs)
        {
            if (t.TryGetComponent(out CinematicCharacterGroup group))
                group.SetActiveFalse();
            else
                t.SetActive(false);
        }
        foreach (var t in characterGroups)
        {
            t.SetActiveFalse();
        }
    }

    public enum Gender
    {
        Male = 0, Female = 1
    }
}
