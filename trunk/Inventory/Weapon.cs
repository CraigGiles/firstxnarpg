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
                        delay = int.Parse(node.InnerText);
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
        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }
        int delay;
        
    }
}
