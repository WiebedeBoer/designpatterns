using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Windows;
using System.Windows.Input;
//using System.Windows.Forms;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Markup;
using Windows.UI.Input;
//using System.Windows.Threading;


//using System.Drawing;
//using System.Windows.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace tekenprogramma
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string type = "Rectangle";
        string mainAction;
        List<CreateRectangle> shapeslist = new List<CreateRectangle>();
        public bool moving = false;




        public MainPage()
        {
            this.InitializeComponent();

            //var w = new Window();
            //w.Width = 600;
            //w.Height = 400;

            //var c = new Canvas();
            var c = paintSurface;

            PointerPoint dragStart = null;



                //global vars               

            //int height = 1;
            //int width = 1;
            int cpx;
            int cpy;
            //bool firstcp = true;
            //int elements = 0;
            int dcpx;
            int dcpy;

            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Colors.Red);
            
            //MouseButtonEventHandler mouseDown = (sender, args) => {
            PointerEventHandler mouseDown = (sender, args) => {
                var element = (UIElement)sender;
                dragStart = args.GetCurrentPoint(element);
                //dragStart = args.GetPosition(element);
                dcpx = Convert.ToInt16(args.GetCurrentPoint(paintSurface).Position.X);
                dcpy = Convert.ToInt16(args.GetCurrentPoint(paintSurface).Position.Y);
                //element.CaptureMouse();
                
                element.CapturePointer(args.Pointer);
                CreateRectangle tmp = new CreateRectangle();
                tmp.leftCoord = dcpx;
                tmp.topCoord = dcpy;
                tmp.actionType = "create";
            };
            //MouseButtonEventHandler mouseUp = (sender, args) => {
            PointerEventHandler mouseUp = (sender, args) => {
                var element = (UIElement)sender;
                dragStart = null;
                element.ReleasePointerCapture(args.Pointer);
            };
            //MouseEventHandler mouseMove = (sender, args) => {
            PointerEventHandler mouseMove = (sender, args) => {
                //if (dragStart != null && args.LeftButton == MouseButtonState.Pressed)
                if (dragStart != null && args.GetCurrentPoint((UIElement)sender).Properties.IsLeftButtonPressed)
                {
                    var element = (UIElement)sender;
                    //var p2 = args.GetCurrentPoint(c);
                    
                    //Canvas.SetLeft(element, p2.X - dragStart.Value.X);
                    //Canvas.SetTop(element, p2.Y - dragStart.Value.Y);
                    cpx = Convert.ToInt16(args.GetCurrentPoint(paintSurface).Position.X);
                    cpy = Convert.ToInt16(args.GetCurrentPoint(paintSurface).Position.Y);
                    //shapeslist.Find(x => x.actionType.Contains("selected")));
                    CreateRectangle tmp2 = shapeslist.Find(x => x.actionType.Contains("create"));
                    tmp2.rightCoord = cpx;
                    tmp2.bottomCoord = cpy;
                    tmp2.actionType = "unselected";
                }
            };

            Action<UIElement> enableDrag = (element) => {
            //Action<Canvas> enableDrag = (element) => {
                //Action<paintsurface> enableDrag = (element) => {
                element.PointerPressed += mouseDown;
                element.PointerMoved += mouseMove;
                element.PointerReleased += mouseUp;
            };

            var shapes = new UIElement[] {
                new Ellipse() { Fill = myBrush, Width = 100, Height = 100 },
                new Rectangle() { Fill = myBrush, Width = 200, Height = 100 },
            };

            foreach (var shape in shapes)
            {
                enableDrag(shape);
                c.Children.Add(shape);
            }

            //w.Content = c;
            //w.ShowDialog();

        }
        

        //elipse
        private void Elipse_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
        }

        //rectangle
        private void Rectangle_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
        }

        //ornament
        private void Ornament_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
        }

        //group
        private void Group_Click(object sender, RoutedEventArgs e)
        {

        }

        //undo
        private void Undo_Click(object sender, RoutedEventArgs e)
        {

        }

        //redo
        private void Redo_Click(object sender, RoutedEventArgs e)
        {

        }

        //save
        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        //load
        private void Load_Click(object sender, RoutedEventArgs e)
        {

        }

        //resize
        private void Resize_Click(object sender, RoutedEventArgs e)
        {
            mainAction = "resize";
            //CreateRectangle tmp3 = new CreateRectangle();
            foreach (var resized in shapeslist)
            {
                if (resized.actionType == "selected")
                {
                    //tmp3 = resized;
                    //var tmp3 = shapeslist.Find(x => x.actionType.Contains("selected"));
                    int left = Convert.ToInt16(resized.leftCoord); //left coord
                    int top = Convert.ToInt16(resized.topCoord); //top coord
                    string textWidth = Width.Text; //new width
                    int resizeWidth = Convert.ToInt16(textWidth);
                    string textHeight = Height.Text; //new height
                    int resizeHeight = Convert.ToInt16(textHeight);
                    int right = left + resizeWidth; //right coord
                    int bottom = top + resizeHeight; //top coord
                    resized.bottomCoord = bottom;
                    resized.rightCoord = right;
                    resized.actionType = "unselected";
                }
            }




        }

        //move
        private void Move_Click(object sender, RoutedEventArgs e)
        {
            mainAction = "move";

            foreach (var resized in shapeslist)
            {
                if (resized.actionType == "selected")
                {
                    int left = Convert.ToInt16(resized.leftCoord); //left coord
                    int top = Convert.ToInt16(resized.topCoord); //top coord
                    string textWidth = Width.Text; //new width
                    int resizeWidth = Convert.ToInt16(textWidth);
                    string textHeight = Height.Text; //new height
                    int resizeHeight = Convert.ToInt16(textHeight);
                    int right = left + resizeWidth; //right coord
                    int bottom = top + resizeHeight; //top coord
                    resized.bottomCoord = bottom;
                    resized.rightCoord = right;
                    resized.actionType = "unselected";
                }
            }

            if (moving)
            {
                front_canvas.Children.Remove(movingelement);
                Canvas.SetLeft(movingelement, cpx);
                Canvas.SetTop(movingelement, cpy);
            }
            moving = !moving;

            /*
            Span<int> storage = stackalloc int[10];
            int num = 0;
            foreach (ref int item in shapeslist)
            {
                item = num++;
            }

            foreach (ref readonly var item in shapeslist)
            {
                //Console.Write($"{item} ");
            }
            */
        }



