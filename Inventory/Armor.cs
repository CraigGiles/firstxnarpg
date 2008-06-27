using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ActionRPG
{
    public class Armor : Gear
    {

        #region Constructor(s)


        public Armor(string asset)
        {
            Load(asset);
        }

        public Armor(string asset, int percent)
        {
            Load(asset);
            this.Percent = percent;
        }

        private void Load(string asset)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Content/Items/Armor/" + asset + ".xml");

            foreach (XmlNode root in doc.ChildNodes)
            {
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name == "Defense")
                        defense = int.Parse(node.InnerText);

                    if (node.Name == "Dodge")
                        block = int.Parse(node.InnerText);
                }
            }

            base.Load(doc);
        }


        #endregion


        /// <summary>
        /// Defense added to the user
        /// </summary>
        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }
        int defense;


        /// <summary>
        /// Blocking ability added to the user
        /// </summary>
        public int Block
        {
            get { return block; }
            set { block = value; }
        }
        int block;

    }
}
