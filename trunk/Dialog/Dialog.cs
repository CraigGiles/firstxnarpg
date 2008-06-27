using System;
using System.Collections.Generic;
using System.Text;

namespace ActionRPG
{
    public class Dialog
    {
        List<string> npcTextLine = new List<string>();
        int currentText = 0;

        public bool IsDialogComplete
        {
            get { return currentText >= npcTextLine.Count - 1;}
        }

        public string GetNextText()
        {
            if (!IsDialogComplete)
                currentText++;

            return npcTextLine[currentText];
        }
    }
}