/*
                     movingelement = e.OriginalSource as FrameworkElement;
                    height = Convert.ToInt16(cpy - e.GetCurrentPoint(front_canvas).Position.Y);
                    width = Convert.ToInt16(cpx - e.GetCurrentPoint(front_canvas).Position.X);
                    if (type == "Rectangle")
                    {
                        Rectangle tmp = new Rectangle();
                        tmp.Width = Math.Abs(width);
                        tmp.Height = Math.Abs(height);
                        tmp.Opacity = 1;
                        SolidColorBrush brush = new SolidColorBrush();
                        brush.Color = Windows.UI.Colors.Blue;
                        tmp.Fill = brush;
                        tmp.Stroke = brush;
                        tmp.Name = elements.ToString();
                        Canvas.SetLeft(tmp, cpx);
                        Canvas.SetTop(tmp, cpy);
                        tmp.PointerPressed += Ink_canvas_PointerPressed;
                        front_canvas.Children.Add(tmp);
                        Rectangle.Content = "Klik 2";
                    }
                    else
                    {
                        Ellipse tmp = new Ellipse();
                        tmp.Width = width;
                        tmp.Height = height;
                        tmp.Opacity = 1;
                        SolidColorBrush brush = new SolidColorBrush();
                        brush.Color = Windows.UI.Colors.Blue;
                        tmp.Fill = brush;
                        tmp.Stroke = brush;
                        tmp.Name = elements.ToString();
                        ++elements;
                        Canvas.SetLeft(tmp, cpx);
                        Canvas.SetTop(tmp, cpy);
                        tmp.PointerPressed += Ink_canvas_PointerPressed;
                        front_canvas.Children.Add(tmp);
                        Rectangle.Content = "Klik 2";
                    }
                    FrameworkElement tmptwee = e.OriginalSource as FrameworkElement;
                    Rectangle.Content = tmptwee.Name;
     */

