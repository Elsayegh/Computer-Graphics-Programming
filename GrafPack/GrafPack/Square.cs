using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GrafPack
{
    class Square : Shape
    {
        //This class contains the specific details for a square defined in terms of opposite corners
       
        public Square(Point keyPt, Point oppPt)   // constructor
        {
            this.keyPt = keyPt;
            this.oppPt = oppPt;
        }
        // You will need a different draw method for each kind of shape. Note the square is drawn
        // from first principles. All other shapes should similarly be drawn from first principles. 
        // Ideally no C# standard library class or method should be used to create, draw or transform a shape
        // and instead should utilse user-developed code.
        public void draw(Graphics g, Pen blackPen)
        {
            // This method draws the square by calculating the positions of the other 2 corners
            double xDiff, yDiff, xMid, yMid;   // range and mid points of x & y  
            // calculate ranges and mid points
            xDiff = oppPt.X - keyPt.X;
            yDiff = oppPt.Y - keyPt.Y;
            xMid = (oppPt.X + keyPt.X) / 2;
            yMid = (oppPt.Y + keyPt.Y) / 2;
            // draw square
            g.DrawLine(blackPen, (int)keyPt.X, (int)keyPt.Y, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
            g.DrawLine(blackPen, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2), (int)oppPt.X, (int)oppPt.Y);
            g.DrawLine(blackPen, (int)oppPt.X, (int)oppPt.Y, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));
            g.DrawLine(blackPen, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2), (int)keyPt.X, (int)keyPt.Y);
        }
        public void fillSquare(Graphics g, Brush redBrush)
        {
            float xDiff = oppPt.X - keyPt.X;
            float yDiff = oppPt.Y - keyPt.Y;
            float xMid = (oppPt.X + keyPt.X) / 2;
            float yMid = (oppPt.Y + keyPt.Y) / 2;

            var path = new GraphicsPath();
            path.AddLines(new PointF[] {
            keyPt,
            new PointF(xMid + yDiff/2, yMid-xDiff/2),
            oppPt
            });
          path.AddLines(new PointF[] {
            keyPt,
            new PointF(xMid - yDiff/2, yMid + xDiff/2),
            oppPt
            });
            path.CloseFigure();
            // Fill Triangle
            g.FillPath(redBrush, path);
        }
    }
}
