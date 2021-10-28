using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNpc : GameCharacter
{
    public GameCharacter GameCharacterNpc;

    private void Awake()
    {
        GameCharacterNpc = new GameCharacter();
    }
}
