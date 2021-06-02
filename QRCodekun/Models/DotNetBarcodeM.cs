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

        public static BitmapImage Create(string text, 
            int width = 200, int height = 200, int quit_zone = 2,
            DotNetBarcode.QRVersions qr_ver = DotNetBarcode.QRVersions.Ver02,
            DotNetBarcode.QRECCRates err_correct =  DotNetBarcode.QRECCRates.Medium15Percent)
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
    }
}
