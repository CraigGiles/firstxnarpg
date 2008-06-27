using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace ActionRPG
{
    public class DialogManager
    {
        public NPC InConversationWith
        {
            get { return inConversationWith; }
            set { inConversationWith = value; }
        }
        NPC inConversationWith;

        public Dialog Conversation
        {
            get { return conversation; }
            set { conversation = value; }
        }
        Dialog conversation;

        public void Update()
        {

        }

        public void Draw()
        {

        }
    }
}
