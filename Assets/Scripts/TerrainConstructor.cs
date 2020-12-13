using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class TerrainConstructor : GameObjConstructor
{
    public Material cityMaterial;
    public Material grassMaterial;

    IEnumerator Start()
    {
        map = GetComponent<MapReader>();
        while (!map.isReady)
        {
            yield return null;
        }

        GameObject mainTerrain = GenerateMainTerrain();

        List<OsmWay> parkData = map.ways.FindAll((w) =>
        {
            return w.isPark;
        });

        foreach (var way in parkData)
        {
            GameObject go = new GameObject(way.name.ToString());
            Vector3 localOrigin = GetCentre(way);
            go.transform.position = localOrigin - map.bounds.centre;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            MeshCollider mc = go.AddComponent<MeshCollider>();

            mr.material = grassMaterial;

            List<Vector3> parkVec = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> indx = new List<int>();
            List<Vector2> uvValue = new List<Vector2> { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };

            for (int i = 0; i < way.nodeIDs.Count; i++)
            {
                Vector3 v1 = map.nodes[way.nodeIDs[i]] - localOrigin - new Vector3(0,(float)0.005,0);
                parkVec.Add(v1);
                uvs.Add(uvValue[i % 4]);
            }

            for (int i = 0; i < parkVec.Count-2; i++)
            {
                indx.Add(i);
                indx.Add(i+1);
                indx.Add(i+2);
            }

            for (int i = 0; i < parkVec.Count/2 + 1; i++)
            {
                if (i + 4 < parkVec.Count)
                {
                    indx.Add(i);
                    indx.Add(i + 2);
                    indx.Add(i + 4);
                }
            }
            

            
            mf.mesh.vertices = parkVec.ToArray();
            mf.mesh.triangles = indx.ToArray();
            mf.mesh.uv = uvs.ToArray();

            mf.mesh.RecalculateNormals();
            mc.sharedMesh = mf.mesh;

            go.transform.parent = mainTerrain.transform;

            yield return null;
        }

    }

    private GameObject GenerateMainTerrain()
    {
        OsmBound boundData = map.bounds;
        List<Vector3> margin = new List<Vector3>();

        margin.Add(new Vector3(0 - boundData.size.x / 2, (float)-0.01, 0 - boundData.size.z / 2));
        margin.Add(new Vector3(0 + boundData.size.x / 2, (float)-0.01, 0 - boundData.size.z / 2));
        margin.Add(new Vector3(0 - boundData.size.x / 2, (float)-0.01, 0 + boundData.size.z / 2));
        margin.Add(new Vector3(0 + boundData.size.x / 2, (float)-0.01, 0 + boundData.size.z / 2));

        GameObject go = new GameObject("TerrainMain");
        go.transform.position = new Vector3(0, 0, 0);

        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        MeshCollider mc = go.AddComponent<MeshCollider>();

        mr.material = cityMaterial;
        mr.material.mainTextureScale = new Vector2(boundData.size.x, boundData.size.z);

        List<Vector3> terVec = margin;
        List<int> indx = new List<int> { 0, 3, 1, 0, 2, 3 };


        Vector2[] uv = new Vector2[4]{
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(1, 0),
            new Vector2(0, 0)
        };

        mf.mesh.vertices = terVec.ToArray();
        mf.mesh.triangles = indx.ToArray();
        mf.mesh.uv = uv;

        mf.mesh.RecalculateNormals();
        mc.sharedMesh = mf.mesh;
        return go;
    }
}

