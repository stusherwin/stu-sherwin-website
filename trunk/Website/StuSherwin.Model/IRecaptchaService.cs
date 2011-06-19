using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StuSherwin.Model
{
    public interface IRecaptchaService
    {
        RecaptchaValidationResponse ValidateResponse(string challenge, string response, string ipAddress);
    }
}
