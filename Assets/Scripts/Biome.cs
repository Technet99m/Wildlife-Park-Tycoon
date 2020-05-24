using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Biome", menuName = "Biome", order = 54)]
public class Biome : ScriptableObject
{
    public string[] kinds;
    public Sprite background, fence, icon;
}
