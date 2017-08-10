using ClashOfTanks.Core.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashOfTanks.GUI
{
    class GUIObject
    {
        public Control Control { get; set; }
        public GameplayElement GameObject { get; set; }

        public GUIObject(Control control, GameplayElement gameplayElement)
        {
            Control = control;
            GameObject = gameplayElement;
        }
    }
}
