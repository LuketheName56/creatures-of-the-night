using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Character Stats", menuName = "Create New Character Stats")]
public class MyNewCharacterStats : ScriptableObject
{
    public float speed;
    public float jumpHeight;
    public float jumpTime;
    public float jumpParryHeight;
    public float jumpParyTime;
    public LayerMask collisionLayerMask;
    public float collisionCheckDistance = 0.02f;
   
    public float Gravity { get; private set; }
    public float InitialJumpVelocity { get; private set; }
    
    private void OnValidate() => CalculateValues();
    private void OnEnable() => CalculateValues();
    
    // As described in Math for Game Programmers: Building a Better Jump https://youtu.be/hG9SzQxaCm8?si=kFOlkUusB9AV7hzf
    // Further simplified and modified by Sasquatch B Studios: https://youtu.be/zHSWG05byEc?si=LWoXSxXS3tw-ff3W
    private void CalculateValues()
    {
        Gravity = -(2f * jumpHeight * 1.054f) / Mathf.Pow(jumpTime, 2f); // g = -2h/t^2 and 1.054f is a magic number for height compensation
        InitialJumpVelocity = Mathf.Abs(Gravity) * jumpTime; // v0 = 2h/t simplified as sqrt(g)*t
    }
}