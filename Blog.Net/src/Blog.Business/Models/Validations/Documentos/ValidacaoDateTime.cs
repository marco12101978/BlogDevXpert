using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Business.Models.Validations.Documentos
{
    public static class ValidacaoDatetime
    {
        public static bool EhUmaDataValida(DateTime? data)
        {
            return data.HasValue && data.Value != DateTime.MinValue;
        }
    }
}
