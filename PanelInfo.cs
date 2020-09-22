namespace caMon.pagemod.LCD4Passenger
{
	public interface IContainsPanelInfo
	{
		public IPanelInfo PanelInfo { get; }
	}
	public interface IPanelInfo
	{
		/// <summary>識別用の唯一名</summary>
		public string UniqueName { get; }

		/// <summary>号車番号</summary>
		public int CarNumber { get; }

		/// <summary>車両内でのドア番号(0の場合 : 対応するドアなし.  5系の3面広告など)</summary>
		public int DoorNumberInThisCar { get; }

		/// <summary>表示器の画面高さ[px]</summary>
		public int PanelHeight { get; }

		/// <summary>表示器の画面幅[px]</summary>
		public int PanelWidth { get; }

		/// <summary>表示器画面を確認する際に向く必要がある方向(進行方向基準)</summary>
		public Direction DirectionToSee { get; }

		/// <summary>方向情報</summary>
		public enum Direction
		{
			/// <summary>情報なし</summary>
			None,
			/// <summary>左</summary>
			Left,
			/// <summary>右</summary>
			Right,
			/// <summary>前</summary>
			Front,
			/// <summary>後</summary>
			Back
		}
	}
	/// <summary>表示器の情報を格納する</summary>
	public class ReadOnlyPanelInfo : IPanelInfo
	{
		public string UniqueName { get; protected set; }
		public int CarNumber { get; protected set; }
		public int DoorNumberInThisCar { get; protected set; }
		public int PanelHeight { get; protected set; }
		public int PanelWidth { get; protected set; }
		public IPanelInfo.Direction DirectionToSee { get; protected set; }
	}

	public class PanelInfo : ReadOnlyPanelInfo
	{
		public new string UniqueName { get; set; }
		public new int CarNumber { get; set; }
		public new int DoorNumberInThisCar { get; set; }
		public new int PanelHeight { get; set; }
		public new int PanelWidth { get; set; }
		public new IPanelInfo.Direction DirectionToSee { get; set; }
	}
}
