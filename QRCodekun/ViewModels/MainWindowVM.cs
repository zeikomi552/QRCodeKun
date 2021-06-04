using MVVMCore.BaseClass;
using QRCodekun.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ThoughtWorks.QRCode.Codec;
using ZXing;
using ZXing.QrCode;

namespace QRCodekun.ViewModels
{
	public enum QRCodeErrorCorrectionLevel
	{
		Low7Percent = 0,
		Medium15Percent = 1,
		Quality25Percent = 2,
		HighQuality30Percent = 3
	}


	public class MainWindowVM : ViewModelBase
	{
		#region COMポート[COMPort]プロパティ
		/// <summary>
		/// COMポート[COMPort]プロパティ用変数
		/// </summary>
		int _COMPort = 3;
		/// <summary>
		/// COMポート[COMPort]プロパティ
		/// </summary>
		public int COMPort
		{
			get
			{
				return _COMPort;
			}
			set
			{
				if (!_COMPort.Equals(value))
				{
					_COMPort = value;
					NotifyPropertyChanged("COMPort");
				}
			}
		}
		#endregion


		#region DotNetBarcodeのQRコードオブジェクト[DotNetBarcodeQRCode]プロパティ
		/// <summary>
		/// DotNetBarcodeのQRコードオブジェクト[DotNetBarcodeQRCode]プロパティ用変数
		/// </summary>
		DotNetBarcodeM _DotNetBarcodeQRCode = new DotNetBarcodeM();
		/// <summary>
		/// DotNetBarcodeのQRコードオブジェクト[DotNetBarcodeQRCode]プロパティ
		/// </summary>
		public DotNetBarcodeM DotNetBarcodeQRCode
		{
			get
			{
				return _DotNetBarcodeQRCode;
			}
			set
			{
				if (_DotNetBarcodeQRCode == null || !_DotNetBarcodeQRCode.Equals(value))
				{
					_DotNetBarcodeQRCode = value;
					NotifyPropertyChanged("DotNetBarcodeQRCode");
				}
			}
		}
		#endregion

		#region ZxingMのQRコードオブジェクト[ZxingQRCode]プロパティ
		/// <summary>
		/// ZxingMのQRコードオブジェクト[ZxingQRCode]プロパティ用変数
		/// </summary>
		ZxingM _ZxingQRCode = new ZxingM();
		/// <summary>
		/// ZxingMのQRコードオブジェクト[ZxingQRCode]プロパティ
		/// </summary>
		public ZxingM ZxingQRCode
		{
			get
			{
				return _ZxingQRCode;
			}
			set
			{
				if (_ZxingQRCode == null || !_ZxingQRCode.Equals(value))
				{
					_ZxingQRCode = value;
					NotifyPropertyChanged("ZxingQRCode");
				}
			}
		}
		#endregion

		#region ThoughtWorksQRコードオブジェクト[ThoughtWorksQRCode]プロパティ
		/// <summary>
		/// ThoughtWorksQRコードオブジェクト[ThoughtWorksQRCode]プロパティ用変数
		/// </summary>
		ThoughtWorksM _ThoughtWorksQRCode = new ThoughtWorksM();
		/// <summary>
		/// ThoughtWorksQRコードオブジェクト[ThoughtWorksQRCode]プロパティ
		/// </summary>
		public ThoughtWorksM ThoughtWorksQRCode
		{
			get
			{
				return _ThoughtWorksQRCode;
			}
			set
			{
				if (_ThoughtWorksQRCode == null || !_ThoughtWorksQRCode.Equals(value))
				{
					_ThoughtWorksQRCode = value;
					NotifyPropertyChanged("ThoughtWorksQRCode");
				}
			}
		}
		#endregion

		#region 読み取り結果[ReaderText]プロパティ
		/// <summary>
		/// 読み取り結果[ReaderText]プロパティ用変数
		/// </summary>
		string _ReaderText = string.Empty;
		/// <summary>
		/// 読み取り結果[ReaderText]プロパティ
		/// </summary>
		public string ReaderText
		{
			get
			{
				return _ReaderText;
			}
			set
			{
				if (!_ReaderText.Equals(value))
				{
					_ReaderText = value;
					NotifyPropertyChanged("ReaderText");
				}
			}
		}
		#endregion

