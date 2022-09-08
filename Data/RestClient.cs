using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace WebBlogApi.Data
{
    public enum HttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public class RestHeaders
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class RestClient
    {

        public string postJSON { get; set; }
        public string endPoint { get; set; }
        public HttpVerb httpMethod { get; set; }
        public List<RestHeaders> RequestHeaders { get; set; }

        public RestClient()
        {
            endPoint = "";
            httpMethod = HttpVerb.POST;
        }
        public string MakeRequest()
        {
            string strResponseValue = string.Empty;

            var request = (HttpWebRequest)WebRequest.Create(endPoint);
            request.ContentType = "application/json";
            if (RequestHeaders != null)
            {
                for (int i = 0; i < RequestHeaders.Count; i++)
                {
                    request.Headers.Add(RequestHeaders[i].Name, RequestHeaders[i].Value);
                }
            }
            request.Method = httpMethod.ToString();

            using (StreamWriter JSONPost = new StreamWriter(request.GetRequestStream()))
            {
                JSONPost.Write(postJSON);
                JSONPost.Close();
            }

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //We catch non Http 200 responses here.
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }

            return strResponseValue;

        }


    }
}

