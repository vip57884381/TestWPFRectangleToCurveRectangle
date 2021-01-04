using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TestWPFRectangleToCurveRectangle
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool moveFg_ = false;
        private Ellipse ellipseTemp_ = null;
        private List<Ellipse> ellipseList_ = new List<Ellipse>();
        private List<Line> lineList_ = new List<Line>();

        private List<Ellipse> ellipseListControl_ = new List<Ellipse>();
        private List<Line> lineListControl_ = new List<Line>();

        private List<Ellipse> ellipseListAdj_ = new List<Ellipse>();
        private List<Line> lineListAdj_ = new List<Line>();

        private List<string> tagList_ = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowRectangle_Button_1_Click(object sender, RoutedEventArgs e)
        {
            int heightCanvas = 0;
            int widthCanvas = 0;

            heightCanvas = (int)Canvas_1.ActualHeight;
            widthCanvas = (int)Canvas_1.ActualWidth;

            int height = 0;
            int width = 0;

            Int32.TryParse(
                Width_TextBox_1.Text
                , out width
            );
            Int32.TryParse(
                Height_TextBox_1.Text
                , out height
            );

            int heightInterval_ = heightCanvas / (height + 1);
            int widthInterval_ = widthCanvas / (width + 1);

            canvasDelElement(ref ellipseList_);
            canvasDelElement(ref ellipseListControl_);
            canvasDelElement(ref ellipseListAdj_);
            canvasDelElement(ref lineList_);
            canvasDelElement(ref lineListControl_);
            canvasDelElement(ref lineListAdj_);

            for (int x = 0, x1 = 1; x < width; x++, x1++)
            {
                for (int y = 0, y1 = 1; y < width; y++, y1++)
                {
                    Ellipse ellipseTemp = new Ellipse()
                    {
                        Fill = Brushes.Blue
                        ,
                        Stroke = Brushes.Blue
                        ,
                        StrokeThickness = 1
                        ,
                        Height = 10
                        ,
                        Width = 10
                        ,
                        Tag = string.Format("{0},{1}"
                            , x
                            , y
                        )
                    };

                    Canvas.SetLeft(
                        ellipseTemp
                        , widthInterval_ * x1
                    );
                    Canvas.SetTop(
                        ellipseTemp
                        , heightInterval_ * y1
                    );

                    Canvas_1.Children.Add(
                        ellipseTemp
                    );
                    ellipseList_.Add(
                        ellipseTemp
                    );
                }
            }
        }

        private void ShowRectangleDrawLine_Button_1_Click(object sender, RoutedEventArgs e)
        {
            drawLine(
                ref ellipseList_
                , ref lineList_
                , Brushes.Blue
            );
        }

        private void ShowCurveRectangle_Button_1_Click(object sender, RoutedEventArgs e)
        {
            #region 新增控制點

            int height = 0;
            int width = 0;
            int heightHalf = 0;
            int widthHalf = 0;

            Int32.TryParse(
                Width_TextBox_1.Text
                , out width
            );
            Int32.TryParse(
                Height_TextBox_1.Text
                , out height
            );

            heightHalf = (height) / 2;
            widthHalf = (width) / 2;

            height -= 1;
            width -= 1;

            tagList_.Clear();
            tagList_.Add(string.Format("{0},{1}", 0, 0));
            tagList_.Add(string.Format("{0},{1}", 0, heightHalf));
            tagList_.Add(string.Format("{0},{1}", 0, height));
            tagList_.Add(string.Format("{0},{1}", widthHalf, 0));
            tagList_.Add(string.Format("{0},{1}", widthHalf, heightHalf));
            tagList_.Add(string.Format("{0},{1}", widthHalf, height));
            tagList_.Add(string.Format("{0},{1}", width, 0));
            tagList_.Add(string.Format("{0},{1}", width, heightHalf));
            tagList_.Add(string.Format("{0},{1}", width, height));

            #endregion 新增控制點

            canvasDelElement(ref ellipseListControl_);

            int i_e = ellipseList_.Count;
            for (int i = 0; i < i_e; i++)
            {
                Ellipse ellipseTemp = ellipseList_[i] as Ellipse;
                if (tagList_.Contains(ellipseTemp.Tag.ToString()) == false)
                    continue;

                double left = Canvas.GetLeft(ellipseTemp);
                double top = Canvas.GetTop(ellipseTemp);

                Ellipse ellipseTemp2 = new Ellipse()
                {
                    Fill = Brushes.Red
                    ,
                    Stroke = Brushes.Red
                    ,
                    StrokeThickness = 1
                    ,
                    Height = 10
                    ,
                    Width = 10
                    ,
                    Tag = ellipseTemp.Tag
                    ,
                    ToolTip = ellipseTemp.Tag
                };

                ellipseTemp2.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
                ellipseTemp2.MouseLeftButtonUp += Ellipse_MouseLeftButtonUp;

                Canvas.SetLeft(
                    ellipseTemp2
                    , left
                );
                Canvas.SetTop(
                    ellipseTemp2
                    , top
                );

                Canvas_1.Children.Add(
                    ellipseTemp2
                );
                ellipseListControl_.Add(
                    ellipseTemp2
                );
            }
        }

        private void ShowCurveRectangleDrawLine_Button_1_Click(object sender, RoutedEventArgs e)
        {
            if (ellipseListControl_.Count == 0)
                goto fatal;

            canvasDelElement(ref lineListControl_);

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    drawLineLevel2(
                        ref ellipseListControl_
                        , ref lineListControl_
                        , Brushes.Red
                        , tagList_[x + y * 3]
                        , tagList_[(x + 1) + y * 3]
                    );
                }
            }

            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    drawLineLevel2(
                        ref ellipseListControl_
                        , ref lineListControl_
                        , Brushes.Red
                        , tagList_[x + y * 3]
                        , tagList_[x + (y+ 1) * 3]
                    );
                }
            }

            fatal:;
        }

        private void ShowCurveRectangleAdj_Button_1_Click(object sender, RoutedEventArgs e)
        {
            ShowCurveRectangleDelAdj_Button_1_Click(null, null);

            List<Point> pointListBefore = new List<Point>();
            List<Point> pointListAfter = new List<Point>();

            for (int i = 0; i < tagList_.Count; i++)
            {
                string tag_i = tagList_[i];
                List<Ellipse> ellipsesListTemp_i = ellipseList_.Where(o => o.Tag.ToString() == tag_i).ToList();

                if (ellipsesListTemp_i == null
                    || ellipsesListTemp_i.Count() == 0
                    )
                    continue;

                Ellipse ellipse_src_i = ellipsesListTemp_i[0];

                List<Ellipse> ellipsesListControlTemp_i = ellipseListControl_.Where(o => o.Tag.ToString() == tag_i).ToList();

                if (ellipsesListControlTemp_i == null
                    || ellipsesListControlTemp_i.Count() == 0
                    )
                    continue;

                Ellipse ellipse_control_i = ellipsesListControlTemp_i[0];

                double left_src_i = Canvas.GetLeft(ellipse_src_i);
                double top_src_i = Canvas.GetTop(ellipse_src_i);

                double left_control_i = Canvas.GetLeft(ellipse_control_i);
                double top_control_i = Canvas.GetTop(ellipse_control_i);

                pointListBefore.Add(new Point(left_src_i, top_src_i));
                pointListAfter.Add(new Point(left_control_i, top_control_i));
            }

            for (int i = 0; i < ellipseList_.Count; i++)
            {
                Ellipse ellipse_src_i = ellipseList_[i];

                double left_src_i = Canvas.GetLeft(ellipse_src_i);
                double top_src_i = Canvas.GetTop(ellipse_src_i);

                Point pointNew_i = computeTransformationParameters(
                    new Point(left_src_i
                    , top_src_i)
                    , pointListBefore
                    , pointListAfter
                );

                Ellipse ellipseTemp = new Ellipse()
                {
                    Fill = Brushes.Yellow
                    ,
                    Stroke = Brushes.Yellow
                    ,
                    StrokeThickness = 1
                    ,
                    Height = 10
                    ,
                    Width = 10
                    ,
                    Tag = ellipse_src_i.Tag
                    ,
                    ToolTip = string.Format("move:({0},{1})"
                        , left_src_i - pointNew_i.X
                        , top_src_i - pointNew_i.Y
                    )
                };

                Canvas.SetLeft(
                    ellipseTemp
                    , pointNew_i.X
                );
                Canvas.SetTop(
                    ellipseTemp
                    , pointNew_i.Y
                );

                Canvas_1.Children.Add(
                    ellipseTemp
                );
                ellipseListAdj_.Add(
                    ellipseTemp
                );
            }
        }

        private void ShowCurveRectangleDelAdj_Button_1_Click(object sender, RoutedEventArgs e)
        {
            canvasDelElement(ref ellipseListAdj_);
            canvasDelElement(ref lineListAdj_);
        }

        private void ShowCurveRectangleAdjDrawLine_Button_1_Click(object sender, RoutedEventArgs e)
        {
            drawLine(
                ref ellipseListAdj_
                , ref lineListAdj_
                , Brushes.Yellow
            );
        }

        private void Canvas_1_MouseMove(object sender, MouseEventArgs e)
        {
            if (ellipseTemp_ == null)
                goto fatal;

            Point mouseMovePoint_Canvas = e.GetPosition(Canvas_1);

            double radius = ellipseTemp_.Height * 0.5;
            Canvas.SetLeft(
                           ellipseTemp_
                           , mouseMovePoint_Canvas.X - radius
                       );
            Canvas.SetTop(
                ellipseTemp_
                , mouseMovePoint_Canvas.Y - radius
            );

            if (moveFg_ == false)
                ellipseTemp_ = null;

            fatal:;
        }

        private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            moveFg_ = false;
            ellipseTemp_ = null;
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            moveFg_ = true;
            ellipseTemp_ = sender as Ellipse;
        }

        public void canvasDelElement(ref List<Ellipse> inUIElement)
        {
            foreach (Ellipse ellipseTemp in inUIElement)
            {
                canvasDelElement(
                    ellipseTemp
                );
            }
            inUIElement.Clear();
        }

        public void canvasDelElement(ref List<Path> inUIElement)
        {
            foreach (Path pathTemp in inUIElement)
            {
                canvasDelElement(
                    pathTemp
                );
            }
            inUIElement.Clear();
        }

        public void canvasDelElement(ref List<Line> inUIElement)
        {
            foreach (Line lineTemp in inUIElement)
            {
                canvasDelElement(
                    lineTemp
                );
            }
            inUIElement.Clear();
        }

        public void canvasDelElement(UIElement inUIElement)
        {
            Canvas_1.Children.Remove(
                inUIElement
            );
        }

        public Point computeTransformationParameters(
           Point inPoint
           , List<Point> inPointListBefore
           , List<Point> inPointListAfter
        )
        {
            Point result = new Point(0, 0);

            List<Point> pointListBefore = inPointListBefore;
            List<Point> pointListAfter = inPointListAfter;

            int i_e = pointListBefore.Count;

            double[] wArr = new double[i_e];
            double wSum = 0;

            for (int i = 0; i < i_e; i++)
            {
                double dX_i, dY_i;
                dX_i = (pointListAfter[i].X - inPoint.X);
                dY_i = (pointListAfter[i].Y - inPoint.Y);

                // Use implicit alpha=2
                // refinement: try do some other distances like Gaussian round stroke p[i]-q[i]
                // shortcut computation on 
                double d_i =
                    (dX_i
                        * dX_i)
                    + (dY_i
                        * dY_i);

                wArr[i] = (d_i == 0) ? 1 : (1.0 / d_i); /* 公式1 */

                wSum += wArr[i];
            }

            /* 公式2 */
            Point pStart = new Point(0, 0);

            for (int i = 0; i < i_e; i++)
            {
                pStart.X += wArr[i] * pointListBefore[i].X;
                pStart.Y += wArr[i] * pointListBefore[i].Y;
            }
            pStart.X = pStart.X / wSum;
            pStart.Y = pStart.Y / wSum;

            Point qStart = new Point(0, 0);
            for (int i = 0; i < i_e; i++)
            {
                qStart.X += wArr[i] * pointListAfter[i].X;
                qStart.Y += wArr[i] * pointListAfter[i].Y;
            }
            qStart.X = qStart.X / wSum;
            qStart.Y = qStart.Y / wSum;

            Point[] pHatArr = new Point[i_e];
            for (int i = 0; i < i_e; i++)
            {
                pHatArr[i].X = pointListBefore[i].X - pStart.X;
                pHatArr[i].Y = pointListBefore[i].Y - pStart.Y;
            }

            Point[] qHatArr = new Point[i_e];
            for (int i = 0; i < i_e; i++)
            {
                qHatArr[i].X = pointListAfter[i].X - qStart.X;
                qHatArr[i].Y = pointListAfter[i].Y - qStart.Y;
            }

            // 獲得M
            double m11 = 0
                , m12 = 0;
            for (int i = 0; i < i_e; i++)
            {
                double a = pHatArr[i].X;
                double b = pHatArr[i].Y;
                double c = qHatArr[i].X;
                double d = qHatArr[i].Y;

                //                         a   b     c   d
                // M = MuNorm* Sum w[i] (        ) (       )    (eq. 6) from article
                //                         b  -a     d  -c

                m11 = m11 + wArr[i] * (a * c + b * d);
                m12 = m12 + wArr[i] * (a * d + b * -c);
            }

            // Norm, Mt M = I so muNorm is
            double muNorm = Math.Sqrt(m11 * m11 + m12 * m12);

            if ((muNorm < double.Epsilon)
                || (i_e == 1)
            )
            {
                m11 = 1.0;
                m12 = 0.0;
            }
            else
            {
                m11 /= muNorm;
                m12 /= muNorm;
            }

            // Transform a point using the transformation parameters of this MeshPoint
            if (i_e <= 0)
            {
                result = inPoint;
                goto fatal;
            }

            double xT = (inPoint.X - pStart.X);
            double yT = (inPoint.Y - pStart.Y);

            // Matrix M, pos.Y= m12*x+ m22*y =..; use m21=-m12 and m22 = m11
            result.X = m11 * xT - m12 * yT + qStart.X;
            result.Y = m12 * xT + m11 * yT + qStart.Y;

            fatal:
            return result;
        }
        
        public void drawLine(ref List<Ellipse> outEllipsesList
            , ref List<Line> outLineList
            , Brush inBrush
            )
        {

            if (outEllipsesList.Count == 0)
                goto fatal;

            canvasDelElement(ref outLineList);

            int height = 0;
            int width = 0;

            Int32.TryParse(
                Width_TextBox_1.Text
                , out width
            );
            Int32.TryParse(
                Height_TextBox_1.Text
                , out height
            );

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < (height - 1); y++)
                {
                    string tag_1 = string.Format("{0},{1}"
                        , x
                        , y
                    );
                    string tag_2 = string.Format("{0},{1}"
                        , x
                        , y + 1
                    );

                    drawLineLevel2(
                        ref outEllipsesList
                        , ref outLineList
                        , inBrush
                        , tag_1
                        , tag_2
                    );
                }
            }

            for (int y = 0; y < (height); y++)
            {
                for (int x = 0; x < (width - 1); x++)
                {
                    string tag_1 = string.Format("{0},{1}"
                        , x
                        , y
                    );
                    string tag_2 = string.Format("{0},{1}"
                        , x + 1
                        , y
                    );

                    drawLineLevel2(
                        ref outEllipsesList
                        , ref outLineList
                        , inBrush
                        , tag_1
                        , tag_2
                    );
                }
            }

            fatal:;
        }

        public void drawLineLevel2(ref List<Ellipse> outEllipsesList
            , ref List<Line> outLineList
            , Brush inBrush
            , string inTag1
            , string inTag2
            )
        {
            List<Ellipse> ellipseList1 = outEllipsesList.Where(o => o.Tag.ToString() == inTag1).ToList();
            List<Ellipse> ellipseList2 = outEllipsesList.Where(o => o.Tag.ToString() == inTag2).ToList();

            if (ellipseList1.Count == 0
                || ellipseList2.Count == 0
            )
                goto fatal;

            Ellipse ellipse1 = ellipseList1[0];
            Ellipse ellipse2 = ellipseList2[0];
            double radio = ellipse1.Height * 0.5;

            Line line_i = new Line();
            line_i.X1 = Canvas.GetLeft(ellipse1) + radio;
            line_i.Y1 = Canvas.GetTop(ellipse1) + radio;
            line_i.X2 = Canvas.GetLeft(ellipse2) + radio;
            line_i.Y2 = Canvas.GetTop(ellipse2) + radio;
            line_i.Stroke = inBrush;
            line_i.StrokeThickness = 1;

            Canvas_1.Children.Add(
                line_i
            );
            outLineList.Add(line_i);

            fatal:;
        }
    }
}
