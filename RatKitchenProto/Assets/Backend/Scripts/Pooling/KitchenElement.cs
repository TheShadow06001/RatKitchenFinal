using UnityEngine;

[CreateAssetMenu(fileName = "KitchenElement", menuName = "Scriptable Objects/KitchenElement")]
public class KitchenElement : ScriptableObject
{
    public string ElementName;
    public int MaxCount;
    public GameObject Prefab;
}