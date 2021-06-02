using MVVMCore.BaseClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QRCodekun.Models
{
    public class QRCodeBase : ModelBase
	{
		#region [ErrorText]プロパティ
		/// <summary>
		/// [ErrorText]プロパティ用変数
		/// </summary>
		string _ErrorText = string.Empty;
		/// <summary>
		/// [ErrorText]プロパティ
		/// </summary>
		public string ErrorText
		{
			get
			{
				return _ErrorText;
			}
			set
			{
				if (!_ErrorText.Equals(value))
				{
					_ErrorText = value;
					NotifyPropertyChanged("ErrorText");
				}
			}
		}
		#endregion
		#region 経過時間[ErapsedMillisecond]プロパティ
		/// <summary>
		/// 経過時間[ErapsedMillisecond]プロパティ用変数
		/// </summary>
		long _ErapsedMillisecond = 0;
		/// <summary>
		/// 経過時間[ErapsedMillisecond]プロパティ
		/// </summary>
		public long ErapsedMillisecond
		{
			get
			{
				return _ErapsedMillisecond;
			}
			set
			{
				if (!_ErapsedMillisecond.Equals(value))
				{
					_ErapsedMillisecond = value;
					NotifyPropertyChanged("ErapsedMillisecond");
				}
			}
		}
		#endregion


		#region QRコード[QRCodeImage]プロパティ
		/// <summary>
		/// QRコード[QRCodeImage]プロパティ用変数
		/// </summary>
		BitmapImage _QRCodeImage = new BitmapImage();
		/// <summary>
		/// QRコード[QRCodeImage]プロパティ
		/// </summary>
		public BitmapImage QRCodeImage
		{
			get
			{
				return _QRCodeImage;
			}
			set
			{
				if (_QRCodeImage == null || !_QRCodeImage.Equals(value))
				{
					_QRCodeImage = value;
					NotifyPropertyChanged("QRCodeImage");
				}
			}
		}
		#endregion


		protected static BitmapImage StreamToBitmapImage(MemoryStream ms)
        {
            // MemoryStreamをシーク
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();

            // MemoryStreamを書き込むために準備する
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
            bitmapImage.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.None;

            // MemoryStreamを書き込む
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();
            return bitmapImage;
        }

    }
}