//paint canvas
private void Front_canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //front_canvas.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0,0,0,0));
        }

        //breedte aanpassen
        private void Width_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var tmp3 = shapeslist.Find(x => x.actionType.Contains("selected"));
            //int left = Convert.ToInt16(tmp3.leftCoord); //left coord
            //TextBox t = (TextBox)sender;

            //int resizeWidth = Convert.ToInt16(TextBox);
            // resizeHeight = Convert.ToInt16(TextBox);
            //int right = left + resizeWidth;
            //tmp3.rightCoord = right;
            //int bottom = top + resizeHeight;
            //tmp3.bottomCoord = bottom;
            //tmp3.actionType = "unselected";

        }

        //hoogte aanpassen
        private void Height_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var tmp3 = shapeslist.Find(x => x.actionType.Contains("selected"));
            //int top = Convert.ToInt16(tmp3.leftCoord); //top coord
            //TextBox t = (TextBox)sender;
            //int resizeHeight = Convert.ToInt16(t.Text);
            //int bottom = top + resizeHeight;
            //tmp3.bottomCoord = bottom;
        }


        /*
         private void Ink_canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (moving)
            {
                cpx = Convert.ToInt16(e.GetCurrentPoint(front_canvas).Position.X);
                cpy = Convert.ToInt16(e.GetCurrentPoint(front_canvas).Position.Y);
                Move_Click(sender, e);
            }
            else
            {
                if (firstcp)
                {
                    cpx = Convert.ToInt16(e.GetCurrentPoint(front_canvas).Position.X);
                    cpy = Convert.ToInt16(e.GetCurrentPoint(front_canvas).Position.Y);
                    Rectangle.Content = "Klik 1";
                }
                else
                {
                    movingelement = e.OriginalSource as FrameworkElement;
                    height = Convert.ToInt16(cpy - e.GetCurrentPoint(front_canvas).Position.Y);
                    width = Convert.ToInt16(cpx - e.GetCurrentPoint(front_canvas).Position.X);
                    if (type == "Rectangle")
                    {
                        Rectangle tmp = new Rectangle();
                        tmp.Width = Math.Abs(width);
                        tmp.Height = Math.Abs(height);
                        tmp.Opacity = 1;
                        SolidColorBrush brush = new SolidColorBrush();
                        brush.Color = Windows.UI.Colors.Blue;
                        tmp.Fill = brush;
                        tmp.Stroke = brush;
                        tmp.Name = elements.ToString();
                        Canvas.SetLeft(tmp, cpx);
                        Canvas.SetTop(tmp, cpy);
                        tmp.PointerPressed += Ink_canvas_PointerPressed;
                        front_canvas.Children.Add(tmp);
                        Rectangle.Content = "Klik 2";
                    }
                    else
                    {
                        Ellipse tmp = new Ellipse();
                        tmp.Width = width;
                        tmp.Height = height;
                        tmp.Opacity = 1;
                        SolidColorBrush brush = new SolidColorBrush();
                        brush.Color = Windows.UI.Colors.Blue;
                        tmp.Fill = brush;
                        tmp.Stroke = brush;
                        tmp.Name = elements.ToString();
                        ++elements;
                        Canvas.SetLeft(tmp, cpx);
                        Canvas.SetTop(tmp, cpy);
                        tmp.PointerPressed += Ink_canvas_PointerPressed;
                        front_canvas.Children.Add(tmp);
                        Rectangle.Content = "Klik 2";
                    }
                    FrameworkElement tmptwee = e.OriginalSource as FrameworkElement;
                    Rectangle.Content = tmptwee.Name;
                }
                firstcp = !firstcp;
            }
}
         
         */


        /*
        private void Ink_canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (firstcp)
            {
                cpx = Convert.ToInt16(e.GetCurrentPoint(front_canvas).Position.X);
                cpy = Convert.ToInt16(e.GetCurrentPoint(front_canvas).Position.Y);
                Rectangle.Content = "Klik 1";
            }
            else
            {
                height = Convert.ToInt16(cpy - e.GetCurrentPoint(front_canvas).Position.Y);
                width = Convert.ToInt16(cpx - e.GetCurrentPoint(front_canvas).Position.X);
                if (type == "Rectangle")
                {
                    Rectangle tmp = new Rectangle();
                    tmp.Width = width;
                    tmp.Height = height;
                    tmp.Opacity = 1;
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Windows.UI.Colors.Blue;
                    tmp.Fill = brush;
                    tmp.Stroke = brush;
                    tmp.Name = elements.ToString();
                    Canvas.SetLeft(tmp, cpx);
                    Canvas.SetTop(tmp, cpy);
                    tmp.PointerPressed += Ink_canvas_PointerPressed;
                    front_canvas.Children.Add(tmp);
                    Rectangle.Content = "Klik 2";
                }
                else
                {
                    Ellipse tmp = new Ellipse();
                    tmp.Width = width;
                    tmp.Height = height;
                    tmp.Opacity = 1;
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Windows.UI.Colors.Blue;
                    tmp.Fill = brush;
                    tmp.Stroke = brush;
                    tmp.Name = elements.ToString();
                    ++elements;
                    Canvas.SetLeft(tmp, cpx);
                    Canvas.SetTop(tmp, cpy);
                    tmp.PointerPressed += Ink_canvas_PointerPressed;
                    front_canvas.Children.Add(tmp);
                    Rectangle.Content = "Klik 2";
                }
                FrameworkElement tmptwee = e.OriginalSource as FrameworkElement;
                Rectangle.Content = tmptwee.Name;
            }
            firstcp = !firstcp;
        }
        */








        /*
    private void Ink_canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
    int x = e.GetCurrentPoint(paintSurface).Position.X;
    int y = e.GetCurrentPoint(paintSurface).Position.Y;
    }
    */


        /*

         double height = 1;
        double width = 1;
        string type = "Rectangle";
        double cpx;
        double cpy;
        bool firstcp = true;

        public MainPage()
        {
            this.InitializeComponent();
        }


        public void MyMouseDoubleClickEvent(object sender, RoutedEventArgs e)
        {
            // Get mouse position
            FrameworkElement button = e.OriginalSource as FrameworkElement;
            type = button.Name;
            Rectangle.Content = type;
            //double x = e.GetCurrentPoint(front_canvas).Position.X;
            //double y = e.GetCurrentPoint(front_canvas).Position.Y;
            // Initialize a new Rectangle
            //Rectangle r = new Rectangle();

            // Set up rectangle's size
            //r.Width = 5;
            //r.Height = 5;

            // Set up the Background color
            //r.Fill = Brushes.Black;

            // Set up the position in the window, at mouse coordonate
            //Canvas.SetTop(r, p.Y);
            //Canvas.SetLeft(r, p.X);

            // Add rectangle to the Canvas
            //ink_canvas.Children.Add(r);
        }

        private void Ink_canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (firstcp)
            {
                cpx = e.GetCurrentPoint(front_canvas).Position.X;
                cpy = e.GetCurrentPoint(front_canvas).Position.Y;
                Rectangle.Content = "Klik 1";
            }
            else
            {
                height = Math.Abs(cpy - e.GetCurrentPoint(front_canvas).Position.Y);
                width = Math.Abs(cpx - e.GetCurrentPoint(front_canvas).Position.X);
                if(type == "Rectangle")
                {
                    Rectangle tmp = new Rectangle();
                    tmp.Width = width;
                    tmp.Height = height;
                    tmp.Opacity = 1;
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Windows.UI.Colors.Blue;
                    tmp.Fill = brush;
                    tmp.Stroke = brush;
                    front_canvas.Children.Add(tmp);
                    Rectangle.Content = "Klik 2";
                }
                else
                {
                    Ellipse tmp = new Ellipse();
                    tmp.Width = width;
                    tmp.Height = height;
                    tmp.Opacity = 1;
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Windows.UI.Colors.Blue;
                    tmp.Fill = brush;
                    tmp.Stroke = brush;
                    front_canvas.Children.Add(tmp);
                    Rectangle.Content = "Klik 2";
                }
            }
            firstcp = !firstcp;
    }

         */





        /*
    private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var row = sender as DataGridRow;
        var dataElement = row?.DataContext as MyItem;
        if (dataElement?.CanDoubleClick != true)
            return;

        // process double-click here
        return;
    }
    */

        /*
    void image_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
        {
            Close();
        }
    }
    */

        /*
    private void Canvas_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
            currentPoint = e.GetPosition(this);
    }

    private void Canvas_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            Line line = new Line();

            line.Stroke = SystemColors.WindowFrameBrush;
            line.X1 = currentPoint.X;
            line.Y1 = currentPoint.Y;
            line.X2 = e.GetPosition(this).X;
            line.Y2 = e.GetPosition(this).Y;

            currentPoint = e.GetPosition(this);

            paintSurface.Children.Add(line);
        }
    }
    */

        /*
        public void MyMouseDoubleClickEvent(object sender, Windows.UI.Xaml.Input.Mouse e)
        {
            // Get mouse position
            Point p = Mouse.GetPosition(front_canvas);

            // Initialize a new Rectangle
            Rectangle r = new Rectangle();

            // Set up rectangle's size
            r.Width = 5;
            r.Height = 5;

            // Set up the Background color
            r.Fill = Brushes.Black;

            // Set up the position in the window, at mouse coordonate
            Canvas.SetTop(r, p.Y);
            Canvas.SetLeft(r, p.X);

            // Add rectangle to the Canvas
            ink_canvas.Children.Add(r);
        }
        */

    }
}
