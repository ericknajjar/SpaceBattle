using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManagger : MonoBehaviour
{
    [SerializeField]
    Ship _prefab;

    [SerializeField]
    int _numShips = 25;

    // Start is called before the first frame update
    List<Ship> _allShips;

    void Awake()
    {
        _allShips = new List<Ship>(_numShips);
        var min = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        for (int i = 0; i < _numShips; ++i)
        {
            var pos = RandomPos(min, max);
            var go = Instantiate(_prefab.gameObject, pos, Quaternion.identity);
            var ship = go.GetComponent<Ship>();
            ship.ShipId = i;
            _allShips.Add(ship);

        }
    }

    Vector2 RandomPos(Vector2 min, Vector2 max)
    {
        var x = UnityEngine.Random.Range(min.x, max.x);
        var y = UnityEngine.Random.Range(min.y, max.y);

        return new Vector2(x, y);
    }

    public Ship FindClosestShip(Ship ship, float okDistance)
    {
        var myPostion = ship.Pos;
        var closest = ship;
        float closestDistance = 999999;
        var count = _allShips.Count;


        for (int i = 0; i < count; ++i)
        {
            var target = _allShips[i];

            if (target.ShipId == ship.ShipId) continue;

            var distance = Vector2.Distance(target.Pos, myPostion);

            if (distance < closestDistance && distance > okDistance)
            {
                closestDistance = distance;
                closest = target;
            }
        }

        return closest;
    }
}
