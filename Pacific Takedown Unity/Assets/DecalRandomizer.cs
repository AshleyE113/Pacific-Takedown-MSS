using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalRandomizer : MonoBehaviour
{

    public Sprite[] oilSprites;
    // Start is called before the first frame update
    void Start()
    {
        var random = Random.Range(0, oilSprites.Length-1);
        gameObject.GetComponent<SpriteRenderer>().sprite = oilSprites[random];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
