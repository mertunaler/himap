using System;

namespace IMAP.API.Models.Responses;

public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
}
