using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{ 
    Enemy,
    Resource,
    Struct
}

public class Target : MonoBehaviour
{
    public TargetType Type;
    public int CircleRenderDistance;
}
