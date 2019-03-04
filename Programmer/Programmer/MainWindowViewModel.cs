using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

	    private readonly Bar _bar = new Bar(10, 10, 10 , 'Ж');
	    private string _horizontalSurface;
	    private string _verticalSurface;
	    private bool _value;

	    public MainWindowViewModel()
        {
            
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
			        VerticalSurface = _bar.GetVerticalSurfuce(value);
	        }
        }

	    public int YCurrent
        {
            get => _yCurrent;
            set => SetField(ref _yCurrent, value);
        }

        public int ZCurrent
        {
            get => _zCurrent;
	        set
	        {
		        if (SetField(ref _zCurrent, value))
			        HorizontalSurface = _bar.GetHorizontalSurface(value);
	        }
        }

	    public string HorizontalSurface
	    {
		    get => _horizontalSurface;
		    set { SetField(ref _horizontalSurface, value); }
	    }

	    public string VerticalSurface
		{
			get => _verticalSurface;
			set
			{
				SetField(ref _verticalSurface, value);
			}
		}

	    public bool Value
		{
			get => _value;
			set
			{
				if (SetField(ref _value, value))
				{
					_bar.SetValue(XCurrent, YCurrent, ZCurrent, value);

					VerticalSurface = _bar.GetVerticalSurfuce(XCurrent);
					HorizontalSurface = _bar.GetHorizontalSurface(ZCurrent);
				}
			}
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
