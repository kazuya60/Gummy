using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character_",menuName = "NewCharacter")]
public class Character : ScriptableObject
{
    public Sprite CharacterSprite;
    public string CharacterName;
}
