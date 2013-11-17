using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Halo.Utilities
{
    public class EmailManager
    {
        public enum ErrorSourceType
        {
            Application = 1,
            WebService = 2,
            WindowsService = 3
        };

        /// <summary>
        /// The mail message
        /// </summary>
        private MailMessage mailMessage = new MailMessage();

        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the credential login.
        /// </summary>
        /// <value>
        /// The credential login.
        /// </value>
        public string CredentialLogin
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the credential password.
        /// </summary>
        /// <value>
        /// The credential password.
        /// </value>
        public string CredentialPassword
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailManager"/> class.
        /// </summary>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public EmailManager()
        {
            // Uses Settings in config.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailManager"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="credentialLogin">The credential login.</param>
        /// <param name="credentialPassword">The credential password.</param>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public EmailManager(string host, string credentialLogin, string credentialPassword)
        {
            Host = host;
            CredentialLogin = credentialLogin;
            CredentialPassword = credentialPassword;
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="to">To Email Address.</param>
        /// <param name="from">From Email Address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <param name="html">if set to <c>true</c> [HTML].</param>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public void SendEmail(string to, string from, string subject, string message, bool html,
            IEnumerable<Attachment> attachments)
        {
            mailMessage.IsBodyHtml = html;
            mailMessage.From = new MailAddress(from);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.To.Clear();
            mailMessage.To.Add(new MailAddress(to));

            if (attachments != null && attachments.Count() > 0)
            {
                foreach (var attachment in attachments)
                {
                    mailMessage.Attachments.Add(attachment);
                }
            }

            SmtpClient smtp = new SmtpClient();

            if (!string.IsNullOrEmpty(Host))
            {
                smtp.Host = Host;
            }

            if (!string.IsNullOrEmpty(CredentialLogin) && !string.IsNullOrEmpty(CredentialPassword))
            {
                smtp.Credentials = new NetworkCredential(CredentialLogin, CredentialPassword);
            }

            smtp.Send(mailMessage);
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="to">String list of To recipents</param>
        /// <param name="from">From email address</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <param name="html">if set to <c>true</c> [HTML].</param>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public void SendEmail(List<string> to, string from, string subject, string message, bool html)
        {
            foreach (string toEmail in to)
            {
                SendEmail(toEmail, from, subject, message, html, null);
            }
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="additionalMessage">The additional message.</param>
        /// <param name="exceptionObject">The exception object.</param>
        /// <param name="includeHTTPContext">if set to <c>true</c> [include HTTP context].</param>
        /// <param name="html">if set to <c>true</c> [HTML].</param>
        /// <createdate>7-30-2013</createdate>
        /// <author>
        /// James Gates Richardson
        /// </author>
        public void SendEmail(string to, string from, string subject, Exception exceptionObject,
            string additionalMessage, bool includeHTTPContext, bool html)
        {

            string errMessage = string.Empty;

            if (includeHTTPContext)
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string referer = String.Empty;

                if (context.Request.ServerVariables["HTTP_REFERER"] != null)
                {
                    referer = context.Request.ServerVariables["HTTP_REFERER"].ToString();
                }

                string formvars = (context.Request.Form != null) ? context.Request.Form.ToString() : String.Empty;
                string query = (context.Request.QueryString != null) ? context.Request.QueryString.ToString() : String.Empty;

                errMessage =
                   "\n\nSOURCE: " + exceptionObject.Source +
                   "\nLogDateTime: " + DateTime.Now +
                   "\nMESSAGE: " + exceptionObject.Message +
                   "\nQUERYSTRING: " + query +
                   "\nTARGETSITE: " + exceptionObject.TargetSite +
                   "\nREFERER: " + referer +
                   "\n\n\nSTACKTRACE: " + exceptionObject.StackTrace +
                   "\n\n\nINNER MESSAGE: " + (exceptionObject.InnerException == null ? string.Empty : exceptionObject.InnerException.ToString()) +
                   "\n\n\nFORM: " + formvars;
            }
            else
            {
                errMessage =
                   "\n\nSOURCE: " + exceptionObject.Source +
                   "\nLogDateTime: " + DateTime.Now +
                   "\nMESSAGE: " + exceptionObject.Message +
                   "\nTARGETSITE: " + exceptionObject.TargetSite +
                   "\n\n\nSTACKTRACE: " + exceptionObject.StackTrace +
                   "\n\n\nINNER MESSAGE: " + (exceptionObject.InnerException == null ? string.Empty : exceptionObject.InnerException.ToString());
            }

            SendEmail(to, from, subject, additionalMessage + errMessage, html, null);
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="additionalMessage">The additional message.</param>
        /// <param name="exceptionObject">The exception object.</param>
        /// <param name="includeHTTPContext">if set to <c>true</c> [include HTTP context].</param>
        /// <param name="html">if set to <c>true</c> [HTML].</param>
        /// <createdate>7-30-2013</createdate>
        /// <author>
        /// James Gates Richardson
        /// </author>
        public void SendEmail(List<string> to, string from, string subject, Exception exceptionObject,
            string additionalMessage, bool includeHTTPContext, bool html)
        {
            foreach (string toEmail in to)
            {
                SendEmail(toEmail, from, subject, exceptionObject, additionalMessage, includeHTTPContext, html);
            }
        }

        /// <summary>
        /// Errors the email.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        /// <param name="source">The source.</param>
        /// <param name="exceptionObject">The exception object.</param>
        /// <param name="includeHTTPContext">if set to <c>true</c> [include HTTP context].</param>
        /// <param name="html">if set to <c>true</c> [HTML].</param>
        /// <createdate>11-??-2010</createdate>
        /// <author>
        /// Barry Fitzgerald
        /// </author>
        public void ErrorEmail(ErrorSourceType sourceType, string applicationName, string to, string from,
            string source, Exception exceptionObject, bool includeHTTPContext, bool html)
        {
            string Subject = applicationName + " " + sourceType.ToString() + " Error";
            string additionalMessage = "An error has occured in the " + sourceType.ToString() + ": " + applicationName + "\nSource: " + source + "\n\n---System Generated Email---Do Not Reply---";

            SendEmail(to, from, Subject, exceptionObject, additionalMessage, includeHTTPContext, html);//From, To, Subject, Body, true);
        }

        /// <summary>
        /// Errors the email.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        /// <param name="source">The source.</param>
        /// <param name="exceptionObject">The exception object.</param>
        /// <param name="includeHTTPContext">if set to <c>true</c> [include HTTP context].</param>
        /// <param name="html">if set to <c>true</c> [HTML].</param>
        /// <createdate>7-30-2013</createdate>
        /// <author>
        /// James Gates Richardson
        /// </author>
        public void ErrorEmail(ErrorSourceType sourceType, string applicationName, List<string> to, string from,
            string source, Exception exceptionObject, bool includeHTTPContext, bool html)
        {
            foreach (string toEmail in to)
            {
                ErrorEmail(sourceType, applicationName, toEmail, from, source, exceptionObject, includeHTTPContext, html);
            }
        }

        #region IDisposable Implementation

        /// <summary>
        /// The disposed
        /// </summary>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        protected bool disposed = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                // Do nothing if the object has already been disposed of.
                if (disposed)
                    return;

                if (disposing)
                {
                    // Release disposable objects used by this instance here.

                    if (mailMessage != null)
                        mailMessage.Dispose();
                }

                // Release unmanaged resources here. Don't access reference type fields.

                // Remember that the object has been disposed of.
                disposed = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public virtual void Dispose()
        {
            Dispose(true);
            // Unregister object for finalization.
            GC.SuppressFinalize(this);
        }

        #endregion
    }  ///End of class
}
