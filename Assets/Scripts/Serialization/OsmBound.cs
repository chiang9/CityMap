using System.Xml;
using UnityEngine;


// < bounds minlat = "34.0206300" minlon = "-118.2878300" maxlat = "34.0236000" maxlon = "-118.2814700" />

class OsmBound : BaseNode
{
    public float minlat { get; private set; }

    public float minlon { get; private set; }

    public float maxlat { get; private set; }

    public float maxlon { get; private set; }

    public Vector3 centre { get; private set; }


    public OsmBound(XmlNode node)
    {
        minlat = GetAttribute<float>("minlat", node.Attributes);
        minlon = GetAttribute<float>("minlon", node.Attributes);
        maxlat = GetAttribute<float>("maxlat", node.Attributes);
        maxlon = GetAttribute<float>("maxlon", node.Attributes);

        float x = (float)((MercatorProjection.lonToX(maxlon) + MercatorProjection.lonToX(minlon))) / 2;
        float y = (float)((MercatorProjection.latToY(maxlat) + MercatorProjection.latToY(minlat))) / 2;

        centre = new Vector3(x, 0, y);

    }
}

