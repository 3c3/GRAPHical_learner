using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public class UiPropertyPanel : UiPanel
    {
        private static int maxProperties = 10;
        private static int minWidth = 120;

        private PropertyHolder holder;

        public PropertyHolder Holder
        {
            set 
            {
                if (value == holder) return;
                else
                {
                    holder = value;
                    OnHolderChanged();
                }
            }
        }

        private UiLabel titleLabel;
        private UiLabel[] propertyLabels = new UiLabel[maxProperties];
        private int propertyCount = 0;
        
        public UiPropertyPanel()
        {
            Width = minWidth;

            titleLabel = new UiLabel("null", GraphicScheme.font1);
            titleLabel.X = 3;
            titleLabel.Y = 3;

            children = new List<UiComponent>();
            AddChild(titleLabel);

            Height = titleLabel.Height + 6;

            for(int i = 0; i < maxProperties; i++)
            {
                propertyLabels[i] = new UiLabel();
                propertyLabels[i].font = GraphicScheme.font1;
                propertyLabels[i].X = 3;
                propertyLabels[i].visible = false;
                AddChild(propertyLabels[i]);
            }
        }

        private void OnHolderChanged()
        {
            if(holder != null)
            {
                titleLabel.Text = holder.GetName();
                int y = titleLabel.Height + 6;

                int idx = 0;

                foreach(Property p in holder.properties)
                {
                    propertyLabels[idx].Text = String.Format("{0}: {1}", p.Name, p.Value);
                    propertyLabels[idx].Y = y;
                    y += propertyLabels[idx].Height + 3;
                    propertyLabels[idx].visible = true;
                    idx++;
                }

                for (; idx < maxProperties; idx++) propertyLabels[idx].visible = false;

                Height = y;
            } 
            else
            {
                titleLabel.Text = "null";
                Height = titleLabel.Height + 6;
                for (int i = 0; i < maxProperties; i++) propertyLabels[i].visible = false;
            }
        }

        public override void Draw(int relX, int relY)
        {
            int idx = 0;

            int w = minWidth;

            if(holder != null) foreach (Property p in holder.properties)
            {
                propertyLabels[idx].Text = String.Format("{0}: {1}", p.Name, p.Value);
                if (propertyLabels[idx].Width + 6> w) w = propertyLabels[idx].Width + 6;
                idx++;
            }

            Width = w;
            base.Draw(relX, relY);
        }
    }
}
