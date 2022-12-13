namespace AuthenticationProject.Services.Utils
{
    public static class ResponseError
    {
        public static readonly string PasswordsAreDifferent = "Auth/Passwords-Are-Diferent";
        public static readonly string IncorrectPassword = "Auth/Password-Is-Incorrect";
        public static readonly string UserNotFound = "Auth/User-Not-Found";
        public static readonly string EmailAlreadyUsed = "Auth/Email-Already-Used";
        public static readonly string InternalServerError = "Auth/Internal-Server-Error";
        public static readonly string BadRequest = "Auth/Incorrect-Request";

    }
}
