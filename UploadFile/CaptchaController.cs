using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace UploadFile
{
    public class CaptchaController : ApiController
    {

        //public IEnumerable<Captcha> GetCaptcha()
        //{
        //    string EncryptKey = "CaptchaTest";

        //    //CaptchaCreate captcha = new CaptchaCreate();
        //    //System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //    //captcha.CreateImage(ms);

        //    System.Collections.ObjectModel.Collection<Captcha> Captchas = new System.Collections.ObjectModel.Collection<Captcha>();

        //    Encrypting wrapper = new Encrypting(EncryptKey);
        //    string cipherText = wrapper.EncryptData(captcha.Text);

        //    Captchas.Add(new Captcha(cipherText, Convert.ToBase64String(ms.ToArray())));

        //    return Captchas;
        //}


    }

    public class Captcha
    {
        public string CaptchaText;

        public string CaptchaImage { get; set; }

        public Captcha(string CaptchaText, string CaptchaImage)
        {
            this.CaptchaText = CaptchaText;
            this.CaptchaImage = CaptchaImage;
        }
    }
}