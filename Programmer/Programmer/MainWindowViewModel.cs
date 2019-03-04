using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace Programmer
{
    class MainWindowViewModel : BindableBase
    {
        private int _xMax;
        private int _yMax;
        private int _zMax;
        private int _tZad;
        private int _xCurrent;
        private int _yCurrent;
        private int _zCurrent;

	    private readonly Bar _bar = new Bar(10, 10, 10);
	    private string _horizontalSurface;
	    private string _verticalSurface;
	    private bool _value;

	    public MainWindowViewModel()
        {
			ClickCommand = new DelegateCommand(ChangeValue);   
        }

	    public ICommand ClickCommand
	    {
		    get;
	    }

		public void UpdateSurfaces()
	    {
		    VerticalSurface = _bar.GetZYSurface(XCurrent, YCurrent, ZCurrent);
		    HorizontalSurface = _bar.GetXYSurface(XCurrent, YCurrent, ZCurrent);
		}

        public int XMax
        {
            get => _xMax;
            set => SetField(ref _xMax, value);
        }

        public int YMax
        {
            get => _yMax;
            set => SetField(ref _yMax, value);
        }

        public int ZMax
        {
            get => _zMax;
            set => SetField(ref _zMax, value);
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

	    public void ChangeValue()
	    {
		    Value = !Value;
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
