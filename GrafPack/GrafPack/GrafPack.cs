using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Windows.Forms;


namespace GrafPack
{
    public partial class GrafPack : Form
    {

        private MainMenu mainMenu;
        private bool selectSquareStatus = false;
        private bool selectTriangleStatus = false;
        private bool selectCircleStatus = false;

        private int clicknumber = 0;
        //two points to store the mouse clicks when user need to draw shape
        private Point one;
        private Point two;
        private int nextIndex = 0; 
        private int prevIndex = -1;
        private bool isMove = false;
        private bool rotateItem = false;

        //list to store the shapes to help the user select and cycle between the shapes
        List<Shape> shapes = new List<Shape>();
        //private object moveItem;

        public GrafPack()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.White;

            // The following approach uses menu items coupled with mouse clicks
            MainMenu mainMenu = new MainMenu();
            MenuItem createItem = new MenuItem();
            MenuItem selectItem = new MenuItem();
            MenuItem squareItem = new MenuItem();
            MenuItem triangleItem = new MenuItem();
            MenuItem circleItem = new MenuItem();
            MenuItem transformItem = new MenuItem();
            MenuItem moveItem = new MenuItem();
            MenuItem rotateItem = new MenuItem();
            MenuItem deleteItem = new MenuItem();
            MenuItem exitForm = new MenuItem();

            //adding text for each item in the menu
            createItem.Text = "&Create";
            squareItem.Text = "&Square";
            triangleItem.Text = "&Triangle";
            circleItem.Text = "&Circle";
            selectItem.Text = "&Select";
            transformItem.Text = "&Transform";
            moveItem.Text = "&Move";
            rotateItem.Text = "&Rotate";
            deleteItem.Text = "&Delete";
            exitForm.Text = "&Exit";

            //adding the sub and main menus
            mainMenu.MenuItems.Add(createItem);
            mainMenu.MenuItems.Add(selectItem);
            mainMenu.MenuItems.Add(transformItem);
            createItem.MenuItems.Add(squareItem);
            createItem.MenuItems.Add(triangleItem);
            createItem.MenuItems.Add(circleItem);
            transformItem.MenuItems.Add(moveItem);
            transformItem.MenuItems.Add(rotateItem);
            mainMenu.MenuItems.Add(deleteItem);
            mainMenu.MenuItems.Add(exitForm);

            //adding the events needed for each functionality
            selectItem.Click += new System.EventHandler(this.selectShape);
            squareItem.Click += new System.EventHandler(this.selectSquare);
            triangleItem.Click += new System.EventHandler(this.selectTriangle);
            circleItem.Click += new System.EventHandler(selectCircle);
            moveItem.Click += new System.EventHandler(MoveClick);
            rotateItem.Click += new System.EventHandler(RotationClick);
            deleteItem.Click += new System.EventHandler(itemDelete);
            exitForm.Click += new System.EventHandler(exit_form);

            this.Menu = mainMenu;
            this.MouseClick += mouseClick;
            //this.mouseClick += MouseDown;

        }

        //terminate the windows form 
        private void exit_form(object sender, EventArgs e)
        {
            this.Close();
        }

        // Delete the selected item
        private void itemDelete(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            Pen blackpen = new Pen(Color.Black);
            
            //loop over the shapes list and delete the selected shape then redraw the remained shapes
            for (int i = 0; i < shapes.Count; i++)
            {
                if (shapes[i].Selected) //if there is a shape highlighted
                {
                    shapes.Remove(shapes[i]); // remove the shape
                    this.Refresh(); // refresh the form
                }
                else
                {
                    MessageBox.Show("Please select a shape"); // show the message if no highlighted shapes
                }
                for (int j = 0; j < shapes.Count; j++) // loop over the list to redraw the remained shapes depending on it's type.
                {
                    if (shapes[j].ShapeType == (int)ShapeTypes.Square)
                    {
                        ((Square)shapes[j]).draw(g, blackpen);
                    }
                    else if (shapes[j].ShapeType == (int)ShapeTypes.Circle)
                    {
                        ((Circle)shapes[j]).draw(g, blackpen);
                    }

                    else if (shapes[j].ShapeType == (int)ShapeTypes.Triangle)
                    {
                        ((Triangle)shapes[j]).draw(g, blackpen);

                    }
                }
            }

        }

