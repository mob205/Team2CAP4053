using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Integer")]
public class ScriptableInt : ScriptableObject
{
    public int Value;

    public void Reset()
    {
        Value = 0;
    }
}
