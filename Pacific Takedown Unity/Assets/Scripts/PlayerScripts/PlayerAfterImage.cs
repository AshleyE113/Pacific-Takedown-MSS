using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{
    public float activeTime=0.1f;
    private float timeActivated;
    private float alpha;
    public float alphaSet = 0.8f;
    public float alphaMultiplier = 0.85f;
    private Transform player;
    private SpriteRenderer _SR;
    private SpriteRenderer playerSR;
    private Color _color;

    private void OnEnable()
    {
        _SR = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSR = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        _SR.sprite = playerSR.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        _color = new Color(1f, 1f, 1f, alpha);
        _SR.color = _color;

        if (Time.time >= (timeActivated + activeTime))
        {
            //Add back to pool
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
