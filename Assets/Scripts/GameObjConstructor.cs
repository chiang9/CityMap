using UnityEngine;

[RequireComponent(typeof(MapReader))]
abstract class GameObjConstructor : MonoBehaviour
{
    protected MapReader map;

    private void Awake()
    {
        map = GetComponent<MapReader>();
    }

    protected Vector3 GetCentre(OsmWay way)
    {
        Vector3 res = Vector3.zero;
        float wayCount = way.nodeIDs.Count;

        foreach (var id in way.nodeIDs)
        {
            res += map.nodes[id];
        }
        return res / wayCount;
    }

}

