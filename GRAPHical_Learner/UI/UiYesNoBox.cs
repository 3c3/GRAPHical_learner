using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRAPHical_Learner
{
    public class UiYesNoBox : UiPanel
    {
        private static int width = 120;

        UiLabel caption, message;
        UiButton btnYes, btnNo;

        public UiYesNoBox(string message, string caption)
        {
            children = new List<UiComponent>();
            backgroundColor = GraphicScheme.uiBackgroundColor;

            Width = width;

            this.caption = new UiLabel(caption, GraphicScheme.font1);
            this.message = new UiLabel(message, GraphicScheme.font1);

            this.caption.X = 3;
            this.caption.Y = 3;
            this.message.X = 3;
            this.message.Y = 6 + this.caption.Height;

            AddChild(this.caption);
            AddChild(this.message);

            btnYes = new UiButton("Да", 20);
            btnNo = new UiButton("Не", 20);

            btnYes.X = 3;
            btnYes.Y = this.message.Y + this.message.Height + 3;

            btnNo.X = btnYes.Width + 6;
            btnNo.Y = btnYes.Y;

            AddChild(btnYes);
            AddChild(btnNo);

            Height = btnYes.Y + btnNo.Height + 3;          
        }
    }
}
