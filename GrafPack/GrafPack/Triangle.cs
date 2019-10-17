using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GrafPack
{

    // Details of Triangle
    class Triangle : Shape
    {

        public Triangle(Point keyPt, Point oppPt)
        {
            this.keyPt = keyPt;
            this.oppPt = oppPt;
        }

        //calculate the points x and y user need to draw the shape between
        public void draw(Graphics g, Pen blackPen)
        {
            double xDiff, yDiff, xMid, yMid;

            xDiff = oppPt.X - keyPt.X;
            yDiff = oppPt.Y - keyPt.Y;
            xMid = (oppPt.X + keyPt.X) / 2;
            yMid = (oppPt.Y + keyPt.Y) / 2;

            // draw triangle
            g.DrawLine(blackPen, (int)keyPt.X, (int)keyPt.Y, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
            g.DrawLine(blackPen, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2), (int)oppPt.X, (int)oppPt.Y);
            g.DrawLine(blackPen, (int)keyPt.X, (int)keyPt.Y, oppPt.X, oppPt.Y);

        }

        public void fillTriangle(Graphics g, Brush redBrush)
        {
            float xDiff = oppPt.X - keyPt.X;
            float yDiff = oppPt.Y - keyPt.Y;
            float xMid = (oppPt.X + keyPt.X) / 2;
            float yMid = (oppPt.Y + keyPt.Y) / 2;

            var path = new GraphicsPath();
            path.AddLines(new PointF[] {
            keyPt,
            new PointF(xMid + yDiff/2, yMid-xDiff/2),
            oppPt,
            });
            path.CloseFigure();

            // Fill Triangle
            g.FillPath(redBrush, path);

        }
    }
}
