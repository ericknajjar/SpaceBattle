using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class ShipManagger : MonoBehaviour
{
    [SerializeField]
    Ship _prefab;

    [SerializeField]
    int _numShips = 25;

    // Start is called before the first frame update
    List<Ship> _allShips;


    int[] _distanceCache;

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
        _distanceCache = new int[count];

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
       // return ship;
      //  return FindClosestShipBruteForce(ship, okDistance);
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
        float closestDistance = 99999999999;
        var count = _allShips.Count;
        var okDistanceSqr = okDistance * okDistance;

        for (int i = 0; i < count; ++i)
        {
            if (ship.ShipId == i) continue;

            var target = _allShips[i];
            var targetPos = target.Pos;

            var x = targetPos.x - myPostion.x;
            var y = targetPos.y - myPostion.y;

           float distance =  x * x + y * y;

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


       Parallel.For(0, count, (i) => {
    
            var me = _allShips[i];
            var other = FindClosestShipBruteForce(me, okDistance);
            _distanceCache[me.ShipId] = other.ShipId;
           
       });

    }

    private void LateUpdate()
    {
        filled = false;
        // _distanceCache.Clear();
        //FillDinstanceCache(_latestOkDistance);
    }

}
