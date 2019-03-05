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
        private int _xMax;
        private int _yMax;
        private int _zMax;
        private int _tZad;
        private int _xCurrent;
        private int _yCurrent;
        private int _zCurrent;

	    private readonly Bar _bar;
	    private readonly Programmator _programmator;
		private string _horizontalSurface;
	    private string _verticalSurface;
	    private bool _value;
	    private DispatcherTimer _timer;

	    public MainWindowViewModel()
	    {
		    _bar = new Bar(10, 10, 10);
			_programmator = new Programmator(_bar);
			TickCommand = new DelegateCommand(() => ProgrammatorTick(null, null));   
			StartCommand = new DelegateCommand(Start);   
		    StopCommand = new DelegateCommand(Stop);
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

		public void UpdateSurfaces(Point point = default(Point))
	    {
		    if (!point.Equals(default(Point)))
		    {
			    _xCurrent = point.X;
			    _yCurrent = point.Y;
			    _zCurrent = point.Z;
			}

		    VerticalSurface = _bar.GetZYSurface(_xCurrent, _yCurrent, _zCurrent);
		    HorizontalSurface = _bar.GetXYSurface(_xCurrent, _yCurrent, _zCurrent);
		}

        public int XMax
        {
	        get => _xMax;
	        set
	        {
		        if(SetField(ref _xMax, value))
					SetProgrammatorSettings();
	        }
        }

	    public int YMax
        {
            get => _yMax;
		    set
		    {
			    if (SetField(ref _yMax, value))
				    SetProgrammatorSettings();
		    }
		}

        public int ZMax
        {
            get => _zMax;
	        set
	        {
		        if (SetField(ref _zMax, value))
			        SetProgrammatorSettings();
	        }
		}

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

	    public bool Value
		{
			get => _value = _bar.GetValue(XCurrent, YCurrent, ZCurrent);
			set
			{
				if (SetField(ref _value, value))
				{
					_bar.SetValue(XCurrent, YCurrent, ZCurrent, value);
					UpdateSurfaces();
				}
			}
		}

	    public void ProgrammatorTick(object e, EventArgs args)
	    {
			var point = _programmator.Tick();
			UpdateSurfaces(point);
	    }

	    public void Start()
	    {
			if(TZad == 0) return;

		    _timer = new DispatcherTimer();
		    _timer.Tick += ProgrammatorTick;
		    _timer.Interval = new TimeSpan(0, 0, 0, 0, TZad);
		    _timer.Start();
		}

	    public void Stop()
	    {
			_timer.Stop();
	    }

		protected bool SetField<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
