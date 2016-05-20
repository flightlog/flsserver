using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Server.Interfaces.RulesEngine
{
    public interface ICondition
    {
        bool IsSatisfied();
    }
}
