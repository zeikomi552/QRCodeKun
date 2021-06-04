using QRCodekun.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.QrCode;

namespace QRCodekun.Models
{
    public class ZxingM : QRCodeBase
    {
        #region 関数
        #region 誤り訂正率の定義をライブラリ用に変換
        /// <summary>
        /// 誤り訂正率の定義をライブラリ用に変換
        /// </summary>
        /// <param name="level">誤り訂正率</param>
        /// <returns>ライブラリ用に変換した結果</returns>
        public static ZXing.QrCode.Internal.ErrorCorrectionLevel ConvertEcL(QRCodeErrorCorrectionLevel level)
        {
            switch (level)
            {
                case QRCodeErrorCorrectionLevel.Low7Percent:
                default:
                    {
                        return ZXing.QrCode.Internal.ErrorCorrectionLevel.L;
                    }
                case QRCodeErrorCorrectionLevel.Medium15Percent:
                    {
                        return ZXing.QrCode.Internal.ErrorCorrectionLevel.M;
                    }
                case QRCodeErrorCorrectionLevel.Quality25Percent:
                    {
                        return ZXing.QrCode.Internal.ErrorCorrectionLevel.Q;
                    }
                case QRCodeErrorCorrectionLevel.HighQuality30Percent:
                    {
                        return ZXing.QrCode.Internal.ErrorCorrectionLevel.H;
                    }
            }
        }
        #endregion

        #region QRコード作成関数
        /// <summary>
        /// QRコード作成関数
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="level">誤り訂正率</param>
        /// <param name="version">QRコードバージョン</param>
        /// <param name="encode">エンコード</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <returns>ビットマップイメージ</returns>
        public static BitmapImage Create(string text,
            ZXing.QrCode.Internal.ErrorCorrectionLevel level,
            int version = 2,
            string encode = "Shift_JIS", int width=200, int height=200)
        {
            try
            {
                BarcodeWriter writer = new BarcodeWriter();
                BarcodeFormat format = BarcodeFormat.QR_CODE;

                QrCodeEncodingOptions options = new QrCodeEncodingOptions()
                {
                    //CharacterSet = "UTF-8",
                    ErrorCorrection = level,
                    QrVersion = version,
                    Height = height,
                    Width = width,
                };

                //文字コード
                writer.Options.Hints.Add(ZXing.EncodeHintType.CHARACTER_SET, encode);

                writer.Options = options;
                writer.Format = format;


                // 文字列を指定してQRコードを生成
                using (var image = writer.Write(text))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // ファイルに保存
                        image.Save(ms, ImageFormat.Png);
                        return StreamToBitmapImage(ms);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
        #endregion
    }
}
