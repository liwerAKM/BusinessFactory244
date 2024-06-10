using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBusHos36_GJYB.BUS
{
    public interface IBusiness
    {
        DataReturn PSNQUERY(string injson);
        DataReturn REGTRY(string injson);
        DataReturn OUTPTRY(string injson);
        DataReturn SETTLE(string injson);
        DataReturn REFUND(string injson);
        DataReturn COMMON(string injson);


        DataReturn PSNINFOQUERY(string injson);

    }
}
