using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace StuSherwin.Model
{
    public class RecaptchaService : IRecaptchaService
    {
        private string _verificationUrl;
        private string _privateKey;

        public RecaptchaService(string verificationUrl, string privateKey)
        {
            _verificationUrl = verificationUrl;
            _privateKey = privateKey;
        }

        public RecaptchaValidationResponse ValidateResponse(string challenge, string response, string ipAddress)
        {
            var verificationRequestContent = CreateRequestContent(challenge, response, ipAddress);
            var verificationRequest = CreateWebRequest(verificationRequestContent);
            var verificationResponse = verificationRequest.GetResponse();
            return VerificationResponseIsValid(verificationResponse)
                ? RecaptchaValidationResponse.Success
                : RecaptchaValidationResponse.Failure;
        }

        private string CreateRequestContent(string challenge, string response, string ipAddress)
        {
            var requestStringBuilder = new QueryStringBuilder();

            requestStringBuilder.Add("privatekey", _privateKey);
            requestStringBuilder.Add("remoteip", ipAddress);
            requestStringBuilder.Add("challenge", challenge);
            requestStringBuilder.Add("response", response);

            return requestStringBuilder.ToString();
        }

        private WebRequest CreateWebRequest(string requestContent)
        {
            var verificationRequest = WebRequest.Create(_verificationUrl);

            verificationRequest.Method = "POST";
            verificationRequest.ContentType = "application/x-www-form-urlencoded";

            SetRequestContent(verificationRequest, requestContent);

            return verificationRequest;
        }

        private bool VerificationResponseIsValid(WebResponse verificationResponse)
        {
            string responseContent = new StreamReader(verificationResponse.GetResponseStream()).ReadToEnd();
            string[] responseLines = responseContent.Split('\n');

            return responseLines[0] == "true";
        }

        private void SetRequestContent(WebRequest request, string requestContent)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(requestContent);
            request.ContentLength = byteArray.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(byteArray, 0, byteArray.Length);
            requestStream.Close();
        }
    }
}
