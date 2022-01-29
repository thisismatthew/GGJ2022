using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sortPointCorrector : MonoBehaviour
{
    private SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        renderer.sortingOrder = -(int)transform.parent.position.y;
    }
}
