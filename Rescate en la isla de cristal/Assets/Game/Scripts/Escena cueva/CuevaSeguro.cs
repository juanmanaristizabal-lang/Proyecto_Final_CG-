using UnityEngine;

public class Cuevaseuro : MonoBehaviour
{
    void Start()
    {
        GameDataStructure.Instance.collectedCrystals.Clear();
        Debug.Log("Cueva reiniciada → cristales en 0");
    }
}
