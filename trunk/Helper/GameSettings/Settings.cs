using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using Helper;

namespace ActionRPG
{
    public class Settings
    {
        #region Save Settings


        /// <summary>
        /// Writes the Config.cfg file to users content folder
        /// </summary>
        public static void SaveSettings()
        {
            //XML Document
            XmlDocument doc = new XmlDocument();

            //create the <Config> element
            XmlElement config = doc.CreateElement("Config");
            doc.AppendChild(config);

            //write FPS settings
            WriteFpsSetting(doc, config);

            //save the XML document
            doc.Save(@"Content\Settings\Config.xml");
        }

        /// <summary>
        /// Writes the Show FPS Counter setting in the config.xml
        /// </summary>
        /// <param name="doc">XmlDocument</param>
        /// <param name="config">Config XmlElement</param>
        private static void WriteFpsSetting(XmlDocument doc, XmlElement config)
        {
            //Create the FPS element
            XmlElement fps = doc.CreateElement("FPS");

            //create the "Show FPS" attribute
            XmlAttribute attr = doc.CreateAttribute("Show");

            //set the attributes value and append
            attr.Value = FPSCounter.DrawFramesPerSecond.ToString();
            fps.Attributes.Append(attr);

            //finalize the FPS portion of config
            config.AppendChild(fps);
        }


        #endregion

        #region Load Settings


        /// <summary>
        /// Loads the Config.cfg from the users content folder
        /// </summary>
        public static void LoadSettings()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Content\Settings\Config.xml");

            foreach (XmlNode node in doc.ChildNodes)
            {
                foreach (XmlNode n in node.ChildNodes)
                {
                    LoadFps(n);

                }
            }

        }

        private static void LoadFps(XmlNode n)
        {
            if (n.Name == "FPS")
                FPSCounter.DrawFramesPerSecond = bool.Parse(n.Attributes["Show"].Value);
        }

        #endregion

    }
}
