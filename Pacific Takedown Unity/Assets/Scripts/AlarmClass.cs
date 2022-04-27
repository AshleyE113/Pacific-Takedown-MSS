using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClass : MonoBehaviour
{
    SpriteRenderer alarmSprite;
    [SerializeField] float timeVal;
    bool isChanging = false;
    // Start is called before the first frame update
    void Start()
    {
        alarmSprite = GetComponent<SpriteRenderer>();
       
    }

    void Update()
    {
        if (isChanging == false)
        {
            isChanging = true;
            StartCoroutine(Flashing(timeVal));
        }
    }

    IEnumerator Flashing(float duration)
    {
        var tempColor = alarmSprite.color;
        tempColor.a = 0.6f;
        alarmSprite.color = tempColor;
        yield return new WaitForSeconds(duration);
        tempColor.a = 0.4f;
        alarmSprite.color = tempColor;
        yield return new WaitForSeconds(duration);
        isChanging = false;
    }

}
