using System.Collections;
using System.Collections.Generic;

using Isometric;

using UnityEngine;

public class TestGen : MonoBehaviour
{

    public GameObject TilePrefab;
    public List<GameObject> Tiles = new List<GameObject>();
    [ContextMenu("Generate Tiles")]
    void GenerateGrid()
    {
        if (TilePrefab == null)
        {
            Debug.LogError(("no tileprefab plz set"));
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                var go = Instantiate(TilePrefab, transform) as GameObject;
                go.GetComponent<IsometricTransform>().position = new Vector3(j, 0f, i);
                Tiles.Add(go);
            }
        }
    }

    [ContextMenu("Blow out")]
    void Degenerate()
    {
        Tiles.ForEach(child => DestroyImmediate(child));
        Tiles = new List<GameObject>();
    }
}
