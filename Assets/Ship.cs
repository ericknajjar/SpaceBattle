using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    ShipManagger _shipManagger;

    [SerializeField]
    public float _speed = 2.0f;
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

    public void Translate(Vector2 tanslation)
    {
        _transform.Translate(tanslation);
    }

    public int ShipId;

    public void LateUpdate()
    {
        Pos = _transform.position;
    }


    public Vector2 Pos;

}
