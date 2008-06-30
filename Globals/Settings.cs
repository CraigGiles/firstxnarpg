using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using Helper;
using Microsoft.Xna.Framework.Input;

namespace ActionRPG
{
    public class Settings
    {
        
        #region Key Bindings

        /// <summary>
        /// Default key = s
        /// </summary>
        public Keys MoveDown
        {
            get { return down; }
            set { down = value; }
        }
        Keys down = Keys.S;

        /// <summary>
        /// Default key = A
        /// </summary>
        public Keys MoveLeft
        {
            get { return left; }
            set { left = value; }
        }
        Keys left = Keys.A;

        /// <summary>
        /// Default key = W
        /// </summary>
        public Keys MoveUp
        {
            get { return up; }
            set { up = value; }
        }
        Keys up = Keys.W;

        /// <summary>
        /// Default key = D
        /// </summary>
        public Keys MoveRight
        {
            get { return right; }
            set { right = value; }
        }
        Keys right = Keys.D;



        /// <summary>
        /// Default key = 1
        /// </summary>
        public Keys KeyboardAttack
        {
            get { return keyboardAttack; }
            set { keyboardAttack = value; }
        }
        Keys keyboardAttack = Keys.NumPad1;

        public Buttons GamePadAttack
        {
            get { return GamePadX; }
        }
        Buttons GamePadX = Buttons.X;


        /// <summary>
        /// Default key = 4
        /// </summary>
        public Keys Inventory
        {
            get { return inventory; }
            set { inventory = value; }
        }
        Keys inventory = Keys.NumPad4;

        public Buttons GamePadInventory
        {
            get { return GamePadY; }
        }
        Buttons GamePadY = Buttons.Y;

        /// <summary>
        /// Default key = 2
        /// </summary>
        public Keys Okay
        {
            get { return okay; }
            set { okay = value; }
        }
        Keys okay = Keys.NumPad2;

        public Buttons GamePadOkay
        {
            get { return GamePadA; }
        }
        Buttons GamePadA = Buttons.A;

        /// <summary>
        /// Default key = 5
        /// </summary>
        public Keys Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
        Keys cancel = Keys.NumPad5;

        public Buttons GamePadCancel
        {
            get { return GamePadB; }
        }
        Buttons GamePadB = Buttons.B;
        #endregion


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
