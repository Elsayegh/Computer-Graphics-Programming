using System;
using System.Drawing;
using System.Windows.Forms;

namespace GrafPack
{
    class Circle : Shape
    {

        

        public Circle(Point keyPt, Point oppPt)
        {
            this.keyPt = keyPt;
            this.oppPt = oppPt;
        }
        //draw the circle based on calculating the radius
        public void draw(Graphics g, Pen blackPen)
        {
            float radius = oppPt.X - keyPt.X;
            g.DrawEllipse(blackPen, keyPt.X - radius, keyPt.Y - radius,
                         radius + radius, radius + radius);
        }

        //fill the circle with red when it's selected
        public void fillCircle(Graphics g, Brush redBrush)
        {
            float radius = oppPt.X - keyPt.X;
            g.FillEllipse(redBrush, keyPt.X - radius, keyPt.Y - radius,
                         radius + radius, radius + radius);
        }
    }
}
