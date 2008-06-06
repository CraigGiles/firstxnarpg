using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace ActionRPG
{
    public class Item
    {
        #region Data

        /// <summary>
        /// Objects Asset name
        /// </summary>
        public string AssetName
        {
            get { return assetName; }
            set { assetName = value; }
        }
        string assetName;


        /// <summary>
        /// Objects description
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        string description;


        /// <summary>
        /// Objects icon graphic
        /// </summary>
        public Texture2D GraphicIcon
        {
            get { return graphicIcon; }
            set { graphicIcon = value; }
        }
        Texture2D graphicIcon;

        /// <summary>
        /// Objects value in gold
        /// </summary>
        public int GoldValue
        {
            get { return goldValue; }
            set { goldValue = value; }
        }
        int goldValue;

        /// <summary>
        /// Can the object be dropped
        /// </summary>
        public bool Dropable
        {
            get { return dropable; }
            set { dropable = value; }
        }
        bool dropable;

        #endregion


        #region Constructor(s)

        public Item(string asset)
        {
            Load(asset);
        }

        #endregion


        #region Load Item


        protected void Load(string asset)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Content/Items/" + asset + ".xml");

            foreach (XmlNode root in doc.ChildNodes)
            {
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name == "Asset")
                        assetName = node.InnerText;

                    if (node.Name == "Description")
                        description = node.InnerText;

                    if (node.Name == "GraphicIcon")
                        graphicIcon = LoadTexture(node.InnerText);

                    if (node.Name == "GoldValue")
                        goldValue = int.Parse(node.InnerText);

                    if (node.Name == "Dropable")
                        dropable = bool.Parse(node.InnerText);

                }//end child nodes 
            }

        }


        private Texture2D LoadTexture(string asset)
        {
            return Globals.Content.Load<Texture2D>("Graphics/Items/" + asset);
        }


        #endregion

    }
}
