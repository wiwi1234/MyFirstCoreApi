using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstCoreApi.Models
{
    public class OpenInfo
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        OpenApiContact Contact { get; set; }
    }
    public class OpenApiContact
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
