namespace Programmer
{
	public interface IView
	{
		void Destroy(Point point);
		void ShowMessageBox();
        void Start(int x, int y, int z);
    }
}
