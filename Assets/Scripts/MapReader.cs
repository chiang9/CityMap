using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MapReader : MonoBehaviour
{
    [Tooltip("The resouce file for OSM data")]
    public string resourceFile;

    void Start()
    {
        var txtAsset = Resources.Load<TextAsset>(resourceFile);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(txtAsset.text);

        SetBounds(doc.SelectSingleNode("/osm/bounds"));
        SetNodes(doc.SelectNodes("/osm/node"));
        GetWays(doc.SelectNodes("/osm/way"));


    }

    private void GetWays(XmlNodeList xmlNodeList)
    {
        throw new NotImplementedException();
    }

    private void SetNodes(XmlNodeList xmlNodeList)
    {
        throw new NotImplementedException();
    }

    private void SetBounds(XmlNode xmlNode)
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
