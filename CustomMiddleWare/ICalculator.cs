using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomMiddleWare
{
    public interface ICalculator
    {
        /// <summary>
        /// Executes abstract mathimatical operation.
        /// </summary>
        /// <param name="operatorName">The name of the operator. E.g. add/</param>
        /// <param name="arguments">The arguments that should be used by the operator.</param>
        /// <returns>
        /// Null when the operation is not possible.
        /// The calculated result otherwise.
        /// </returns>
        double? ExecuteOperation(string operatorName, string[] arguments);
    }
}
