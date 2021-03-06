﻿using AuditLogApp.Membership;
using AuditLogApp.Models.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuditLogApp.ErrorNotification
{
    public class EmailErrorNotifier : IErrorNotifier
    {
        private SmtpClient _mailClient;
        private IUserMembership _membership;
        private EmailErrorNotifierOptions _options;

        public EmailErrorNotifier(SmtpClient mailClient, IUserMembership membership, EmailErrorNotifierOptions options)
        {
            _mailClient = mailClient;
            _membership = membership;
            _options = options;
        }

        public async Task NotifyAsync(DescriptiveError descriptiveError, Exception unhandledException, string path, ClaimsPrincipal user = null)
        {
            var details = new Dictionary<string, string>() {
                { "IsClientSide", "false" },
                { "Path", path },
                { "Descriptive.Message", descriptiveError.Message },
                { "Time", DateTime.UtcNow.ToString("g") }
            };

            await AppendUserAsync(user, details);
            AppendAdditionalDetails(descriptiveError, details);

            await ReportAsync(details, ExceptionToHandle.FromException(unhandledException));
        }


        public async Task NotifyAsync(DescriptiveError descriptiveError, bool isClientSide, ClaimsPrincipal user = null)
        {
            var details = new Dictionary<string, string>() {
                { "IsClientSide", isClientSide.ToString() },
                { "Descriptive.Message", descriptiveError.Message },
                { "Time", DateTime.UtcNow.ToString("g") }
            };

            await AppendUserAsync(user, details);
            AppendAdditionalDetails(descriptiveError, details);


            await ReportAsync(details);
        }

        private async Task AppendUserAsync(ClaimsPrincipal user, Dictionary<string, string> details)
        {
            if (user != null)
            {
                try
                {
                    var userDetails = await _membership.DescribeUserForErrorAsync(user);
                    foreach (var k in userDetails.Keys)
                    {
                        details[k] = userDetails[k].ToString();
                    }
                }
                catch (Exception exc)
                {
                    details.Add("Membership Unavailable", "Membership details are unavailable, received an Exception: " + exc.Message);

                    // fallback to just dumping raw claims
                    foreach (var claim in user.Claims)
                    {
                        details.Add("User.Claims." + claim.Type, claim.Value);
                    }
                }
            }
        }


        private void AppendAdditionalDetails(DescriptiveError descriptiveError, Dictionary<string, string> details)
        {
            if (descriptiveError.AdditionalDetails != null)
            {
                foreach (var kvp in descriptiveError.AdditionalDetails)
                {
                    details.Add(kvp.Key, kvp.Value);
                }
            }
        }

        private async Task ReportAsync(Dictionary<string, string> details, ExceptionToHandle exc = null)
        {
            string subject = $"[{_options.EnvironmentName}][Error] ";
            if (exc != null)
                subject += exc.ExceptionType;
            else if (details.ContainsKey("ErrorType"))
                subject += details["ErrorType"];

            string body = "<table>\n";
            body += String.Join("\n", details.Select(kvp => $"<tr><td>{kvp.Key}</td><td>{kvp.Value}</td></tr>"));
            body += "</table>\n\n";

            if (exc != null)
            {
                body += "<h1>Exception</h1>\n";
                body += ExceptionToHtml(exc);
            }

            var message = new MailMessage(_options.FromAddress, _options.ToAddress, subject, body);
            message.IsBodyHtml = true;
            //TODO what to do with exceptions here?
            await _mailClient.SendMailAsync(message);
        }

        private string ExceptionToHtml(ExceptionToHandle exc)
        {
            var html = "<table>\n";
            html += $"<tr><td>Type</td><td>{exc.ExceptionType}</td></tr>\n";
            html += $"<tr><td>Message</td><td>{exc.Message}</td></tr>\n";
            html += $"<tr><td>Source</td><td>{exc.Source}</td></tr>\n";
            html += $"<tr><td>Stack</td><td><pre>{exc.StackTrace}</pre></td></tr>\n";

            if (exc.InnerException != null)
            {
                html += $"<tr><td>Inner Exception</td><td>\n{ExceptionToHtml(exc.InnerException)}\n</td></tr>";
            }

            html += "</table>\n";
            return html;
        }

    }
}
