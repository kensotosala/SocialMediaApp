using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Dominio.Entidades
{
    public class ApiErrorResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Details { get; set; }
    }
}
