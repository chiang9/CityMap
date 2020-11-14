using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

public class OsmHttpRequest
{
    public XmlDocument HttpGetOsm()
    {
        XmlDocument res = new XmlDocument();
        var url = "https://api.openstreetmap.org/api/0.6/map?bbox=-118.28783,34.02063,-118.28147,34.02360";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content.ReadAsStringAsync().Result);

                return doc;
                
            }

        }
        return res;
    }
}

        