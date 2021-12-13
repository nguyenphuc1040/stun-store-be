using System;
using System.Net;
using System.Net.Mail;

namespace SmtpManager
{
    [Serializable]
    public class SmtpController
    {
        public static bool CreateEmailVerify(string toAdress, string code, string idUser)
        {
            string url = ProcessLinkCode(code,idUser);

            MailMessage message = new MailMessage(GetUserName(), toAdress);
            message.Subject = "Stun Store - Confirm your Account";
            message.Body = GetConfirmAccountMailBody(code,url);
            message.IsBodyHtml = true;
            using SmtpClient client = new SmtpClient{
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential(GetUserName(),GetPassword())
            };

            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex.ToString());
                return false;
            }
        }        
        public static bool CreateResetPasswordVerify(string toAdress, string code, string idUser)
        {
            string url = ProcessLinkCode(code,idUser);

            MailMessage message = new MailMessage(GetUserName(), toAdress);
            message.Subject = "Stun Store - Reset password";
            message.Body = GetResetPasswordMailBody(code,url);
            message.IsBodyHtml = true;
            using SmtpClient client = new SmtpClient{
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential(GetUserName(),GetPassword())
            };

            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex.ToString());
                return false;
            }
        }        
        private static string ProcessLinkCode(string code, string idUser){
            int i = 1;
            string url = idUser;
            foreach(char item in code){
                url = url.Insert(i,((char)(item+49)).ToString());
                i+= 3;
            }
            return url;
        }
        public static string GetUserName(){
            return "stun.services@gmail.com";
        }
        public static string GetPassword(){
            return "71311147601";
        }
        public static string GetConfirmAccountMailBody(string code, string url){
            return @"<html>
                                <head>
                                    <style type='text/css'>
                                        h1 {
                                            color:rgb(51, 51, 51);
                                            font-weight: 600;
                                            font-size: 24px;
                                        }
                                        h4{
                                            color:rgb(63, 63, 63);
                                            font-weight: 500;
                                            font-size: 14px;
                                        }
                                        .h2-c{
                                            color:rgb(63, 63, 63);
                                            font-weight: 600;
                                            font-size: 37px;
                                        }

                                    </style>
                                </head>
                                <body>
                                    <div>
                                        <div style='  padding: 20px 50px;
                                        height: fit-content;
                                        width: 400px;
                                        background-color: rgb(255, 255, 255);
                                        border-radius: 5px;'>
                                            <h1>
                                                Welcome!
                                            </h1>
                                            <h4>
                                                We're excited to have you get started. First, you need to confirm your account. Just press the button below.
                                            </h4>" + @$"
                                            <div style='width: 100%; display: flex; justify-content: center;padding:10px 0'>
                                                <a style='height: 33px;
                                                width: 190px;
                                                background-color: brown;
                                                color: white;
                                                font-weight: 600;
                                                border-radius: 5px;
                                                transition: .3s;
                                                text-align: center;
                                                padding-top: 13px;
                                                text-decoration: none;' href='https://stun-store.vercel.app/email-verify/{url}'>
                                                    Confirm Account
                                                </a>
                                            </div>
                                            <h4>
                                                If that doesn't work, copy and paste the code in your browser
                                            </h4>
                                            <div class='h2-c' style='width: 100%;padding:10px'>
                                                   {code}
                                            </div>  
                                            <h4>
                                                If you have any questions, just reply to this email-we're always happy to help out <br/><br/>
                                                Cheers, <br/>
                                                The Stun Team
                                            </h4>
                                        </div>
                                    </div>
                                </body>
                                </html>";
        }
        public static string GetResetPasswordMailBody(string code, string url){
            return @"<html>
                            <head>
                                <style type='text/css'>
                                    h1 {
                                        color:rgb(51, 51, 51);
                                        font-weight: 600;
                                        font-size: 24px;
                                    }
                                    h4{
                                        color:rgb(63, 63, 63);
                                        font-weight: 500;
                                        font-size: 14px;
                                    }
                                    .h2-c{
                                        color:rgb(63, 63, 63);
                                        font-weight: 600;
                                        font-size: 37px;
                                    }

                                </style>
                            </head>
                            <body>
                                <div>
                                    <div style='  padding: 20px 50px;
                                    height: fit-content;
                                    width: 400px;
                                    background-color: rgb(255, 255, 255);
                                    border-radius: 5px;'>
                                        <h1>
                                            Hello!
                                        </h1>
                                        <h4>
                                            You are receiving this email because we received a password reset request for your account
                                        </h4>"+@$"
                                        <div style='width: 100%; display: flex; justify-content: center;padding:10px 0'>
                                            <a style='height: 33px;
                                            width: 190px;
                                            background-color: brown;
                                            color: white;
                                            font-weight: 600;
                                            border-radius: 5px;
                                            transition: .3s;
                                            text-align: center;
                                            padding-top: 13px;
                                            text-decoration: none;' href='https://stun-store.vercel.app/reset-pwd-verify/{url}'>
                                                Reset Password
                                            </a>
                                        </div>
                                        <h4>
                                            If that doesn't work, copy and paste the code in your browser
                                        </h4>
                                        <div class='h2-c' style='width: 100%;padding:10px'>
                                                {code}
                                        </div>  
                                        <h4>
                                            If you have any questions, just reply to this email-we're always happy to help out <br/><br/>
                                            Cheers, <br/>
                                            The Stun Team
                                        </h4>
                                    </div>
                                </div>
                            </body>
                            </html>";
        }
    }
}