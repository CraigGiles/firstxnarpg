using System;
using System.Collections.Generic;
using System.Text;

namespace ActionRPG
{
    public interface IMindset
    {
        void Standing(Player player);
        void Traveling(Player player);
        void Guarding(Player player);
        void Roaming(Player player);
        void Patroling(Player player);
        void Aggressive(Player player);
        void Fleeing(Player player);
    }
}
