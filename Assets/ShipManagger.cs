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
    Ship[] _allShips;


    Vector2[] _translationCache;

    bool filled = false;
    void Awake()
    {
        _allShips = new Ship[_numShips];

        var min = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        for (int i = 0; i < _numShips; ++i)
        {
            var pos = RandomPos(min, max);
            var go = Instantiate(_prefab.gameObject, pos, Quaternion.identity);
            var ship = go.GetComponent<Ship>();
            ship.ShipId = i;
            _allShips[i] = ship;

        }

        var count = _allShips.Length;
        _translationCache = new Vector2[count];

        FillDinstanceCache(0);
    }

    Vector2 RandomPos(Vector2 min, Vector2 max)
    {
        var x = UnityEngine.Random.Range(min.x, max.x);
        var y = UnityEngine.Random.Range(min.y, max.y);

        return new Vector2(x, y);
    }

    public void Update()
    {
   
        var count = _allShips.Length;
        float okDistance = _allShips[0]._speed * 1.5f * (1f / 18);

        var okDistanceSqr = okDistance * okDistance;

        for (int i = 0; i < count; ++i)
        {
            var me = _allShips[i];
            var direction = FindAproachingDirection(me, okDistance);
            me.Translate(direction * Time.deltaTime);
        }
    }

    public Vector2 FindAproachingDirection(Ship ship, float okDistance)
    {
       // return ship;
      //  return FindClosestShipBruteForce(ship, okDistance);
        if (!filled)
        {
            filled = true;
            FillDinstanceCache(okDistance);
        }

        var found = _translationCache[ship.ShipId];

        return found;
    }

    Ship FindClosestShipBruteForce(Ship ship, float okDistance)
    {

        var myPostion = ship.Pos;
        var closest = ship;
        float closestDistance = 99999999999;
        var count = _allShips.Length;
        var okDistanceSqr = okDistance * okDistance;

        for (int i = 0; i < count; ++i)
        {
            if (ship.ShipId == i) continue;

            var target = _allShips[i];
            var targetPos = target.Pos;

            var x = targetPos.x - myPostion.x;
            var y = targetPos.y - myPostion.y;

            float distance = x * x + y * y;

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
        var count = _allShips.Length;


       Parallel.For(0, count, (i) => {
    
            var me = _allShips[i];
            var other = FindClosestShipBruteForce(me, okDistance);
           var direction = (other.Pos - me.Pos).normalized * me._speed;
           _translationCache[me.ShipId] = direction;
           
       });

    }

    private void LateUpdate()
    {
      //  if(Time.frameCount%10==0)
       filled = false;
    }

}