		#region QRコード文字列[QRCodeText]プロパティ
		/// <summary>
		/// QRコード文字列[QRCodeText]プロパティ用変数
		/// </summary>
		string _QRCodeText = string.Empty;
        /// <summary>
        /// QRコード文字列[QRCodeText]プロパティ
        /// </summary>
        public string QRCodeText
        {
            get
            {
                return _QRCodeText;
            }
            set
            {
                if (!_QRCodeText.Equals(value))
                {
                    _QRCodeText = value;
                    NotifyPropertyChanged("QRCodeText");
                }
            }
        }
		#endregion

		#region QRコードバージョン[QRCodeVersion]プロパティ
		/// <summary>
		/// QRコードバージョン[QRCodeVersion]プロパティ用変数
		/// </summary>
		Dictionary<int, string> _QRCodeVersion = new Dictionary<int, string>(){
			{1,"1"},{2,"2"},{3,"3"},{4,"4"},{5,"5"},{6,"6"},{7,"7"},{8,"8"},{9,"9"},{10,"10"},
			{11,"11"},{12,"12"},{13,"13"},{14,"14"},{15,"15"},{16,"16"},{17,"17"},{18,"18"},{19,"19"},{20,"20"},
			{21,"21"},{22,"22"},{23,"23"},{24,"24"},{25,"25"},{26,"26"},{27,"27"},{28,"28"},{29,"29"},{30,"30"},
			{31,"31"},{32,"32"},{33,"33"},{34,"34"},{35,"35"},{36,"36"},{37,"37"},{38,"38"},{39,"39"},{40,"40"},
			};
		/// <summary>
		/// QRコードバージョン[QRCodeVersion]プロパティ
		/// </summary>
		public Dictionary<int, string> QRCodeVersion
		{
			get
			{
				return _QRCodeVersion;
			}
			set
			{
				if (_QRCodeVersion == null || !_QRCodeVersion.Equals(value))
				{
					_QRCodeVersion = value;
					NotifyPropertyChanged("QRCodeVersion");
				}
			}
		}
		#endregion
		#region 誤り訂正率[QRCorrectRatio]プロパティ
		/// <summary>
		/// 誤り訂正率[QRCorrectRatio]プロパティ用変数
		/// </summary>
		Dictionary<QRCodeErrorCorrectionLevel, QRCodeErrorCorrectionLevel> _QRCorrectRatio = new Dictionary<QRCodeErrorCorrectionLevel, QRCodeErrorCorrectionLevel>()
		{
			{ QRCodeErrorCorrectionLevel.Low7Percent, QRCodeErrorCorrectionLevel.Low7Percent },
			{ QRCodeErrorCorrectionLevel.Medium15Percent, QRCodeErrorCorrectionLevel.Medium15Percent },
			{ QRCodeErrorCorrectionLevel.Quality25Percent, QRCodeErrorCorrectionLevel.Quality25Percent },
			{ QRCodeErrorCorrectionLevel.HighQuality30Percent, QRCodeErrorCorrectionLevel.HighQuality30Percent },
		};
		/// <summary>
		/// 誤り訂正率[QRCorrectRatio]プロパティ
		/// </summary>
		public Dictionary<QRCodeErrorCorrectionLevel, QRCodeErrorCorrectionLevel> QRCorrectRatio
		{
			get
			{
				return _QRCorrectRatio;
			}
			set
			{
				if (_QRCorrectRatio == null || !_QRCorrectRatio.Equals(value))
				{
					_QRCorrectRatio = value;
					NotifyPropertyChanged("QRCorrectRatio");
				}
			}
		}
		#endregion
		#region 選択されている誤り訂正率[SelectedQRCorrectRatio]プロパティ
		/// <summary>
		/// 選択されている誤り訂正率[SelectedQRCorrectRatio]プロパティ用変数
		/// </summary>
		QRCodeErrorCorrectionLevel _SelectedQRCorrectRatio = 0;
		/// <summary>
		/// 選択されている誤り訂正率[SelectedQRCorrectRatio]プロパティ
		/// </summary>
		public QRCodeErrorCorrectionLevel SelectedQRCorrectRatio
		{
			get
			{
				return _SelectedQRCorrectRatio;
			}
			set
			{
				if (!_SelectedQRCorrectRatio.Equals(value))
				{
					_SelectedQRCorrectRatio = value;
					NotifyPropertyChanged("SelectedQRCorrectRatio");
				}
			}
		}
		#endregion


		#region 選択されているQRコードのバージョン[SelectedQRCodeVersion]プロパティ
		/// <summary>
		/// 選択されているQRコードのバージョン[SelectedQRCodeVersion]プロパティ用変数
		/// </summary>
		int _SelectedQRCodeVersion = 2;
		/// <summary>
		/// 選択されているQRコードのバージョン[SelectedQRCodeVersion]プロパティ
		/// </summary>
		public int SelectedQRCodeVersion
		{
			get
			{
				return _SelectedQRCodeVersion;
			}
			set
			{
				if (!_SelectedQRCodeVersion.Equals(value))
				{
					_SelectedQRCodeVersion = value;
					NotifyPropertyChanged("SelectedQRCodeVersion");
				}
			}
		}
		#endregion


