using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipManagger : MonoBehaviour
{
    [SerializeField]
    Ship _prefab;

    [SerializeField]
    int _numShips = 25;

    // Start is called before the first frame update
    List<Ship> _allShips;


    Dictionary<int, int> _distanceCache = new Dictionary<int, int>();

    float _latestOkDistance;
    bool filled = false;
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

        var count = _allShips.Count;


        FillDinstanceCache(0);
    }

    Vector2 RandomPos(Vector2 min, Vector2 max)
    {
        var x = UnityEngine.Random.Range(min.x, max.x);
        var y = UnityEngine.Random.Range(min.y, max.y);

        return new Vector2(x, y);
    }

    public Ship FindClosestShip(Ship ship, float okDistance)
    {
        _latestOkDistance = okDistance;
        // return FindClosestShipBruteForce(ship, okDistance);
        if (!filled)
        {
            filled = true;
            FillDinstanceCache(okDistance);
        }

        var found = _distanceCache[ship.ShipId];

        return _allShips[found];
    }

    Ship FindClosestShipBruteForce(Ship ship, float okDistance)
    {

        var myPostion = ship.Pos;
        var closest = ship;
        float closestDistance = 999999;
        var count = _allShips.Count;
        var okDistanceSqr = okDistance * okDistance;

        for (int i = 0; i < count; ++i)
        {
            if (ship.ShipId == i) continue;

            var target = _allShips[i];

            var distance = SqrDistance(target.Pos, ref myPostion);

            if (distance < closestDistance && distance > okDistanceSqr)
            {
                closestDistance = distance;
                closest = target;
            }
        }

        return closest;
    }

    private void FillDinstanceCache(float okDistance)
    {
        var count = _allShips.Count;

        for (int i = 0; i < count; ++i)
        {
            var me = _allShips[i];
            var shipsClosests = _distanceCache[i];
            var other = FindClosestShipBruteForce(me, okDistance);
            _distanceCache[me.ShipId] = other.ShipId;
            _distanceCache[other.ShipId] = me.ShipId;
        }
    }

    private void LateUpdate()
    {
        filled = false;
        // _distanceCache.Clear();
        //FillDinstanceCache(_latestOkDistance);
    }

    float SqrDistance(Vector2 a, ref Vector2 b)
    {
        var x = a.x - b.x;
        var y = a.y - b.y;

        return x * x + y * y;
    }
}
