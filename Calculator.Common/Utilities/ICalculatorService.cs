using Calculator.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Common.Utilities
{
    public interface ICalculatorService
    {
        IEnumerable<EncodedData> GetAllEncodedData();
        void PostEncodedData(EncodedData encodedData);        
    }
}
