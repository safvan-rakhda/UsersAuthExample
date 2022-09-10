using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace UsersAuthExample.Response
{
    public class ResponseBase
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public bool IsValid => HttpStatusCode == HttpStatusCode.OK && (Errors == null || !Errors.Any());

        public IDictionary<string, ICollection<string>> Errors { get; set; }
    }
}
