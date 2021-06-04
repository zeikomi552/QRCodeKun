using QRCodekun.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QRCodekun.Models
{
    public class DotNetBarcodeM : QRCodeBase
    {
        #region 関数
        #region 誤り訂正率の定義をライブラリ用に変換
        /// <summary>
        /// 誤り訂正率の定義をライブラリ用に変換
        /// </summary>
        /// <param name="level">誤り訂正率</param>
        /// <returns>ライブラリ用に変換した結果</returns>
        public static DotNetBarcode.QRECCRates ConvertEcL(QRCodeErrorCorrectionLevel level)
        {
            switch (level)
            {
                case QRCodeErrorCorrectionLevel.Low7Percent:
                default:
                    {
                        return DotNetBarcode.QRECCRates.Low7Percent;
                    }
                case QRCodeErrorCorrectionLevel.Medium15Percent:
                    {
                        return DotNetBarcode.QRECCRates.Medium15Percent;
                    }
                case QRCodeErrorCorrectionLevel.Quality25Percent:
                    {
                        return DotNetBarcode.QRECCRates.Quality25Percent;
                    }
                case QRCodeErrorCorrectionLevel.HighQuality30Percent:
                    {
                        return DotNetBarcode.QRECCRates.HighQuality30Percent;
                    }
            }
        }
        #endregion

        #region QRコードの作成処理
        /// <summary>
        /// QRコードの作成処理
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="err_correct">誤り訂正率</param>
        /// <param name="qr_ver">QRコードバージョン</param>
        /// <param name="width">幅</param>
        /// <param name="height">高さ</param>
        /// <param name="quit_zone">クワイエットゾーン</param>
        /// <returns>QRコードイメージ</returns>
        public static BitmapImage Create(string text,
            DotNetBarcode.QRECCRates err_correct = DotNetBarcode.QRECCRates.Medium15Percent,
            DotNetBarcode.QRVersions qr_ver = DotNetBarcode.QRVersions.Ver02,
            int width = 200, int height = 200, int quit_zone = 2
            )
        {
            try
            {
                DotNetBarcode makeQR = new DotNetBarcode();
                makeQR.Type = DotNetBarcode.Types.QRCode;
                makeQR.QRSetECCRate = err_correct;
                makeQR.QRSetVersion = qr_ver;
                makeQR.QRQuitZone = quit_zone;

                //描画先とするImageオブジェクトを作成する
                using (var canvas = new Bitmap(width, height))
                {
                    //ImageオブジェクトのGraphicsオブジェクトを作成する
                    using (Graphics g = Graphics.FromImage(canvas))
                    {
                        makeQR.WriteBar(text, 0, 0, width, height, g);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            canvas.Save(ms, ImageFormat.Png);
                            return StreamToBitmapImage(ms);
                        }
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
