using UnityEngine;

public abstract class Usable : MonoBehaviour
{
    [HideInInspector]
    public string useString = "Use";
    public virtual void Start() { gameObject.tag = "Usable";  }
    public abstract void Use(GameObject player);
}