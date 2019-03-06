using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Mvvm;

namespace Programmer
{
	internal class MainWindowViewModel : BindableBase
	{
		public const int XBarMax = 20;
		public const int YBarMax = 4;
		public const int ZBarMax = 2;

		private int _xMax = 10;
		private int _yMax = 4;
		private int _zMax = 2;
		private int _tZad = 1000;
		private int _xCurrent;
		private int _yCurrent;
		private int _zCurrent;

		private Bar _bar;
		private readonly Programmator _programmator;
		private string _horizontalSurface;
		private string _verticalSurface;
		private bool _value;
		private DispatcherTimer _timer;
		private readonly Action<Point> _delete;
		private string _mode;
		private string _switch;
		private readonly IView _view;

		public MainWindowViewModel(IView view)
		{
			_view = view;
			_bar = new Bar(XBarMax, YBarMax, ZBarMax);
			_programmator = new Programmator(_bar);
			_mode = Modes.Settings;
			_switch = Modes.Off;
			TickCommand = new DelegateCommand(() => ProgrammatorTick(null, null));
			StartCommand = new DelegateCommand(Start);
			StopCommand = new DelegateCommand(Stop);
            view.Start(_xMax, _yMax, _zMax);

			SetProgrammatorSettings();
		}

		public ICommand TickCommand
		{
			get;
		}

		public ICommand StartCommand
		{
			get;
		}

		public ICommand StopCommand
		{
			get;
		}

		private void SetProgrammatorSettings()
		{
			_programmator.Settings.XMax = XMax;
			_programmator.Settings.YMax = YMax;
			_programmator.Settings.ZMax = ZMax;
		}

		public void UpdateSurfaces(Point point = null)
		{
			VerticalSurface = _bar.GetZYSurface(_xCurrent, _yCurrent, _zCurrent);
			HorizontalSurface = _bar.GetXYSurface(_xCurrent, _yCurrent, _zCurrent);
		}

		public int XMax
		{
			get => _xMax;
			set
			{
				if (value < 1 || value > XBarMax) return;
				if (SetField(ref _xMax, value))
					SetProgrammatorSettings();
			}
		}

		public int YMax
		{
			get => _yMax;
			set
			{
				if (value < 1 || value > YBarMax) return;
				if (SetField(ref _yMax, value))
					SetProgrammatorSettings();
			}
		}

		public int ZMax
		{
			get => _zMax;
			set
			{
				if (value < 1 || value > ZBarMax) return;

				if (SetField(ref _zMax, value))
					SetProgrammatorSettings();
			}
		}

		// ReSharper disable once InconsistentNaming
		public int TZad
		{
			get => _tZad;
			set => SetField(ref _tZad, value);
		}

		public int XCurrent
		{
			get => _xCurrent;
			set
			{
				if (SetField(ref _xCurrent, value))
					UpdateSurfaces();
			}
		}

		public int YCurrent
		{
			get => _yCurrent;
			set
			{
				if (SetField(ref _yCurrent, value))
					UpdateSurfaces();
			}
		}

		public int ZCurrent
		{
			get => _zCurrent;
			set
			{
				if (SetField(ref _zCurrent, value))
					UpdateSurfaces();
			}
		}

		public string HorizontalSurface
		{
			get => _horizontalSurface;
			set => SetField(ref _horizontalSurface, value);
		}

		public string VerticalSurface
		{
			get => _verticalSurface;
			set => SetField(ref _verticalSurface, value);
		}

		//public bool Value
		//{
		//	get => _value = _bar.GetValue(XCurrent, YCurrent, ZCurrent);
		//	set
		//	{
		//		if (SetField(ref _value, value))
		//		{
		//			_bar.SetValue(XCurrent, YCurrent, ZCurrent, value);
		//			UpdateSurfaces();
		//		}
		//	}
		//}

		public string Mode
		{
			get => _mode;
			set
			{
				if (SetField(ref _mode, value))
					RaisePropertyChanged(nameof(ModeIsSettings));
			}
		}

		public string Switch
		{
			get => _switch;
			set { SetField(ref _switch, value); }
		}

		public bool ModeIsSettings => Mode == Modes.Settings;

		public void ProgrammatorTick(object e, EventArgs args)
		{
			var tickData = _programmator.TickVersion2();

			if (tickData.Finished)
			{
				_timer?.Stop();
				_view.ShowMessageBox();
				return;
			}

			var deletedPoint = tickData.DeletedPoint;

			if (deletedPoint != null)
			{
				XCurrent = deletedPoint.X;
				YCurrent = deletedPoint.Y;
				ZCurrent = deletedPoint.Z;
			}

			UpdateSurfaces(deletedPoint);
			if (deletedPoint != null)
				_view.Destroy(deletedPoint);
		}

		public void Start()
		{
			if (TZad == 0) return;

			_timer = new DispatcherTimer();
			_timer.Tick += ProgrammatorTick;
			_timer.Interval = new TimeSpan(0, 0, 0, 0, TZad);
			_timer.Start();
		}

		public void Stop()
		{
			_timer.Stop();
		}

        public void OnKeyDown(Key key)
        {
            switch (key)
            {
                case Key.F:
                    if (_switch != Modes.Off)
                        break;
                    Mode = Modes.Auto;
                    break;
                case Key.H:
                    if (_switch != Modes.Off)
                        break;
                    Mode = Modes.Hand;
                    break;
                case Key.Y:
                    if (_switch != Modes.Off)
                        break;
                    Mode = Modes.Settings;
                    break;
                case Key.G:
                    if (_mode != Modes.Auto || _switch != Modes.Off)
                        break;
                    Switch = Modes.On;
                    Start();
                    break;
                case Key.I:
                    if (_mode != Modes.Hand || _switch != Modes.Off)
                        break;
                    ProgrammatorTick(null, null);
                    break;
                case Key.C:
                    if (_mode != Modes.Auto || _switch != Modes.On)
                        break;
                    Switch = Modes.Off;
                    Stop();
                    break;
                case Key.R:
                    if (_switch != Modes.Off)
                        break;
                    _bar.Restart();
					_programmator.Restart();
                    _view.Start(_xMax, _yMax, _zMax);
                    break;
            }
        }

		private static class Modes
		{
			public const string Settings = "Настройка";
			public const string Hand = "Ручной";
			public const string Auto = "Автоформат";
			public const string Pause = "Остановка";

			public const string On = "Включен";
			public const string Off = "Выключен";
		}

		protected bool SetField<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value))
				return false;
			field = value;
#pragma warning disable 618
			OnPropertyChanged(propertyName);
#pragma warning restore 618
			return true;
		}
	}
}
