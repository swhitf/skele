using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    public interface IPresenter
    {
        bool Confirm(string message);

        string Prompt(string message);

        T Prompt<T>(string message)
            where T : IConvertible;
    }
}
