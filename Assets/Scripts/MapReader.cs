using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


class MapReader : MonoBehaviour
{

    public string maxlat;
    public string minlat;
    public string maxlon;
    public string minlon;

    public TerrainData terrainData;

    public TerrainLayer grassLayer;


    [HideInInspector]
    public Dictionary<ulong, OsmNode> nodes;

    [HideInInspector]
    public OsmBound bounds;

    [HideInInspector]
    public List<OsmWay> ways;

    [HideInInspector]
    public List<OsmRelation> relations;


    public bool isReady { get; private set; }

    public OsmHttpRequest OsmHttpRequest = new OsmHttpRequest();

    void Start()
    {
        nodes = new Dictionary<ulong, OsmNode>();
        ways = new List<OsmWay>();
        relations = new List<OsmRelation>();

        XmlDocument doc = OsmHttpRequest.HttpGetOsm(maxlat, minlat, maxlon, minlon);

        SetBounds(doc.SelectSingleNode("/osm/bounds"));
        SetNodes(doc.SelectNodes("/osm/node"));
        GetWays(doc.SelectNodes("/osm/way"));
        GetRelation(doc.SelectNodes("/osm/relation"));

        isReady = true;

        init_setting();
    }



    void Update()
    {
        foreach(OsmWay w in ways)
        {
            if (w.visible)
            {
                Color c = Color.cyan;
                if (!w.isBoundary) c = Color.red;

                for (int i = 1; i<w.nodeIDs.Count; i++)
                {
                    OsmNode p1 = nodes[w.nodeIDs[i - 1]];
                    OsmNode p2 = nodes[w.nodeIDs[i]];

                    Vector3 v1 = p1 - bounds.centre;
                    Vector3 v2 = p2 - bounds.centre;

                    Debug.DrawLine(v1, v2, c);
                }
            }
        }

        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            Debug.Log(gameObject.name + " here ");
            Debug.Log("mouse down");
        }

    }


    private void create_terrain()
    {
        GameObject TerrainObj = new GameObject("TerrainObj");
        
        TerrainCollider _TerrainCollider = TerrainObj.AddComponent<TerrainCollider>();
        Terrain _Terrain2 = TerrainObj.AddComponent<Terrain>();

        
        _TerrainCollider.terrainData = terrainData;
        _Terrain2.terrainData = terrainData;
        _Terrain2.transform.position = new Vector3(0 - bounds.size.x / 2, (float)-0.01, 0 - bounds.size.z / 2);
        _Terrain2.terrainData.size = bounds.size;
        this.AddTerrainLayer(terrainData, grassLayer);
    }

    void AddTerrainLayer(TerrainData terrainData, TerrainLayer inputLayer)
    {
        if (inputLayer == null)
            return;

        var layers = terrainData.terrainLayers;
        for (var idx = 0; idx < layers.Length; ++idx)
        {
            if (layers[idx] == inputLayer)
                return;
        }

        int newIndex = layers.Length;
        var newarray = new TerrainLayer[newIndex + 1];
        System.Array.Copy(layers, 0, newarray, 0, newIndex);
        newarray[newIndex] = inputLayer;

        terrainData.terrainLayers = newarray;
    }
    private void init_setting()
    {
        create_terrain();
    }

    void GetWays(XmlNodeList xmlNodeList)
    {
        foreach(XmlNode node in xmlNodeList)
        {
            OsmWay way = new OsmWay(node);
            ways.Add(way);
        }
    }

    void SetNodes(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode n in xmlNodeList)
        {
            OsmNode node = new OsmNode(n);
            nodes[node.ID] = node;

        }
    }

    void SetBounds(XmlNode xmlNode)
    {
        bounds = new OsmBound(xmlNode);
    }

    void GetRelation(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode node in xmlNodeList)
        {
            OsmRelation rel = new OsmRelation(node);
            relations.Add(rel);

            if (rel.type == "multipolygon")
            {
                OsmWay item = ways.Find(x => x.ID == rel.members[0].nodeID);
                item.Height = rel.Height;
                item.isBuilding = true;
                item.name = rel.name;
            }
        }
    }

}
