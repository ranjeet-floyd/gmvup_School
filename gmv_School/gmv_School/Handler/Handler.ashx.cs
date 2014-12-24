using System;
using System.Web;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace gmv_School.Handler
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string MethodName = string.Empty; string q = string.Empty;
            try
            {
                var content = context.Request.Form;
                context.Response.ContentType = "application/json";
                if (context.Request.QueryString["MethodName"] != null)
                    MethodName = context.Request.QueryString["MethodName"].ToString();
                else
                {
                    if (content["methodName"] != null)
                        MethodName = content["methodName"].ToString();
                }

                if (context.Request.QueryString["q"] != null)
                    q = context.Request.QueryString["q"].ToString();

                //string bodys = context.Request.Files[0];

                switch (MethodName)
                {
                    case "ContactUs":
                        context.Response.Write(this.ContactUs(q));
                        break;

                    default:
                        context.Response.Write("Wrong Method");
                        break;
                }
            }
            catch (Exception ex) { context.Response.Write("Exception" + ex.Message); }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        //Send email
        public string ContactUs(string q)
        {
            string[] qArr = q.Split('~');
            if (qArr.Length > 3)
            {
                GmailHelper helper = new GmailHelper();
                return helper.SendEmail(qArr[0], qArr[1], qArr[2], qArr[3]);
            }
            return "false";

        }

    }

    //Helper | for sending email
    public class GmailHelper
    {
        private string EmailTo { get; set; }
        private string Password { get; set; }


        public GmailHelper()
        {
            NameValueCollection appValues = ConfigurationManager.AppSettings;
            this.EmailTo = appValues["EMAIL_TO"];
            this.Password = appValues["EMAIL_FROM_PASSWORD"];
        }

        public string SendEmail(string name, string email, string subject, string message)
        {
            string returnVal = string.Empty;
            try
            {

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(email, name);
                mailMessage.To.Add(this.EmailTo);
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = false;

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.Credentials = new NetworkCredential(this.EmailTo, this.Password);
                client.EnableSsl = true;
                Task t = new Task(() => client.Send(mailMessage));
                t.Start();
            }
            catch (AggregateException ae)
            {
                // Trace.Write("Send Email Exception : " + ae.Message);
                return ae.InnerException.Message;

            }
            catch (Exception ex)
            {
                returnVal = ex.Message;
                //Trace.Write("Send Email Exception : " + ex.Message);
                return returnVal;
            }
            return returnVal;

        }
    }
}
