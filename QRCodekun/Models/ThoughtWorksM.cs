using QRCodekun.ViewModels;
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
        public static int ConvertEcL(QRCodeErrorCorrectionLevel level)
        {
            return (int)level;
        }

        /// <summary>
        /// QRコード作成関数
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="qr_ver">QRコードバージョン</param>
        /// <param name="err_correct">誤り訂正率</param>
        /// <param name="scale">QRコードサイズ</param>
        /// <returns>ビットマップイメージ</returns>
        public static BitmapImage Create(string text, int err_correct = 1
            , int qr_ver = 2, int scale = 4)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var sjis = Encoding.GetEncoding("shift_jis");

            return Create(text, sjis, qr_ver, err_correct, scale);

        }


        /// <summary>
        /// QRコード作成関数
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="enc">エンコード</param>
        /// <param name="qr_ver">バージョン</param>
        /// <param name="err_correct">誤り訂正率</param>
        /// <param name="scale">QRコードサイズ</param>
        /// <returns>ビットマップイメージ</returns>
        public static BitmapImage Create(string text, Encoding enc,
            int qr_ver = 2, int err_correct = 1, int scale = 1)
        {
            try
            {
                QRCodeEncoder qrEnc = new QRCodeEncoder();

                // エンコードは英数字
                qrEnc.QRCodeEncodeMode =
                    QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC;

                // エラー訂正はM
                qrEnc.QRCodeErrorCorrect = (QRCodeEncoder.ERROR_CORRECTION)err_correct;

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
