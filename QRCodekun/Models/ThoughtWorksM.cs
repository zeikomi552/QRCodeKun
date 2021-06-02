using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ThoughtWorks.QRCode.Codec;

namespace QRCodekun.Models
{
    public class ThoughtWorksM : QRCodeBase
    {

        public static BitmapImage Create(string text, 
            int qr_ver = 2, int scale = 4, int err_correct = 1)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var sjis = Encoding.GetEncoding("shift_jis");

            return Create(text, sjis, qr_ver, scale, err_correct);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="enc"></param>
        /// <param name="qr_ver"></param>
        /// <param name="scale"></param>
        /// <param name="error_correct"></param>
        /// <returns></returns>
        public static BitmapImage Create(string text, Encoding enc, int qr_ver, int scale, int error_correct)
        {
            try
            {
                QRCodeEncoder qrEnc = new QRCodeEncoder();

                // エンコードは英数字
                qrEnc.QRCodeEncodeMode =
                    QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;

                // エラー訂正はM
                qrEnc.QRCodeErrorCorrect = (QRCodeEncoder.ERROR_CORRECTION)error_correct;

                qrEnc.QRCodeVersion = qr_ver; // バージョン（1～40）
                qrEnc.QRCodeScale = scale; // 1セルのピクセル数

                // 文字列を指定してQRコードを生成
                using (var image = qrEnc.Encode(text, enc))
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
