using System;
using System.IO;
using System.Net.Mail;
using System.Xml;

namespace MIBREQUESTS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("SharpHOJobs MIB Request Enabled Check!");
            XmlDocument xDoc = new XmlDocument();
            try
            {
                if (File.Exists(@"\\ailap\pcapps\prod\sharphojobs\QuickRun.xml"))
                {                    
                    xDoc.Load(@"\\ailap\pcapps\prod\sharphojobs\QuickRun.xml");
                    if (xDoc != null && xDoc.SelectNodes("/Settings/Group[@ name='JOBS']") != null)
                    {
                        XmlNodeList? nodeList = xDoc.SelectNodes("/Settings/Group[@ name='JOBS']/Section");
                        if (nodeList != null)
                        {                            
                            var consoleKey = "";
                            foreach (XmlNode list in nodeList)
                            {
                                if (list.Attributes?.Count > 0 && list.Attributes["name"]?.Value == "MIB REQUESTS")
                                {                                   
                                    XmlNode? node = list.SelectSingleNode("Setting[@ name='ENABLED']");                                   
                                    if (node?.Attributes?.Count > 0 && node.Attributes["value"]?.Value == "False")
                                    {
                                        Console.WriteLine("Enabled flag for MIB REQUESTS is : " + node.Attributes["value"]?.Value);
                                        Console.WriteLine("Would you like change 'ENABLED' Flag to True ? Y/N?");
                                        consoleKey = Console.ReadLine();
                                        if (consoleKey.ToUpper() == "N")
                                            break;
                                        else
                                        {
                                            node.Attributes["value"].Value = "True";
                                            MailMessage message = new MailMessage();
                                            SmtpClient smtp = new SmtpClient();
                                            message.From = new MailAddress(Environment.UserName + "@globe.life");
                                            message.To.Add(new MailAddress("venrkalyanapu@globe.life"));
                                            //message.To.Add(new MailAddress("crmrunteam@globe.life"));
                                            message.To.Add(new MailAddress("vensnambiar@globe.life"));

                                            message.IsBodyHtml = true; //to make message body as html
                                            message.Subject = "Morning Health Check For sharphojobs";
                                            message.Body = "Good morning," + "<br /> <br />" + "As of " + DateTime.Now.ToString("h:mm tt") + " this morning   " +
                                                       "<br /> " +
                                                       "&nbsp;&nbsp;&nbsp;• Enabled flag for MIB REQUESTS is  'False'.";
                                            smtp.Port = 25;
                                            smtp.Host = "smtp.mail.globe.life";
                                            smtp.EnableSsl = true;
                                            smtp.UseDefaultCredentials = false;//DONOTReply@torchmarkcorp.com
                                                                               //smtp.Credentials = new NetworkCredential("venrkalyanapu@globe.life", "Shreyan@201513");
                                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                            smtp.Send(message);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Enabled flag for MIB REQUESTS is : " + node.Attributes["value"]?.Value);
                                        break;
                                    }
                                }

                            }
                            if (consoleKey.ToUpper() == "Y")
                            {
                                xDoc.Save(@"\\ailap\pcapps\prod\sharphojobs\QuickRun.xml");
                                Console.WriteLine("Enabled flag fo MIB REQUESTS is updated successfully.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
