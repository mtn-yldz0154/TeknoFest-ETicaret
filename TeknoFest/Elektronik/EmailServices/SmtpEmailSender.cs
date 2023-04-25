using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ElektronikWebUI.EmailServices
{
	public class SmtpEmailSender : IEmailSender
	{
		private string _host;
		private string _username;
		private string _password;
		private int _port;
		private bool _enableSSL;
		public SmtpEmailSender(string host,int port,bool enableSSL,string username,string password)
		{
			this._enableSSL=enableSSL;
			this._host=host;
			this._port=port;
			this._username=username;
			this._password=password;
		}

		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var client=new SmtpClient(this._host, this._port)
			{
				Credentials=new NetworkCredential(_username, _password),
				EnableSsl= true,
			};

			return client.SendMailAsync(new MailMessage(_username, email, subject, htmlMessage)
			{
				IsBodyHtml=true
			});
		}
	}
}
