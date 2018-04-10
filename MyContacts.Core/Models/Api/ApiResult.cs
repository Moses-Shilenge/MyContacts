using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyContacts.API.Models
{
    public class ApiResult<T>
    {
        public ApiResult() { }
        public ApiResult(T result)
        {
            Result = result;
        }
        public string Error { get; set; }
        public T Result { get; set; }
    }
}
