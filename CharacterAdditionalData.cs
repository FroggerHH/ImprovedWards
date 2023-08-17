using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ImprovedWards;

[Serializable]
public class CharacterAdditionalData
{
    public Rigidbody rigidbody;
    public Collider collider;

    public CharacterAdditionalData()
    {
        collider = null;
        rigidbody = null;
    }

    public void Init(Character character)
    {
        rigidbody = character.GetComponent<Rigidbody>();
        collider = character.GetComponent<Collider>();
    }
}

public static class CharacterExtension
{
    private static readonly ConditionalWeakTable<Character, CharacterAdditionalData> data = new();

    public static CharacterAdditionalData GetAdditionalData(this Character character) => data.GetOrCreateValue(character);

    public static void AddData(this Character character, CharacterAdditionalData value)
    {
        try
        {
            data.Add(character, value);
        }
        catch
        {
        }
    }
}