using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class RoadConstructor : GameObjConstructor
{
    public Material roadMaterial;

    IEnumerator Start()
    {
        map = GetComponent<MapReader>();
        while (!map.isReady)
        {
            yield return null;
        }

        List<OsmWay> roadData = map.ways.FindAll((w) =>
        {
            return w.isRoad;
        });

        foreach (var way in roadData)
        {
            GameObject go = new GameObject();
            Vector3 localOrigin = GetCentre(way);
            go.transform.position = localOrigin - map.bounds.centre;

            MeshFilter mf = go.AddComponent<MeshFilter>();
            MeshRenderer mr = go.AddComponent<MeshRenderer>();

            mr.material = roadMaterial;

            List<Vector3> roadVec = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> indx = new List<int>();

            for (int i = 1; i < way.nodeIDs.Count; i++)
            {
                OsmNode p1 = map.nodes[way.nodeIDs[i - 1]];
                OsmNode p2 = map.nodes[way.nodeIDs[i]];

                Vector3 s1 = p1 - localOrigin;
                Vector3 s2 = p2 - localOrigin;

                Vector3 diff = (s2 - s1).normalized;
                var cross = Vector3.Cross(diff, Vector3.up) * 2.0f;

                Vector3 v1 = s1 + cross;
                Vector3 v2 = s1 - cross;
                Vector3 v3 = s2 + cross;
                Vector3 v4 = s2 - cross;

                roadVec.Add(v1);
                roadVec.Add(v2);
                roadVec.Add(v3);
                roadVec.Add(v4);

                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);

                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(1, 0));
                uvs.Add(new Vector2(0, 1));
                uvs.Add(new Vector2(1, 1));

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

            mf.mesh.vertices = roadVec.ToArray();
            mf.mesh.normals = normals.ToArray();
            mf.mesh.triangles = indx.ToArray();
            mf.mesh.uv = uvs.ToArray();

            yield return null;
        }

    }
}

