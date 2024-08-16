using System;

namespace IMAP.API.Models.Requests;

public class LoginRequest
{
    public string UserName { get; set; }
    public string PassWord { get; set; }
}