       //this method is for moving the selected shape right and left based on the clicked arrow  if it's right or left.
        private void moveItem(int x)
        {
            SolidBrush redBrush = new SolidBrush(Color.Red);
            GraphicsPath myPath = new GraphicsPath();
            if (isMove)
            {
                for (int i = 0; i < shapes.Count; i++) // loop over the lists shapes if it's not empty 
                {
                    Graphics g = this.CreateGraphics();
                    Pen blackpen = new Pen(Color.Black);
                    if (shapes[i].Selected)  // if shape is highlighted
                    {
                        if (shapes[i].ShapeType == (int)ShapeTypes.Square) //if the highlighted shape is square
                        {                      
                            shapes[i].keyPt = new Point(shapes[i].keyPt.X + x, shapes[i].keyPt.Y);
                            shapes[i].oppPt = new Point(shapes[i].oppPt.X + x, shapes[i].oppPt.Y); 
                            g.Clear(Color.White);
                            ((Square)shapes[i]).draw(g, blackpen);
                            ((Square)shapes[i]).fillSquare(g, redBrush);
                        }
                        else if (shapes[i].ShapeType == (int)ShapeTypes.Circle) //if the highlighted shape is Circle
                        {
                            shapes[i].keyPt = new Point(shapes[i].keyPt.X + x, shapes[i].keyPt.Y);
                            shapes[i].oppPt = new Point(shapes[i].oppPt.X + x, shapes[i].oppPt.Y);
                            g.Clear(Color.White);
                            ((Circle)shapes[i]).draw(g, blackpen);
                            ((Circle)shapes[i]).fillCircle(g, redBrush);
                        }

                        else if (shapes[i].ShapeType == (int)ShapeTypes.Triangle) ////if the highlighted shape is triangle
                        {
                            shapes[i].keyPt = new Point(shapes[i].keyPt.X + x, shapes[i].keyPt.Y);
                            shapes[i].oppPt = new Point(shapes[i].oppPt.X + x, shapes[i].oppPt.Y);
                            g.Clear(Color.White);
                            ((Triangle)shapes[i]).draw(g, blackpen);
                            ((Triangle)shapes[i]).fillTriangle(g, redBrush);
                        }
                    }

                }
            }
        }
        //rotate the shape when it's selected
        private void itemRotation()
        {
            Graphics g = this.CreateGraphics();
            SolidBrush redBrush = new SolidBrush(Color.Red);
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            Pen blackPen = new Pen(Color.Black);
            isMove = false;
            int angle = 10;
            for (int i = 0; i < shapes.Count; i++)
            {
                //calculate the angle and redraw in the new position
                int width = Math.Abs(shapes[i].keyPt.X - shapes[i].oppPt.X);
                int height = Math.Abs(shapes[i].keyPt.Y - shapes[i].oppPt.Y);
                PointF center = new PointF(shapes[i].keyPt.X + (shapes[i].oppPt.X / 2.0f), shapes[i].keyPt.Y + (shapes[i].oppPt.Y / 2.0f));
                //rotate the shape depends on it's type
                if (shapes[i].Selected)
                {

                    if (shapes[i].ShapeType == (int)ShapeTypes.Square)
                    {
                        g.TranslateTransform(this.Width / 2, this.Height / 2);
                        g.RotateTransform(angle);
                        g.TranslateTransform(-this.Width / 2, -this.Height / 2);
                        Square aSquare = new Square(new Point(shapes[i].keyPt.X, shapes[i].keyPt.Y), new Point(shapes[i].oppPt.X, shapes[i].oppPt.Y));
                        aSquare.ShapeType = (int)ShapeTypes.Square;
                        g.Clear(Color.White);
                        aSquare.draw(g, blackPen);
                        ((Square)shapes[i]).fillSquare(g, redBrush);
                    }
                    else if (shapes[i].ShapeType == (int)ShapeTypes.Triangle)
                    {                     
                        g.TranslateTransform(this.Width / 2, this.Height / 2);

                        g.RotateTransform(angle);

                        g.TranslateTransform(-this.Width / 2, -this.Height / 2);

                        Pen blackpen = new Pen(Color.Black);

                        Triangle aTriangle = new Triangle(new Point(shapes[i].keyPt.X, shapes[i].keyPt.Y), new Point(shapes[i].oppPt.X, shapes[i].oppPt.Y));
                        aTriangle.ShapeType = (int)ShapeTypes.Triangle;
                        g.Clear(Color.White);
                        aTriangle.draw(g, blackpen);
                        ((Triangle)shapes[i]).fillTriangle(g, redBrush);

                    }

                    else if (shapes[i].ShapeType == (int)ShapeTypes.Circle)
                    {
                        g.TranslateTransform(this.Width / 2, this.Height / 2);

                        g.RotateTransform(angle);

                        g.TranslateTransform(-this.Width / 2, -this.Height / 2);


                        Circle aCircle = new Circle(new Point(shapes[i].keyPt.X, shapes[i].keyPt.Y), new Point(shapes[i].oppPt.X, shapes[i].oppPt.Y));
                        aCircle.ShapeType = (int)ShapeTypes.Circle;
                        g.Clear(Color.White);
                        aCircle.draw(g, blackPen);
                        ((Circle)shapes[i]).fillCircle(g, redBrush);

                    }
                }
            }
        }
        // Generally, all methods of the form are usually private
        private void selectSquare(object sender, EventArgs e)
        {

            selectSquareStatus = true;
            MessageBox.Show("Click OK and then click once each at two locations to create a square");
        }

