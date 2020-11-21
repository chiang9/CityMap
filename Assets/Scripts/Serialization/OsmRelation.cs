using System;
using System.Collections.Generic;
using System.Xml;

class OsmRelation : BaseNode
{
    /*
 <relation id = "6095459" visible="true" version="2" changeset="55619395" timestamp="2018-01-21T06:11:23Z" user="leohoyee" uid="7470237">
      <member type = "way" ref="154906749" role="outer"/>
      <member type = "way" ref="406923868" role="inner"/>
      <tag k = "addr:city" v="los Angeles"/>
      <tag k = "addr:housenumber" v="3650"/>
      <tag k = "addr:postcode" v="90007"/>
      <tag k = "addr:state" v="CA"/>
      <tag k = "addr:street" v="Watt Way"/>
      <tag k = "building" v="university"/>
      <tag k = "building:levels" v="3"/>
      <tag k = "name" v="Physical Education Building"/>
      <tag k = "type" v="multipolygon"/>
 </relation>
    */
    public static float levelHightRatio = 3.0f;
    public static float heightRatio = 0.3048f;

    public ulong ID { get; private set; }

    public bool visible { get; private set; }

    public List<RelationMember> members { get; private set; }

    public float Height { get; private set; }

    public string name { get; private set; }

    public string type { get; private set; }

    public OsmRelation(XmlNode node)
    {
        ID = GetAttribute<ulong>("ref", node.Attributes);
        visible = GetAttribute<bool>("visible", node.Attributes);
        Height = 3.0f;

        XmlNodeList mem = node.SelectNodes("member");
        XmlNodeList tags = node.SelectNodes("tag");

        foreach (XmlNode m in mem)
        {
            RelationMember rel = new RelationMember(m);
            members.Add(rel);
        }

        foreach (XmlNode tag in tags)
        {
            string key = GetAttribute<string>("k", tag.Attributes);

            if (key == "name")
            {
                name = GetAttribute<string>("v", tag.Attributes);
            } 
            else if (key == "type")
            {
                type = GetAttribute<string>("v", tag.Attributes);
            } 
            else if (key == "building:levels")
            {
                Height = levelHightRatio * GetAttribute<float>("v", tag.Attributes);
            }
            else if (key == "height")
            {
                Height = heightRatio * GetAttribute<float>("v", tag.Attributes);
            }
        }
        
    }
}

class RelationMember :BaseNode
{
    public string type { get; private set; }

    public ulong nodeID { get; private set; }

    public string role { get; private set; }

    public RelationMember(XmlNode node)
    {
        type = GetAttribute<string>("type", node.Attributes);
        nodeID = GetAttribute<ulong>("ref", node.Attributes);
        role = GetAttribute<string>("role", node.Attributes);
    }
}