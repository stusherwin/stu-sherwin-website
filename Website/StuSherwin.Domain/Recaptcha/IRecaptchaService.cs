﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StuSherwin.Domain.Recaptcha
{
    public interface IRecaptchaService
    {
        RecaptchaValidationResponse ValidateResponse(string challenge, string response, string ipAddress);
    }
}
