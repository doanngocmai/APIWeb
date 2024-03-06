using API.Models;
using APIProject.Common.Utils;
using APIProject.Service.Interfaces;
using APIProject.Service.Utils;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using SmsSami;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Service.Services
{
    public class SendSmsService : ISendSmsService
    {
        public async Task<string> SendSms(string phone, string otp, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                // clientId - clientId của hệ thống API gửi sms, luôn luôn là "efa66179-1eb9-4187-9c0f-52fc99388492"
                const string clientId = SystemParam.SMS_CLIENT_ID;
                const string userName = SystemParam.SMS_USERNAME;
                const string password = SystemParam.SMS_PASSWORD;
                const int cooperateId = SystemParam.SMS_COOPERATE_ID;
                ITokenClient clientToken = new TokenClient(SystemParam.SMS_TOKEN_URL, userName, password, clientId);

                ISmsClient smsClient = new SmsClient(SystemParam.SMS_URL);

                
                var folderName = Path.Combine("PrivateKey", "id_rsa");
                string privateKeyPath = Path.Combine(webHostEnvironment.WebRootPath, folderName);
                var provider = Crypto.PemKeyUtils.GetRSAProviderFromPemFile(privateKeyPath);

               
                List<SmsOut> smsOuts = new List<SmsOut>();
                var destAddr = Util.convertPhone84(phone);
                var message = string.Format(SystemParam.SMS_MESSAGE, otp);
                SmsOut smsBrandname = new SmsOut
                {
                    CooperateMsgId = Guid.NewGuid().ToString(),
                    DestAddr = destAddr,
                    Message = message,
                    ShortCode = "Savico Mall",
                    CdrIndicator = "FREE",
                    MtType = "AN"
                };
                smsOuts.Add(smsBrandname);

                // Tạo transaction
                SmsTransaction smsTransaction = new SmsTransaction
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    CoopereateId = cooperateId,
                    CreateTime = DateTime.Now,
                    // Danh sách Sms cần gửi
                    SmsOuts = smsOuts
                };

                // Chuyển transaction sang chuỗi Json
                string jsonTran = JsonConvert.SerializeObject(smsTransaction);

                var byteData = Encoding.UTF8.GetBytes(jsonTran);

                var payload = Convert.ToBase64String(byteData);

                Crypto.IRsaCrypto rsaCrypto = new Crypto.RsaCrypto();

                var signature = Convert.ToBase64String(rsaCrypto.SignHash(byteData, provider));

                var verify = rsaCrypto.VerifyHashData(rsaCrypto.SignHash(byteData, provider), byteData,provider);
                SmsTransactionRequest smsTransactionRequest = new SmsTransactionRequest
                {
                    Payload = payload,
                    Signature = signature
                };
                string jsonTransactionRequest = JsonConvert.SerializeObject(smsTransactionRequest);

                byte[] bodyRawBytes = Encoding.UTF8.GetBytes(jsonTransactionRequest);

                // Kiểm tra token còn hạn không, nếu không còn hạn sử dụng thì lấy token mới
                // Hiện chúng tôi đang để token sẽ hết hạn trong vòng 5 phut
                var token = await clientToken.GetTokenAsync();

                if (token != null)
                {
                    var response = await smsClient.SendAsync(bodyRawBytes, token);

                    var resBytes = await response.Content.ReadAsByteArrayAsync();

                    var resString = Encoding.UTF8.GetString(resBytes);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var transResponse = JsonConvert.DeserializeObject<SmsTransactionResponse>(resString);
                    }
                    else
                    {

                    }
                    return jsonTransactionRequest;
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
