using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafPack
{
    //this enum to set the shapes into int values 
    public enum ShapeTypes {
        Circle=1,
        Triangle=2,
        Square=3
    }
    abstract class Shape
    {


        public Point keyPt, oppPt;      // these points identify opposite corners of the square
        // This is the base class for Shapes in the application. It should allow an array or LL
        // to be created containing different kinds of shapes.
        public Shape()   // constructor
        {
        }

        //set and get to shapes
        public int ShapeType { get; set; }

        //boolean for shape if selected
        public bool Selected { get; internal set; }

    }
}
