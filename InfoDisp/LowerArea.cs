using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace caMon.pagemod.LCD4Passenger.InfoDisp
{
	public class LowerArea : Border
	{
		//初期状態ではデフォルト表示を表示する

		//常に各ページを保持しておく.
		IContainsRouteInfoManager ContainsRouteInfoManagerInstance;
		public LowerArea(in IContainsRouteInfoManager arg)
		{
			ContainsRouteInfoManagerInstance = arg;
			Child = new Label()
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Content = "LowerArea",
				FontSize = 64,
				Foreground = new SolidColorBrush(Colors.Red)
			};
			Background = new SolidColorBrush(Colors.Aqua);
			Margin = new Thickness(0);
			HorizontalAlignment = HorizontalAlignment.Center;
			VerticalAlignment = VerticalAlignment.Center;
		}
	}
}
