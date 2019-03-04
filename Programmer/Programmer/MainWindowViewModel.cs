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


        public MainWindowViewModel()
        {
            
        }


        public int XMax
        {
            get => _xMax;
            set { SetField(ref _xMax, value); }
        }

        public int YMax
        {
            get => _yMax;
            set { SetField(ref _yMax, value); }
        }

        public int ZMax
        {
            get => _zMax;
            set { SetField(ref _zMax, value); }
        }

        public int TZad
        {
            get => _tZad;
            set { SetField(ref _tZad, value); }
        }

        public int XCurrent
        {
            get => _xCurrent;
            set { SetField(ref _xCurrent, value); }
        }

        public int YCurrent
        {
            get => _yCurrent;
            set { SetField(ref _yCurrent, value); }
        }

        public int ZCurrent
        {
            get => _zCurrent;
            set { SetField(ref _zCurrent, value); }
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
