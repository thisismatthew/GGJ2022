using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BlobSortingCorrector : MonoBehaviour
{
    private SortingGroup renderer;
    public int offset = 3;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SortingGroup>();
    }
    // Update is called once per frame
    void Update()
    {
        renderer.sortingOrder = -(int)transform.position.y;
    }
}
