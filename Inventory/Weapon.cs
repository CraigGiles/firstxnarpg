using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ActionRPG
{
    public class Weapon : Gear
    {

        #region Constructor(s)


        public Weapon(string asset)
        {
            Load(asset);
        }

        public Weapon(string asset, int percent)
        {
            Load(asset);
            this.Percent = percent;
        }

        private void Load(string asset)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Content/Items/Weapon/" + asset + ".xml");

            foreach (XmlNode root in doc.ChildNodes)
            {
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name == "Damage")
                        damage = int.Parse(node.InnerText);

                    if (node.Name == "Delay")
                        delay = (float.Parse(node.InnerText) / 100);

                    if (node.Name == "Range")
                        range = int.Parse(node.InnerText);

                    if (node.Name == "MaxCombo")
                        maxCombo = int.Parse(node.InnerText);
                }
            }
            
            base.Load(doc);
        }


        #endregion


        /// <summary>
        /// Weapon damage
        /// </summary>
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        int damage;


        /// <summary>
        /// Delay between attacks
        /// </summary>
        public float Delay
        {
            get { return delay; }
            set { delay = value; }
        }
        float delay;


        /// <summary>
        /// Max Combo of the weapon
        /// </summary>
        public int MaxCombo
        {
            get { return maxCombo; }
            set { maxCombo = value; }
        }
        int maxCombo;

        /// <summary>
        /// Range of the weapon
        /// </summary>
        public int Range
        {
            get { return range; }
            set { range = value; }
        }
        int range;


    }
}
