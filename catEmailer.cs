using System.Net;
using System.Net.Http;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.Net.Mime;
void Main()
{
	string[] creds = new string[] {user, password};
	string[] emailOpts = new string[] {from, to, subject, attachmentPath};
    var Caller = new Caller();
    Caller.Get(creds, emailOpts);
}
public class Caller
{
	private readonly HttpClient _caller;
	public Caller()
	{

		_caller = new HttpClient(); 
	}
	public async void Get(string[] creds, string[] emailOpts)
	{
		var response = await _caller.GetByteArrayAsync ( "https://cataas.com/cat" );
		using(Image image = Image.FromStream(new MemoryStream(response)))
		{
		    image.Save(@"‪output.jpg", ImageFormat.Jpeg ); 
			SmtpClient SmtpServer = new SmtpClient();
			SmtpServer.Port = 587;
			SmtpServer.UseDefaultCredentials = false;
			SmtpServer.Host = "smtp.office365.com";
			SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
			SmtpServer.Credentials = new System.Net.NetworkCredential(creds[0], creds[1]);
			SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
			SmtpServer.EnableSsl = true;
			
			MailMessage mail = new MailMessage();
	        mail.From = new MailAddress(emailOpts[0]);
	        mail.To.Add(emailOpts[1]);
	        mail.Subject = emailOpts[2];
			mail.Body = "<img src=" + emailOpts[3] + "></img>";
			mail.IsBodyHtml = true;
			mail.Attachments.Add(new Attachment(emailOpts[3]));
			SmtpServer.Send(mail);
		}
	}
}

public static class ResponseHelper
{
	public static bool HandleResponse(HttpStatusCode statusCode)
	{
		bool success = false;
		switch (statusCode)
		{
			case HttpStatusCode.OK:
				success = true;
				//log OK or do nothing
				break;
			case HttpStatusCode.Forbidden:
				throw new HttpRequestException( "Request is Forbidden" );
			default:
				throw new ArgumentException( string.Format( "The status code: {0} is not being handled" ), statusCode.ToString() );
		}
		return success;
	}
}

private AlternateView CreateHtmlMessage(string message, string meowPath)
{
    var inline = new LinkedResource(meowPath);
    inline.ContentId = "meow";
    var alternateView = AlternateView.CreateAlternateViewFromString(
                            message, 
                            Encoding.UTF8, 
                            MediaTypeNames.Text.Html);
    alternateView.LinkedResources.Add(inline);

    return alternateView;
}