        private void selectTriangle(object sender, EventArgs e)
        {
            selectTriangleStatus = true;
            MessageBox.Show("Click Ok and then click once at two locations to create a triangle");
        }

        private void selectCircle(object sender, EventArgs e)
        {
            selectCircleStatus = true;
            MessageBox.Show("Click Ok and then click once at two locations to create a circle");
        }

        private void MoveClick(object sender, EventArgs e)
        {
            isMove = true;
        }

        private void RotationClick(object sender, EventArgs e)
        {
            rotateItem = true;
        }



        private void selectShape(object sender, EventArgs e)
        {
            //this method is to highlight the first shape in the shapes list when the user click select menu and the user can cycle over the shapes after by using the right and left arrow keys
            Graphics g = this.CreateGraphics();
            MessageBox.Show("Please move the arrow left and right to cycle between the shapes");
            SolidBrush redBrush = new SolidBrush(Color.Red);
            GraphicsPath myPath = new GraphicsPath();


            if (shapes.Count > 0)  //if the shapes list is not empty
            {
                shapes[0].Selected = true; 
                switch (shapes[0].ShapeType)
                {
                    case (int)ShapeTypes.Circle:
                        ((Circle)shapes[0]).fillCircle(g, redBrush);
                        break;
                    case (int)ShapeTypes.Triangle:
                        // shapeType = "Triangle";
                        ((Triangle)shapes[0]).fillTriangle(g, redBrush);
                        break;
                    case (int)ShapeTypes.Square:
                        // shapeType = "Square";
                        ((Square)shapes[0]).fillSquare(g, redBrush);
                        break;
                }

            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            //use the key arrow events
            Graphics g = this.CreateGraphics();
            SolidBrush redBrush = new SolidBrush(Color.Red);
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            GraphicsPath myPath = new GraphicsPath();
            Pen blackPen = new Pen(Color.Black);

            if (e.KeyCode == Keys.Left)
            {
                if (!isMove && !rotateItem) //if move and rotate item are false so it's select option
                {
                    // increase the nextIndex value by 1 everytime user click on left arrow
                    nextIndex++;
                    // decrease the nextIndex value by 1 everytime user click on right arrow
                    prevIndex = nextIndex - 1; 

                    if (nextIndex == shapes.Count)
                    {
                        nextIndex = 0;
                    }

                    if (nextIndex == 0)
                    {
                        prevIndex = shapes.Count - 1;
                    }

                    for (int i = 0; i < shapes.Count; i++)
                    {
                        shapes[i].Selected = false;
                    }
                    shapes[nextIndex].Selected = true;

                    switch (shapes[nextIndex].ShapeType)
                    {
                        //if the selected shape is circle, highlight it with red color and fill the remained shapes with white
                        case (int)ShapeTypes.Circle:
                            // nextIndex++;
                            //prevIndex--;
                            ((Circle)shapes[nextIndex]).fillCircle(g, redBrush);
                            if (shapes[prevIndex].ShapeType == (int)ShapeTypes.Circle)
                            {
                                ((Circle)shapes[prevIndex]).fillCircle(g, whiteBrush);
                            }
                            else if (shapes[prevIndex].ShapeType == (int)ShapeTypes.Triangle)
                            {
                                ((Triangle)shapes[prevIndex]).fillTriangle(g, whiteBrush);
                            }
                            else if (shapes[prevIndex].ShapeType == (int)ShapeTypes.Square)
                            {
                                ((Square)shapes[prevIndex]).fillSquare(g, whiteBrush);

                            }
                            break;
                        //if the selected shape is triangle, highlight it with red color and fill the remained shapes with white
                        case (int)ShapeTypes.Triangle:

                            // shapeType = "Triangle";
                            ((Triangle)shapes[nextIndex]).fillTriangle(g, redBrush);
                            if (shapes[prevIndex].ShapeType == (int)ShapeTypes.Circle)
                            {
                                ((Circle)shapes[prevIndex]).fillCircle(g, whiteBrush);
                            }
                            else if (shapes[prevIndex].ShapeType == (int)ShapeTypes.Triangle)
                            {
                                ((Triangle)shapes[prevIndex]).fillTriangle(g, whiteBrush);
                            }
                            else if (shapes[prevIndex].ShapeType == (int)ShapeTypes.Square)
                            {
                                ((Square)shapes[prevIndex]).fillSquare(g, whiteBrush);

                            }

                            break;
                        //if the selected shape is square, highlight it with red color and fill the remained shapes with white
                        case (int)ShapeTypes.Square:

                            // shapeType = "Square";
                            ((Square)shapes[nextIndex]).fillSquare(g, redBrush);
                            if (shapes[prevIndex].ShapeType == (int)ShapeTypes.Circle)
                            {
                                ((Circle)shapes[prevIndex]).fillCircle(g, whiteBrush);
                            }
                            else if (shapes[prevIndex].ShapeType == (int)ShapeTypes.Triangle)
                            {
                                ((Triangle)shapes[prevIndex]).fillTriangle(g, whiteBrush);
                            }
                            else if (shapes[prevIndex].ShapeType == (int)ShapeTypes.Square)
                            {
                                ((Square)shapes[prevIndex]).fillSquare(g, whiteBrush);

                            }

                            break;
                    }
                }
                // if move is true, move the shape to right by 20 px.
                else if (isMove)
                {
                    moveItem(-20);
                }
                //rotates the shape if rotate item variable is true
                else if (rotateItem)
                {
                    //rotateItem = true;
                    itemRotation();
                }
            }
            if (e.KeyCode == Keys.Right)
            {
                if (!isMove)
                {
                    prevIndex++;
                    nextIndex = nextIndex - 1;
                    if (prevIndex == shapes.Count)
                    {
                        prevIndex = 0;
                    }

                    if (prevIndex == 0)
                    {
                        nextIndex = shapes.Count - 1;
                    }
                    for (int i = 0; i < shapes.Count; i++)
                    {
                        shapes[i].Selected = false;
                    }
                    shapes[prevIndex].Selected = true;

                    switch (shapes[prevIndex].ShapeType)
                    {
                        case (int)ShapeTypes.Circle:
                            // nextIndex++;
                            //prevIndex--;
                            ((Circle)shapes[prevIndex]).fillCircle(g, redBrush);
                            if (shapes[nextIndex].ShapeType == (int)ShapeTypes.Circle)
                            {
                                ((Circle)shapes[nextIndex]).fillCircle(g, whiteBrush);
                            }
                            else if (shapes[nextIndex].ShapeType == (int)ShapeTypes.Triangle)
                            {
                                ((Triangle)shapes[nextIndex]).fillTriangle(g, whiteBrush);
                            }
                            else if (shapes[nextIndex].ShapeType == (int)ShapeTypes.Square)
                            {
                                ((Square)shapes[nextIndex]).fillSquare(g, whiteBrush);

                            }
                            break;
                        case (int)ShapeTypes.Triangle:

                            // shapeType = "Triangle";
                            ((Triangle)shapes[prevIndex]).fillTriangle(g, redBrush);
                            if (shapes[nextIndex].ShapeType == (int)ShapeTypes.Circle)
                            {
                                ((Circle)shapes[nextIndex]).fillCircle(g, whiteBrush);
                            }
                            else if (shapes[nextIndex].ShapeType == (int)ShapeTypes.Triangle)
                            {
                                ((Triangle)shapes[nextIndex]).fillTriangle(g, whiteBrush);
                            }
                            else if (shapes[nextIndex].ShapeType == (int)ShapeTypes.Square)
                            {
                                ((Square)shapes[nextIndex]).fillSquare(g, whiteBrush);
                            }

                            break;
                        case (int)ShapeTypes.Square:

                            // shapeType = "Square";
                            ((Square)shapes[prevIndex]).fillSquare(g, redBrush);
                            if (shapes[nextIndex].ShapeType == (int)ShapeTypes.Circle)
                            {
                                ((Circle)shapes[nextIndex]).fillCircle(g, whiteBrush);
                            }
                            else if (shapes[nextIndex].ShapeType == (int)ShapeTypes.Triangle)
                            {
                                ((Triangle)shapes[nextIndex]).fillTriangle(g, whiteBrush);
                            }
                            else if (shapes[nextIndex].ShapeType == (int)ShapeTypes.Square)
                            {
                                ((Square)shapes[nextIndex]).fillSquare(g, whiteBrush);

                            }

                            break;
                    }
                }
                else
                {
                    moveItem(20);
                }
            }
        }

        // This method is quite important and detects all mouse clicks - other methods may need
        // to be implemented to detect other kinds of event handling eg keyboard presses.
        private void mouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // 'if' statements can distinguish different selected menu operations to implement.
                // There may be other (better, more efficient) approaches to event handling,
                // but this approach works.
                if (selectSquareStatus == true)
                {
                    if (clicknumber == 0)
                    {
                        one = new Point(e.X, e.Y);
                        clicknumber = 1;
                    }
                    else
                    {
                        two = new Point(e.X, e.Y);
                        clicknumber = 0;
                        selectSquareStatus = false;

                        Graphics g = this.CreateGraphics();
                        Pen blackpen = new Pen(Color.Black);

                        Square aSquare = new Square(one, two);
                        aSquare.ShapeType = (int)ShapeTypes.Square;

                        aSquare.draw(g, blackpen);

                        //add the square to the shapes list
                        shapes.Add(aSquare);


                    }
                }

                //If Triangle Selected
                else if (selectTriangleStatus == true)
                {
                    if (clicknumber == 0)
                    {
                        one = new Point(e.X, e.Y);
                        clicknumber = 1;
                    }
                    else
                    {
                        two = new Point(e.X, e.Y);
                        clicknumber = 0;
                        selectTriangleStatus = false;

                        Graphics g = this.CreateGraphics();
                        Pen blackpen = new Pen(Color.Black);

                        Triangle aTriangle = new Triangle(one, two);
                        aTriangle.ShapeType = (int)ShapeTypes.Triangle;
                        aTriangle.draw(g, blackpen);

                        //add the triangle to the shapes list
                        shapes.Add(aTriangle);
                    }
                }
                else if (selectCircleStatus == true)
                {
                    if (clicknumber == 0)
                    {
                        one = new Point(e.X, e.Y);
                        clicknumber = 1;
                    }
                    else
                    {
                        two = new Point(e.X, e.Y);
                        clicknumber = 0;
                        selectTriangleStatus = false;

                        Graphics g = this.CreateGraphics();
                        Pen blackpen = new Pen(Color.Black);

                        Circle aCircle = new Circle(one, two);
                        aCircle.ShapeType = (int)ShapeTypes.Circle;
                        aCircle.draw(g, blackpen);

                        //add the circle to the shapes list
                        shapes.Add(aCircle);
                    }
                }
            }
        }


        //public static void Main()
        //{
        //    Application.Run(new GrafPack());
        //}

    }
}


