using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ingredient", menuName = "Ingredient", order = 51)]
public class Ingredient : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private Color color = Color.white;
}
