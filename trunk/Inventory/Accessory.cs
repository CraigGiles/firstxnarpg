using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ActionRPG
{
    public class Accessory : Gear
    {

        #region Constructor(s)


        public Accessory(string asset)
        {
            Load(asset);
            Slot = EquipmentSlot.Accessory;
        }

        public Accessory(string asset, int percent)
        {
            Load(asset);
            this.Percent = percent;
        }

        private void Load(string asset)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Content/Items/Accessory/" + asset + ".xml");

            foreach (XmlNode root in doc.ChildNodes)
            {
            }

            base.Load(doc);
        }

        #endregion

    }
}
