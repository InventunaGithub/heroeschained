using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    public GameCharacter PlayerGameCharacter;
    Hero[] heroes;
    public float Gold;

    private void Awake()
    {
        PlayerGameCharacter = new GameCharacter();
    }
}
