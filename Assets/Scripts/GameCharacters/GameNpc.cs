using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNpc : MonoBehaviour
{
    public GameCharacter GameCharacterNpc;

    private void Awake()
    {
        GameCharacterNpc = new GameCharacter();
    }
}
