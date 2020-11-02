using System.Xml;
using UnityEngine;

class OsmNode : BaseNode
{
    public ulong ID { get; private set; }

    public float Latitude { get; private set; }

    public float Longitude { get; private set; }

    public float X { get; private set; }

    public float Y { get; private set; }

    public static implicit operator Vector3(OsmNode node)
    {
        return new Vector3(node.X, 0, node.Y);
    }


    public OsmNode(XmlNode node)
    {
        ID = GetAttribute<ulong>("id", node.Attributes);
        Latitude = GetAttribute<float>("lat", node.Attributes);
        Longitude = GetAttribute<float>("lon", node.Attributes);

        X = (float)MercatorProjection.lonToX(Longitude);
        Y = (float)MercatorProjection.latToY(Latitude);
    }

}

