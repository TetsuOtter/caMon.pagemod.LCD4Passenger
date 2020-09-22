using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace caMon.pagemod.LCD4Passenger.InfoDisp
{
	public class CommonPage<T> : Grid
		where T : IContainsRouteInfoManager, IContainsPanelInfo
	{
		public CommonPage(in T arg)
		{
			Height = arg.PanelInfo.PanelHeight;
			Width = arg.PanelInfo.PanelWidth;
			Margin = new Thickness(0);

			//H/W (16:10で0.625, 16:9で0.5625, 16:3で0.1875, 4:3で0.75) => 0.4(2:5)未満で分割タイプとする
			if ((double)arg.PanelInfo.PanelHeight / arg.PanelInfo.PanelWidth < ConstantValues.IsCutLCD_AspectThreshold)
				Children.Add(new CutDisp(arg));
			else
			{
				RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });//上部領域
				RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });//下部領域

				UIElement uie1 = new UpperArea();
				UIElement uie2 = new LowerArea(arg);
				Grid.SetRow(uie1, 0);
				Grid.SetRow(uie2, 1);

				Children.Add(uie1);
				Children.Add(uie2);
			}

			Background = new SolidColorBrush(Colors.Blue);
		}
	}
}
