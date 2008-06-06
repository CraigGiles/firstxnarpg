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
        }

        private void Load(string asset)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"Content/Items/Accessory/" + asset + ".xml");

            foreach (XmlNode root in doc.ChildNodes)
            {                
                //Put any accessory specific load data here
            }

            base.Load(doc);
        }

        #endregion

    }
}
