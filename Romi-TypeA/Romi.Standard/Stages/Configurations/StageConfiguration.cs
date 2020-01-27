using Microsoft.Xna.Framework;
using Romi.Standard.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Romi.Standard.Stages.Configurations
{
    using StageConfigurationImageCollection = List<StageConfigurationImage>;

    [XmlRoot("Map")]
    public class StageConfiguration
    {

        public static StageConfiguration Load(int id)
        {
            using (var s = TitleContainer.OpenStream(Path.Combine(ContentLoaderMan.RootDirectory, $"Maps/{id:D09}.xml")))
            {
                var serializer = new XmlSerializer(typeof(StageConfiguration));
                return (StageConfiguration)serializer.Deserialize(s);
            }
        }

        [XmlElement("Info")]
        public StageConfigurationInfo Info { get; set; }

        [XmlArray("Back")]
        [XmlArrayItem("Image")]
        public StageConfigurationImageCollection Back { get; set; } = new StageConfigurationImageCollection();
    }

    public class StageConfigurationInfo
    {
        [XmlElement("BGM")]
        public string BGM { get; set; }
    }

    public class StageConfigurationImage
    {
        [XmlAttribute("Path")]
        public string Path { get; set; }

        [XmlAttribute("X")]
        public int X { get; set; }

        [XmlAttribute("Y")]
        public int Y { get; set; }

        [XmlAttribute("ScaleX")]
        public float ScaleX { get; set; } = 1.0f;

        [XmlAttribute("ScaleY")]
        public float ScaleY { get; set; } = 1.0f;

        [XmlIgnore]
        public Point Position => new Point(X, Y);
    }
}
