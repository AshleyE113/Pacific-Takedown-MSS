using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalRandomizer : MonoBehaviour
{

    public Sprite[] oilSprites;

    public float minSize=.5f;

    public float maxSize=1f;
    // Start is called before the first frame update
    void Start()
    {
        if (oilSprites.Length > 0)
        {
            var random = Random.Range(0, oilSprites.Length-1);
            gameObject.GetComponent<SpriteRenderer>().sprite = oilSprites[random];
        }
        gameObject.transform.localScale = new Vector3(Random.Range(minSize, maxSize), Random.Range(minSize, maxSize),
            Random.Range(minSize, maxSize));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
