using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class BuildingConstructor : GameObjConstructor
{

    public Material buildingMat;

    IEnumerator Start()
    {
        map = GetComponent<MapReader>();
        while (!map.isReady)
        {
            yield return null;
        }

        List<OsmWay> buildingData = map.ways.FindAll((w) =>
        {
            return w.isBuilding && w.nodeIDs.Count > 1;
        });

        foreach(var way in buildingData)
        {
            GameObject go = new GameObject(way.ID.ToString());
            Vector3 localOrigin = GetCentre(way);
            go.transform.position = localOrigin - map.bounds.centre;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();

            mr.material = buildingMat;

            List<Vector3> buildingVec = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indx = new List<int>();


            for (int i = 1; i < way.nodeIDs.Count; i++)
            {
                OsmNode p1 = map.nodes[way.nodeIDs[i - 1]];
                OsmNode p2 = map.nodes[way.nodeIDs[i]];

                Vector3 v1 = p1 - localOrigin;
                Vector3 v2 = p2 - localOrigin;
                Vector3 v3 = v1 + new Vector3(0, way.Height, 0);
                Vector3 v4 = v2 + new Vector3(0, way.Height, 0);

                buildingVec.Add(v1);
                buildingVec.Add(v2);
                buildingVec.Add(v3);
                buildingVec.Add(v4);

                normals.Add(-Vector3.forward);
                normals.Add(-Vector3.forward);
                normals.Add(-Vector3.forward);
                normals.Add(-Vector3.forward);

                int idx4 = 4 * i - 1;
                int idx3 = 4 * i - 2;
                int idx2 = 4 * i - 3;
                int idx1 = 4 * i - 4;

                
                indx.Add(idx1);
                indx.Add(idx3);
                indx.Add(idx2);

                indx.Add(idx3);
                indx.Add(idx4);
                indx.Add(idx2);

                indx.Add(idx2);
                indx.Add(idx3);
                indx.Add(idx1);

                indx.Add(idx2);
                indx.Add(idx4);
                indx.Add(idx3);
                

            }

            mf.mesh.vertices = buildingVec.ToArray();
            mf.mesh.normals = normals.ToArray();
            mf.mesh.triangles = indx.ToArray();

            yield return null;

        }
    }



}

