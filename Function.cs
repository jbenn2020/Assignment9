using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Serialization;
using Amazon.Lambda.APIGatewayEvents;


using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Assignment9
{
    public class Function
    {
        public static readonly HttpClient client = new HttpClient();
        public async Task<ExpandoObject> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            
            dynamic item = new ExpandoObject();

            HttpResponseMessage response = await client.GetAsync("https://api.nytimes.com/svc/books/v3/lists/current/" + input.QueryStringParameters["list"] + ".json?api-key=BNwDeRtxd1vhWR9QguBPFAbmH5aDMMuF");

            //Only use this for debugging. input must be a string instead of an APIGatewayProxyRequest for this to work
            //HttpResponseMessage response = await client.GetAsync("https://api.nytimes.com/svc/books/v3/lists/current/hardcover-fiction.json?api-key=BNwDeRtxd1vhWR9QguBPFAbmH5aDMMuF");

            response.EnsureSuccessStatusCode();
            string result = response.Content.ReadAsStringAsync().Result;

            //Convert the expandoobject
            var converter = new Newtonsoft.Json.Converters.ExpandoObjectConverter();
            item = JsonConvert.DeserializeObject<ExpandoObject>(result, converter);
            return item;
        }
    }
}


//Hardcover book API for testing: https://api.nytimes.com/svc/books/v3/lists/current/hardcover-fiction.json?api-key=BNwDeRtxd1vhWR9QguBPFAbmH5aDMMuF