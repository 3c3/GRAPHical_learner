using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace GRAPHical_Learner
{
    public class UiVerticalMenu : UiPanel
    {
        public UiVerticalMenu()
        {
            children = new List<UiComponent>();
            backgroundColor = ColorScheme.uiBackgroundColor;
        }

        public void AddItem(String text, ComponentClickedHandler handler)
        {
            UiButton newButton = new UiButton(text, 20);
            newButton.box.Left = 3;
            newButton.box.Top = 5 + 25 * children.Count;
            if (box.Width < (newButton.box.Width + 6))
            {
                box.Width = newButton.box.Width + 6;
                UpdateButtonsWidth();
            }
            else newButton.updateWidth(box.Width - 6);

            AddChild(newButton);
            newButton.ComponentClicked += handler;
            box.Height = 5 + 25 * children.Count;
        }

        private void UpdateButtonsWidth()
        {
            children.ForEach(b => (b as UiButton).updateWidth(box.Width - 6));
        }

        public void RemoveLast()
        {
            children.RemoveAt(children.Count - 1);
            box.Height -= 25;
        }
    }
}