		public override void Init(object sender, EventArgs e)
        {

		}

        public override void Close(object sender, EventArgs e)
        {
		}

		public void CreateQRCode()
        {
			try
			{
				this.DotNetBarcodeQRCode.ErrorText = string.Empty;

				Stopwatch sw = new Stopwatch();
				sw.Start();

				// DotNetBarcode
				this.DotNetBarcodeQRCode.QRCodeImage
					= DotNetBarcodeM.Create(this.QRCodeText, 
					DotNetBarcodeM.ConvertEcL(this.SelectedQRCorrectRatio),
					(DotNetBarcode.QRVersions)this.SelectedQRCodeVersion);

				sw.Stop();
				this.DotNetBarcodeQRCode.Erapsed = sw.Elapsed.ToString();
			}
			catch (Exception e)
			{
				this.DotNetBarcodeQRCode.ErrorText = e.Message;
			}

			try
			{
				this.ThoughtWorksQRCode.ErrorText = string.Empty;
				Stopwatch sw = new Stopwatch();
				sw.Start();

				// ThoughtWorks
				this.ThoughtWorksQRCode.QRCodeImage 
					= ThoughtWorksM.Create(this.QRCodeText,
					ThoughtWorksM.ConvertEcL(this.SelectedQRCorrectRatio), 
					this.SelectedQRCodeVersion);

				sw.Stop();
				this.ThoughtWorksQRCode.Erapsed = sw.Elapsed.ToString();
			}
			catch (Exception e)
			{
				this.ThoughtWorksQRCode.ErrorText = e.Message;
			}

			try
			{
				this.ZxingQRCode.ErrorText = string.Empty;

				Stopwatch sw = new Stopwatch();
				sw.Start();

				// Zxing
				this.ZxingQRCode.QRCodeImage
					= ZxingM.Create(this.QRCodeText, ZxingM.ConvertEcL(this.SelectedQRCorrectRatio), this.SelectedQRCodeVersion);

				sw.Stop();
				this.ZxingQRCode.Erapsed = sw.Elapsed.ToString();
			}
			catch (Exception e)
			{
				this.ZxingQRCode.ErrorText = e.Message;
			}


		}



		static System.IO.Ports.SerialPort _serial;

		/// <summary>
		/// スキャナ接続処理
		/// </summary>
        public void Connect()
        {
            _serial = new SerialPort("COM"+this.COMPort.ToString());   // COMの名前を指定　デバイスマネージャーでDENSO WAVE Active USB-COM Portとなっているやつを探す

			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			_serial.Encoding = Encoding.GetEncoding("shift_jis");
			
			_serial.DataReceived -= Recieved;   // シリアルポートでデータを受けた時のイベント　一旦クリア
            _serial.DataReceived += Recieved;    // シリアルポートでデータを受けた時のイベント登録

            // シリアルポートオープン
            _serial.Open();

            _serial.DtrEnable = true;  // DTRの有効化
            _serial.RtsEnable = true;  // RTSの有効化

            System.Threading.Thread.Sleep(50);  // 安定するまで一瞬待つ

            _serial.WriteLine("U1" + '\r');      // オートオフモード
            System.Threading.Thread.Sleep(50);

            _serial.WriteLine("Z" + '\r');      // 読み取り待機
            System.Threading.Thread.Sleep(50);

            _serial.WriteLine("R" + '\r');      // 読み取り可能状態に入る
        }

		/// <summary>
		/// スキャナ切断処理
		/// </summary>
		public void Disconnect()
		{
			// COMポートのクローズ
			_serial.Close();

			// オブジェクトの破棄
			_serial.Dispose();
		}

		void Recieved(object sender, SerialDataReceivedEventArgs e)
        {
			string text = _serial.ReadExisting().Trim();
			byte[] data = System.Text.Encoding.GetEncoding("shift_jis").GetBytes(text);


			// シリアルポートからのデータ吸出し
			this.ReaderText += text + " : ";
			foreach (var tmp in data)
			{
				this.ReaderText += tmp.ToString("x2") + " ";
			}
			this.ReaderText += "\r\n";
        }

		public void Clear()
        {
			this.ReaderText = string.Empty;
        }

	}
}
