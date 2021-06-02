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

        public static BitmapImage Create(string text, string encode = "Shift_JIS", int width=200, int height=200, int version=2)
        {
            try
            {
                BarcodeWriter writer = new BarcodeWriter();
                BarcodeFormat format = BarcodeFormat.QR_CODE;

                QrCodeEncodingOptions options = new QrCodeEncodingOptions()
                {
                    //CharacterSet = "UTF-8",
                    ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.M,
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

    }
}
