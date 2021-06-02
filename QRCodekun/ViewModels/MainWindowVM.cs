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
    public class MainWindowVM : ViewModelBase
    {
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


        public override void Init(object sender, EventArgs e)
        {
			// 接続
			Connect();
		}

        public override void Close(object sender, EventArgs e)
        {
			// COMポートのクローズ
			_serial.Close();

			// オブジェクトの破棄
			_serial.Dispose();
		}

        public void CreateQRCode()
        {
			try
			{
				this.DotNetBarcodeQRCode.ErrorText = string.Empty;

				Stopwatch sw = new Stopwatch();
				sw.Start();

				// DotNetBarcode
				this.DotNetBarcodeQRCode.QRCodeImage = DotNetBarcodeM.Create(this.QRCodeText);

				sw.Stop();
				this.DotNetBarcodeQRCode.ErapsedMillisecond = sw.ElapsedMilliseconds;
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
				this.ThoughtWorksQRCode.QRCodeImage = ThoughtWorksM.Create(this.QRCodeText);

				sw.Stop();
				this.ThoughtWorksQRCode.ErapsedMillisecond = sw.ElapsedMilliseconds;

				
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
				this.ZxingQRCode.QRCodeImage = ZxingM.Create(this.QRCodeText);

				sw.Stop();
				this.ZxingQRCode.ErapsedMillisecond = sw.ElapsedMilliseconds;
			}
			catch (Exception e)
			{
				this.ZxingQRCode.ErrorText = e.Message;
			}


		}



		static System.IO.Ports.SerialPort _serial;

        void Connect()
        {
            _serial = new SerialPort("COM3");   // COMの名前を指定　デバイスマネージャーでDENSO WAVE Active USB-COM Portとなっているやつを探す

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

        void Recieved(object sender, SerialDataReceivedEventArgs e)
        {
            // シリアルポートからのデータ吸出し
            this.ReaderText += _serial.ReadExisting();
        }

		public void Clear()
        {
			this.ReaderText = string.Empty;
        }

	}
}
