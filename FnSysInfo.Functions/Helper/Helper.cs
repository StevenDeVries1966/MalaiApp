using AutoMapper;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System.Net;

namespace SysInfo.Functions.Helper
{
    public class Helper
    {
        public static HttpResponseData HttpResponseDataHelperList<T>(HttpRequestData req, IMapper _mapper, List<T> items)
        {
            var list = _mapper.Map<List<T>>(items);
            var outputStr = JsonConvert.SerializeObject(list);
            var responseOut = req.CreateResponse(HttpStatusCode.OK);
            responseOut.Headers.Add("Content-Type", "application/json");
            responseOut.WriteString(outputStr);
            return responseOut;
        }
        public static HttpResponseData HttpResponseDataHelperSingle<T>(HttpRequestData req, IMapper _mapper, T item)
        {
            var list = _mapper.Map<T>(item);
            var outputStr = JsonConvert.SerializeObject(list);
            var responseOut = req.CreateResponse(HttpStatusCode.OK);
            responseOut.Headers.Add("Content-Type", "application/json");
            responseOut.WriteString(outputStr);
            return responseOut;
        }
    }
}
