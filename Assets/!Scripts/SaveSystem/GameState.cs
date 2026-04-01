using System;
using System.Collections.Generic;

[Serializable]
public class GameState
{
    public PlayerState Player;
    public List<EnemyState> Enemies = new List<EnemyState>();
}

[Serializable]
public struct PlayerState
{
    public float PosX;
    public float PosY;
    public float PosZ;
    public float Health;
}

[Serializable]
public struct EnemyState
{
    public string Id;
    public float PosX;
    public float PosY;
    public float PosZ;
    public float Health;
}
