using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new PlayerData", menuName = "Game/Player Data")]
public class PlayerDataSO : ScriptableObject
{
    public float jogSpeed;
    public float runSpeed;
    public float sneakSpeed;
    public float jumpForce;
}
