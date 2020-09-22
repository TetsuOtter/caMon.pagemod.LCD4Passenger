using System.Windows.Controls;

using caMon.pagemod.LCD4Passenger.InfoDisp;

namespace caMon.pagemod.LCD4Passenger
{
	/// <summary>
	/// FrontPage.xaml の相互作用ロジック
	/// </summary>
	public partial class FrontPage : Page, IContainsRouteInfoManager, IContainsPanelInfo
	{
		public RouteInfoManager RouteInfoManager { get; private set; } = null;
		public IPanelInfo PanelInfo { get; }

		public FrontPage()
		{
			InitializeComponent();

			PanelInfo = new PanelInfo
			{
				PanelHeight = 768,
				PanelWidth = 1024,
				CarNumber = 1,
				DirectionToSee = IPanelInfo.Direction.Left,
				DoorNumberInThisCar = 1,
				UniqueName = "C1_D1_L"
			};

			MainBorder.Child = new CommonPage<FrontPage>(this);
		}

	}
}
