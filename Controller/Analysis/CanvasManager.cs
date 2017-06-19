using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Controller
{
    /// <summary>
    /// Takes care of visualizing graphs in canvases
    /// </summary>
    class CanvasManager
    {
        /// <summary>
        /// Normalized points
        /// </summary>
        private double[] _points;

        /// <summary>
        /// Max value of the y axis
        /// </summary>
        private double _ymax;

        /// <summary>
        /// Min value of the y axis
        /// </summary>
        private double _ymin;

        /// <summary>
        /// Canvas panel
        /// </summary>
        private Canvas _canvas;

        /// <summary>
        /// Creates a canvas graph manager based on the given data
        /// </summary>
        /// <param name="canvas">Canvas used for the graph</param>
        /// <param name="points">Vector of points used for the graph</param>
        /// <param name="yMax">y axis max value</param>
        /// <param name="yMin">y axis min value</param>
        public CanvasManager(Canvas canvas, double[] points, double yMax, double yMin)
        {
            _canvas = canvas;
            _points = points;
            _ymax = yMax;
            _ymin = yMin;

            Normalize();
        }

        /// <summary>
        /// Normalizes points to suit the canvas height
        /// </summary>
        private void Normalize()
        {
            double min = Math.Abs(_ymin);
            double range = Math.Abs(_ymax) + Math.Abs(_ymin);

            for(int i = 0; i < _points.Length; i++)
            {
                _points[i] += min;
                _points[i] *= _canvas.Height / range;
            }
        }

        /// <summary>
        /// Draws the points of the graph
        /// </summary>
        public void DrawPoints()
        {
            //Draws the yaxis
            /*
            Line yaxis = new Line() { X1 = 0, Y1 = 0, X2 = 0, Y2 = _canvas.Height, Width = 1, Stroke = Brushes.LightGray };
            Canvas.SetTop(yaxis, 0);
            Canvas.SetLeft(yaxis, 0);
            _canvas.Children.Add(yaxis);*/

            //Draws each point
            for(int i = 0; i < _points.Length; i++)
            {
                Ellipse point = new Ellipse() { Width = 3, Height = 3, Stroke = Brushes.Black };
                Canvas.SetBottom(point, _points[i] - point.Height/2);
                Canvas.SetLeft(point, (i) * _canvas.Width / (_points.Length - 1) - point.Width/2);
                _canvas.Children.Add(point);
            }
        }

        /// <summary>
        /// Clears all the points
        /// </summary>
        public void Clear()
        {
            _canvas.Children.Clear();
            GC.Collect();
        }
    }
}
