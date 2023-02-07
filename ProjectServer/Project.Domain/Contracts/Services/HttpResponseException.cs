using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Contracts.Services;

public class HttpResponseException : Exception
{
    public HttpResponseException(int statusCode, object? value = null) => (StatusCode, Value) = (statusCode, value);
    public int StatusCode { get; set; }
    public object? Value { get; set; }
}
