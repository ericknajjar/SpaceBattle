﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    ShipManagger _shipManagger;

    [SerializeField]
    float _speed = 2.0f;
    Transform _transform;
    // Start is called before the first frame update
    private void Awake()
    {
        _transform = transform;
    }
    void Start()
    {
        _shipManagger = FindObjectOfType<ShipManagger>();
        _transform = transform;
        Pos = _transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var okDistance = _speed * 1.5f * (1f/18);
        var ship = _shipManagger.FindClosestShip(this, okDistance);
        //Debug.Log(ShipId);
        if (ship != this)
        {
            var direction = (ship.Pos - Pos);

            transform.Translate(direction.normalized * _speed * Time.deltaTime);
        }
    }

    public int ShipId;

    public void LateUpdate()
    {
        Pos = _transform.position;
    }


    public Vector2 Pos;

}
