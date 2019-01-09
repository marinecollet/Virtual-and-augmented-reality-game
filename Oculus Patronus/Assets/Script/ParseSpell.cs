using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{
    [XmlRoot(ElementName = "shot")]
    public class Shot
    {
        [XmlElement(ElementName = "collider")]
        public List<string> Collider { get; set; }
    }

    [XmlRoot(ElementName = "holohomora")]
    public class Holohomora
    {
        [XmlElement(ElementName = "collider")]
        public List<string> Collider { get; set; }
    }

    [XmlRoot(ElementName = "lave")]
    public class Lave
    {
        [XmlElement(ElementName = "collider")]
        public List<string> Collider { get; set; }
    }

    [XmlRoot(ElementName = "protego")]
    public class Protego
    {
        [XmlElement(ElementName = "collider")]
        public List<string> Collider { get; set; }
    }

    [XmlRoot(ElementName = "spellList")]
    public class SpellList
    {
        [XmlElement(ElementName = "shot")]
        public Shot Shot { get; set; }
        [XmlElement(ElementName = "holohomora")]
        public Holohomora Holohomora { get; set; }
        [XmlElement(ElementName = "lave")]
        public Lave Lave { get; set; }
        [XmlElement(ElementName = "protego")]
        public Protego Protego { get; set; }
    }

}