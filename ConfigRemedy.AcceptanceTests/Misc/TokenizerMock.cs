using ConfigRemedy.Security.Modules;
using ConfigRemedy.Security.Nancy;
using Nancy;
using Nancy.Authentication.Token;
using Nancy.Security;

namespace ConfigRemedy.AcceptanceTests.Misc
{
    public class TokenizerMock : ITokenizer
    {
        public const string Token = "SmFtZXNCb25kDQphZG1pbnx1c2VyDQo2MzU0Mjc4MDgwODM5NzAyMDANCk1vemlsbGEvNS4wIChXaW5kb3dzIE5UIDYuMzsgV09XNjQpIEFwcGxlV2ViS2l0LzUzNy4zNiAoS0hUTUwsIGxpa2UgR2Vja28pIENocm9tZS8zNi4wLjE5ODUuMTI1IFNhZmFyaS81MzcuMzY=:8U8fr2LRfhiEaZ/4B1W3Gvj4D33jTbmslHRIESYj4+o=";
        public string Tokenize(IUserIdentity userIdentity, NancyContext context)
        {
            return Token;
        }

        public IUserIdentity Detokenize(string token, NancyContext context)
        {
            return new ConfiguratronUserIdentity
            {
                UserName = "JamesBond",
                DisplayName = "James Bond",
                UserId = "user/1",
                Role = "admin",
                Claims = new[] { "admin", "user"},
            };
        }
    }
}