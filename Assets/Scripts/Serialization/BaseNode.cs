using UnityEngine;
using UnityEditor;
using System.Xml;
using System;

class BaseNode
{
    protected T GetAttribute<T>(string attrName, XmlAttributeCollection attributes)
    {
        string strValue = attributes[attrName].Value;
        return (T)Convert.ChangeType(strValue, typeof(T));
    }
}
