using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Akem.Controls;

namespace Akem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImageScrollViewerViewer_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void ImagePanel_Drop(object sender, DragEventArgs e)
        {
            var fileDrop = (string[])e.Data.GetData("FileName");
            if (!fileDrop.Any())
            {
                return;
            }

            var fileName = fileDrop[0];

            var mozaicCanvas = (MozaicCanvas)MainGrid.Resources["MozaicCanvas"];

            var bi = new BitmapImage(new Uri(fileName));

            var visual = new DrawingVisual();
            var dc = visual.RenderOpen();
            dc.DrawImage(bi, new Rect(new Size(bi.Width, bi.Height)));
            dc.Close();
            mozaicCanvas.AddVisual(visual);

            InitMozaicCanvas(bi.Width, bi.Height);
        }

        private void InitMozaicCanvas(double width, double height)
        {
            var mozaicCanvas = (MozaicCanvas)MainGrid.Resources["MozaicCanvas"];

            ImagePanel.Children.Clear();
            ImagePanel.Children.Add(mozaicCanvas);

            mozaicCanvas.Children.Clear();
            mozaicCanvas.Width = width;
            mozaicCanvas.Height = height;
        }

        private void FillCanvasWithTiles(MozaicCanvas canvas)
        {
            var tileSize = 15;

             var clientWidth = canvas.Width;
             var clientHeight = canvas.Height;

             var tilesCountHor = clientWidth / tileSize;
             var tilesCountVer = clientHeight / tileSize;

             var gridWidth = clientWidth / tilesCountHor;
             var gridHeight = clientHeight / tilesCountVer;


             var brush1 = new SolidColorBrush(Colors.DimGray);
             var brush2 = new SolidColorBrush(Colors.DarkGray);

             var brushes = new Brush[] {brush1, brush2};

             var randomizer = new Random(0);

             for (int i = 0; i < tilesCountVer; i++)
             {
                 for (int j = 0; j < tilesCountHor; j++)
                 {
                     var positionX = gridWidth * j;
                     var positionY = gridHeight * i;
                     var visual = new DrawingVisual();
                     var dc = visual.RenderOpen();

                     var selectedBrush = brushes[randomizer.Next(2)];

                     dc.DrawRectangle(selectedBrush, new Pen(new SolidColorBrush(Colors.Black), 0.3), new Rect(positionX, positionY, gridWidth, gridHeight));

                     dc.Close();
                     canvas.AddVisual(visual);
                 }
             }
        }
    }
}
