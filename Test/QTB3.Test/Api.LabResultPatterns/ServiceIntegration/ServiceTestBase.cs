using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using QTB3.Api.LabResultPatterns.Utilities;
using QTB3.Test.LabResultPatterns.Support.Db;
using QTB3.Test.LabResultPatterns.Support.TestStartups;
using QTB3.Test.Support.Jwt;
using QTB3.Test.Support.TestServices;

namespace QTB3.Test.Api.LabResultPatterns.ServiceIntegration
{
    public class ServiceTestBase: SqlitePropertyContextTestBase
    {
        public const string EndpointBase = "http://localhost";

        public TestTokenGenerator TokenGenerator = new TestTokenGenerator();

        public List<TestClaim> GoodClaimsList = new List<TestClaim>
        {
            new TestClaim {Name= "http://schemas.microsoft.com/identity/claims/scope", Value="labresultpatterns" },
            new TestClaim {Name= "jobTitle", Value="Advanced" }
        };

        public List<TestClaim> GoodScopeBadUserClaimsList = new List<TestClaim>
        {
            new TestClaim {Name= "http://schemas.microsoft.com/identity/claims/scope", Value="labresultpatterns" },
            new TestClaim {Name= "jobTitle", Value="Subpar" }
        };

        public List<TestClaim> BadScopeGoodUserClaimsList = new List<TestClaim>
        {
            new TestClaim {Name= "http://schemas.microsoft.com/identity/claims/scope", Value="someotherapp" },
            new TestClaim {Name= "jobTitle", Value="Advanced" }
        };

        public List<TestClaim> BadScopeBadUserClaimsList = new List<TestClaim>
        {
            new TestClaim {Name= "http://schemas.microsoft.com/identity/claims/scope", Value="someotherapp" },
            new TestClaim {Name= "jobTitle", Value="Subpar" }
        };

        public HttpClient GetTestReadClient()
        {
            var serverBuilder = new IntegrationTestServerBuilder();
            var testReadServer = serverBuilder.CreateServer<TestReadServiceStartup>();
            return testReadServer.CreateClient();
        }

        public HttpClient GetTestWriteClient()
        {
            var serverBuilder = new IntegrationTestServerBuilder();
            var testReadServer = serverBuilder.CreateServer<TestWriteServiceStartup>();
            return testReadServer.CreateClient();
        }

        public StringContent GetWriteContent(object item)
        {
            var jsonString = JsonConvert.SerializeObject(item);
            return new StringContent(jsonString, Encoding.UTF8, "application/json");
        }

        public HttpRequestMessage GetRequestWithToken
        (
            HttpMethod method, 
            string endpoint, 
            List<TestClaim> claims, 
            string mediaType = LrpSupportedMedia.LrpMediaTypeV1
        )
        {
            var token = TokenGenerator.CreateTestToken(claims);
            var httpMessage = new HttpRequestMessage(method, endpoint);
            httpMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            return httpMessage;
        }
    }
}
