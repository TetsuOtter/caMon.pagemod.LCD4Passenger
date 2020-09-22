using System;
using System.Collections.Generic;

using TR.BIDSSMemLib;

namespace caMon.pagemod.LCD4Passenger
{
	public class RouteInfo
	{
		public string TrainNumber;

		/// <summary>上り列車かどうか[上り:true, 下り:false]</summary>
		public bool IsInbound { get; set; }

		/// <summary>駅情報一覧</summary>
		public List<StationInfo> StaInfoList { get; set; }

		/// <summary>駅0から各駅までの標準所要時間[秒]のリスト</summary>
		public List<int> StandardTravelTime { get; set; }
	}


	public class StationInfo
	{
		/// <summary>路線名</summary>
		public string LineName;

		/// <summary>駅情報がこの列車で使用されるかどうか(falseの場合, 路線に存在するが経由しない駅として扱われる)</summary>
		public bool IsEnabled;

		/// <summary>駅名</summary>
		public string StaName;
		
		/// <summary>停目位置[m]</summary>
		public double Location;
		
		/// <summary>奥方向の停車位置許容誤差[m](絶対値化されます)</summary>
		public double StopPos_Threshold_Positive;
		
		/// <summary>手前方向の停車位置許容誤差[m](絶対値化されます)</summary>
		public double StopPos_Threshold_Negative;
		
		/// <summary>停車駅かどうか</summary>
		public bool IsStop;
		
		/// <summary>左側のドアが開くかどうか</summary>
		public bool WillRightDoorOpen;
		
		/// <summary>右側のドアが開くかどうか</summary>
		public bool WillLeftDoorOpen;

		/// <summary>種別画像へのパス</summary>
		public string TrainTypeImagePath;

		/// <summary>前停車駅からこの駅までに通過駅が存在したかどうか</summary>
		public bool PassedThroughSomeStations;
		
		/// <summary>次の駅までのラインカラー[0xAARRGGBB]</summary>
		public uint LineColor;
		
		/// <summary>ナンバリング画像までのパス</summary>
		public string PathToNumberingImage;
		
		/// <summary>乗り換え先路線情報</summary>
		public List<TransferInfo> TransferInfoList;
		
		/// <summary>乗り換え先路線情報構造</summary>
		public class TransferInfo
		{
			/// <summary>路線名</summary>
			public string LineName;

			/// <summary>路線アイコンへのパス</summary>
			public string LineIconPath;
		}

		/* - 発着時刻は要る?
		 * - 進入出制限を入れたら揺れ警告を出せるよね.
		 */
	}


	public interface IContainsRouteInfoManager
	{
		public RouteInfoManager RouteInfoManager { get; }
	}

	/// <summary>ファイルから設定を読み込み, 管理するクラス</summary>
	public class RouteInfoManager
	{
		public event EventHandler<DoorStatusChangedEventArgs> DoorStatusChanged;
		public event EventHandler PassedThroughPassSta;
		public event EventHandler ArrivedToStopSta;
		public event EventHandler DepartedFromStopSta;

		public class DoorStatusChangedEventArgs : EventArgs
		{
			public DoorStatus NoDirDoorStatus;
			public DoorStatus LeftDoorStatus;
			public DoorStatus RightDoorStatus;

			public StationInfo CurrentStationInfo;//該当なしの場合はnull

			public enum DoorStatus
			{
				NoData,
				Closed,
				Opening,
				FullOpen,
				Error,
			}
		}

		//SMemLib SML;
		public readonly RouteInfo RouteInfo;
		int NextSta = 0;
		public RouteInfoManager(SMemLib sml, RouteInfo ri)
		{
			//SML = sml;
			RouteInfo = ri as RouteInfo;

			sml.SMC_BSMDChanged += Sml_SMC_BSMDChanged;
		}

		private void Sml_SMC_BSMDChanged(object sender, TR.ValueChangedEventArgs<BIDSSharedMemoryData> e)
		{
			StationInfo si = null;

			if (RouteInfo.StaInfoList?.Count > NextSta)
			{
				//LastOrCurrentStopStaがListに存在する

				bool IsInTheStopArea =
					(RouteInfo.StaInfoList[NextSta].Location - Math.Abs(RouteInfo.StaInfoList[NextSta].StopPos_Threshold_Negative)) < e.NewValue.StateData.Z &&
					e.NewValue.StateData.Z < (RouteInfo.StaInfoList[NextSta].Location + Math.Abs(RouteInfo.StaInfoList[NextSta].StopPos_Threshold_Positive));

				if (RouteInfo.StaInfoList[NextSta].IsStop &&//次が停車駅で
					e.OldValue.StateData.V != e.NewValue.StateData.V &&//速度情報に更新があり
					e.NewValue.StateData.V == 0 &&//新しい速度が0km/h(停止状態)であり
					IsInTheStopArea)//新しい距離が許容範囲内
					ArrivedToStopSta?.Invoke(this, null);//停車したと判定する.

				if (e.OldValue.IsDoorClosed != e.NewValue.IsDoorClosed)//ドア状態に変化があった場合
					DoorStatusChanged?.Invoke(this, new DoorStatusChangedEventArgs
					{
						NoDirDoorStatus = e.NewValue.IsDoorClosed ? DoorStatusChangedEventArgs.DoorStatus.Closed : DoorStatusChangedEventArgs.DoorStatus.FullOpen,
						LeftDoorStatus = DoorStatusChangedEventArgs.DoorStatus.NoData,
						RightDoorStatus = DoorStatusChangedEventArgs.DoorStatus.NoData,
						CurrentStationInfo = IsInTheStopArea//現在位置が許容範囲内
					? RouteInfo.StaInfoList[NextSta] : null//許容位置ならその駅情報を, そうでなければNULL
					});
			}
		}
	}

}
