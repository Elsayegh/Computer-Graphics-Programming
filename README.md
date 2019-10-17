# Computer-Graphics-Programming

2D drawing windows application using C# for implementation through visual studio.

# Design and Implementation:

Graphpack solution contains four classes which are circle, square and triangle class and each class contains the draw method to draw the shape when it’s called and fill method that highlight the shape when it’s selected, the three class inherits from the shape class.
Graphpack.cs that contains all the methods needed to achieve the required form functionality.
Initializing the main menu and adding Create, Select, Transform, Delete, Exit elements. Adding the sub menu elements, a list of type shape that store the shapes when its drawn
Then working on these elements by adding special methods and events that are
exit_form: close the form when the user clicks on Exit menu.
itemDelete: it loops over the shapes list that stores the drawn shapes. If it’s not empty and there is a selected shape, it will delete it from the form and the list and redraw the remained shapes.
moveItem: move the item, which redraw the shape by 20 pixels if arrow key right clicked and -20 pixels if it’s to left. 
itemRotation: rotate the item selected by the angle value.
selectSquare, selectTriangle, selectCircle, MoveClick, RotationClick: re set the boolean value to true and achieve an event depends on which value is set true it achieves a scope of code in the OnKeyDown method. 
selectShape: to highlight the first shape in the shapes list when the user clicks select menu.
OnKeyDown:this method contains the arrow keys right and left used in select, move or rotate the item based on the event that is true.
mouseClick: when create menu clicked and based on the choice from the sub menu, shape will be drawn between two points user will use mouse click to allocate it.

Testing:  drawing shapes works good but in selection when user cycle by right arrow key, index out of range or don’t highlight the triangle. If move selected from menu and arrow key clicked to move the shape, the rest of drawn shapes will be deleted, in addition to the rotation that happen only with one  left click but no transformation happen with more clicks.
