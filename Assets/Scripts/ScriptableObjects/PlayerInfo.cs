using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
public class PlayerInfo : ScriptableObject
{
    [Range(1f, 15f)]
    public float WalkingSpeed = 6f;
    [Range(1f, 15f)]
    public float RunningSpeed = 10f;
    [Range(1f, 20f)]
    public float RotationPower = 10f;
    [Range(5f, 20f)]
    public float Gravity = 10f;
    public GameObject YangProjectile;
}