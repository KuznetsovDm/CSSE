﻿using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace Programmer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IView
    {
        private ModelVisual3D[,,] _models;
        private readonly MainWindowViewModel _viewModel;
        private ModelVisual3D _mainModel;
        private ModelVisual3D _light;
        private const int _border = 2;
        private const double _coef = 1.0;

        public MainWindow()
        {
            InitializeComponent();
            _mainModel = MainModel;
            _light = Light;

            _viewModel = new MainWindowViewModel(this);

            DataContext = _viewModel;

            PreviewKeyDown += OnPreviewKeyDown;
        }

        public void Start(int x, int y, int z)
        {
            Create(20, 4, 2);
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.A || e.Key > Key.Z)
                return;
            e.Handled = true;
            _viewModel.OnKeyDown(e.Key);
        }

        private void Create(int x, int y, int z)
        {
            Viewport.Children.Clear();
            Viewport.Children.Add(_mainModel);
            Viewport.Children.Add(_light);
            var meshGeometry3d = new MeshGeometry3D()
            {
                Positions = new Point3DCollection
                {
                    new Point3D(0,0,0),
                    new Point3D(1,0,0),
                    new Point3D(0,1,0),
                    new Point3D(1,1,0),
                    new Point3D(0,0,1),
                    new Point3D(1,0,1),
                    new Point3D(0,1,1),
                    new Point3D(1,1,1)
                },
                TriangleIndices = new Int32Collection
                {
                    0,2,1, 1,2,3, 0,4,2, 2,4,6,
                    0,1,4, 1,5,4, 1,7,5, 1,3,7,
                    4,5,6, 7,6,5, 2,6,3, 3,6,7,
                },
                TextureCoordinates = new PointCollection()
                {
                    new System.Windows.Point(0, 1),
                    new System.Windows.Point(1, 1),
                    new System.Windows.Point(0, 0),
                    new System.Windows.Point(1, 0),

                    new System.Windows.Point(0, 1),
                    new System.Windows.Point(1, 1),
                    new System.Windows.Point(0, 0),
                    new System.Windows.Point(1, 0),
                }
            };
            //var material = new DiffuseMaterial(Brushes.Blue);
            var material = new DiffuseMaterial(new ImageBrush(new BitmapImage(new Uri(@"../../images.jpg", UriKind.Relative))));
            var geometryModel3D = new GeometryModel3D(meshGeometry3d, material);

            _models = new ModelVisual3D[x + _border * 2, y + _border * 2, z + 1];

            int i = 0, j, k;
            for (double offsetX = 0; i < x + _border * 2; i++, offsetX += _coef)
            {
                j = 0;
                for (double offsetY = 0; j < y + _border * 2; j++, offsetY += _coef)
                {
                    k = 0;
                    for (double offsetZ = 0; k < z + 1; k++, offsetZ -= _coef)
                    {
                        _models[i, j, k] = new ModelVisual3D
                        {
                            Content = geometryModel3D,
                            Transform = new TranslateTransform3D(offsetX, offsetY, offsetZ)
                        };
                        Viewport.Children.Add(_models[i, j, k]);
                    }
                }
            }
        }

        public void Destroy(Point point)
        {
            Console.WriteLine($"{point.X}, {point.Y}, {point.Z}");
            point = new Point(point.X + _border, point.Y + _border, point.Z);
            if (point.X < 0 || point.X >= _models.GetLength(0) || point.Y < 0 || point.Y >= _models.GetLength(1) || point.Z < 0 || point.Z >= _models.GetLength(2))
                return;

            OffsetX.Value = point.X * _coef;
            OffsetY.Value = point.Y * _coef;
            OffsetZ.Value = -point.Z * _coef;
            Console.WriteLine($"{OffsetX.Value}, {OffsetY.Value}, {OffsetZ.Value}");

            Viewport.Children.Remove(_models[point.X, point.Y, point.Z]);

            var meshGeometry3d = new MeshGeometry3D
            {
                Positions = new Point3DCollection
                {
                    new Point3D(0,0,0),
                    new Point3D(0.3,0,0),
                    new Point3D(0,0.3,0),
                    new Point3D(0.3,0.3,0),
                    new Point3D(0,0,0.3),
                    new Point3D(0.3,0,0.3),
                    new Point3D(0,0.3,0.3),
                    new Point3D(0.3,0.3,0.3)
                },
                TriangleIndices = new Int32Collection
                {
                    0,2,1, 1,2,3, 0,4,2, 2,4,6,
                    0,1,4, 1,5,4, 1,7,5, 1,3,7,
                    4,5,6, 7,6,5, 2,6,3, 3,6,7,
                },
                TextureCoordinates = new PointCollection()
                {
                    new System.Windows.Point(0, 1),
                    new System.Windows.Point(1, 1),
                    new System.Windows.Point(0, 0),
                    new System.Windows.Point(1, 0),

                    new System.Windows.Point(0, 1),
                    new System.Windows.Point(1, 1),
                    new System.Windows.Point(0, 0),
                    new System.Windows.Point(1, 0),
                }
            };
            var transform = _models[point.X, point.Y, point.Z].Transform as TranslateTransform3D;

            //var material = new DiffuseMaterial(Brushes.Green);
            var material = new DiffuseMaterial(new ImageBrush(new BitmapImage(new Uri(@"../../image2.jpg", UriKind.Relative))));
            var geometryModel3D = new GeometryModel3D(meshGeometry3d, material);

            var tempChildren = new ModelVisual3D[3, 3, 3];
            int i = 0, j, k;
            for (double offsetX = 0; i < tempChildren.GetLength(0); i++, offsetX += 0.35)
            {
                j = 0;
                for (double offsetY = 0; j < tempChildren.GetLength(1); j++, offsetY += 0.35)
                {
                    k = 0;
                    for (double offsetZ = 0; k < tempChildren.GetLength(2); k++, offsetZ += 0.35)
                    {
                        tempChildren[i, j, k] = new ModelVisual3D
                        {
                            Content = geometryModel3D,
                            Transform = new TranslateTransform3D(offsetX + transform.OffsetX, offsetY + transform.OffsetY, offsetZ + transform.OffsetZ)
                        };
                        Viewport.Children.Add(tempChildren[i, j, k]);
                    }
                }
            }

            new DestroyChildren(tempChildren, Dispatcher, Viewport);
        }

        //private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        //{
        //    if (int.TryParse(XCoord.Text, out var x) && int.TryParse(YCoord.Text, out var y) && int.TryParse(ZCoord.Text, out var z))
        //        Destroy(new Point(x, y, z));
        //}

		public void ShowMessageBox()
		{
			MessageBox.Show("Паз проделан.");
		}

		//private void Camera_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		//{
		//    if (CameraXpSlider == null)
		//        return;
		//    Console.WriteLine(CameraYZ.Value);
		//    CameraXpSlider.Value = CameraR.Value * Math.Sin(CameraXY.Value) * Math.Cos(CameraYZ.Value);
		//    CameraYpSlider.Value = CameraR.Value * Math.Sin(CameraXY.Value) * Math.Sin(CameraYZ.Value);
		//    CameraZpSlider.Value = CameraR.Value * Math.Cos(CameraXY.Value);
		//}
	}



    public class DestroyChildren
    {
        private readonly ModelVisual3D[,,] _children;
        private readonly Dispatcher _dispatcher;
        private readonly Viewport3D _viewport;
        private readonly Timer _timer;
        private int _counter;

        public DestroyChildren(ModelVisual3D[,,] children, Dispatcher dispatcher, Viewport3D viewport)
        {
            _children = children;
            _dispatcher = dispatcher;
            _viewport = viewport;
            _counter = 0;
            _timer = new Timer(Tick, null, 0, 20);
        }

        private void Tick(object obj)
        {
            _dispatcher.BeginInvoke(new Action(() =>
            {
                for (var i = 0; i < 3; i++)
                for (var j = 0; j < 3; j++)
                for (var k = 0; k < 3; k++)
                {
                    var transform = _children[i, j, k].Transform as TranslateTransform3D;
                    transform.OffsetZ += 0.1;
                    transform.OffsetY += (j - 1) * 0.001 * (2 + k * 2);
                    transform.OffsetX += (i - 1) * 0.001 * (2 + k * 2);
                }

                if (_counter++ > 100)
                {
                    foreach (var child in _children)
                        _viewport.Children.Remove(child);
                    _timer.Dispose();
                }
            }));
        }
    }


    public class ToVector3DConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Vector3D((double)values[0] - (double)values[1], (double)values[2] - (double)values[3], (double)values[4] - (double)values[5]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ToPoint3DConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Point3D((double)values[0], (double)values[1], (double)values[2]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CoefficientConverter : IValueConverter
    {
        public double Coefficient { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = (double?)parameter ?? 0;
            return param + (double)value * Coefficient;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
