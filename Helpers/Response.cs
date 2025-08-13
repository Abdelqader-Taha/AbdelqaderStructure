using Microsoft.AspNetCore.Mvc;

namespace AbdelqaderStructure.Helpers;

public class Response<T>
{
    public T Data { get; set; }
    public string Msg { get; set; }
    public int StatusCode { get; set; }

    public Response(T data, string msg, int statusCode)
    {
        Data = data;
        Msg = msg;
        StatusCode = statusCode;
    }
}