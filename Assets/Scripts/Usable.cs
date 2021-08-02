using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Usable : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onUse;
    private readonly List<OutlineScript> outlines = new List<OutlineScript>();

    [SerializeField] private List<GameObject> outlineGameObjects;

    private void Awake()
    {
        if (outlineGameObjects.Count == 0)
            outlineGameObjects.Add(gameObject);
        foreach (GameObject target in outlineGameObjects)
        {
            OutlineScript outline = target.AddComponent<OutlineScript>();
            outline.OutlineMode = OutlineScript.Mode.OutlineVisible;
            outline.OutlineWidth = 5f;
            outline.OutlineColor = Color.green;
            outline.enabled = false;
            outlines.Add(outline);
        }
    }

    public void SetOutlineState(bool outlineState)
    {
        foreach (OutlineScript outline in outlines)
            outline.enabled = outlineState;
    }
    public void Use()
    {
        onUse.Invoke();
        print("huy");
    }
}