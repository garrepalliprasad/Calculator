using Calculator.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Calculator.Common.Utilities
{
    public class CalculatorService:ICalculatorService
    {
        private readonly AppDbContext _context;

        public CalculatorService(AppDbContext context)
        {
            _context = context;
        }
       
       
        public void PostEncodedData(EncodedData encodedData)
        {
            _context.Add(encodedData);
            _context.SaveChanges();
        }

        public IEnumerable<EncodedData> GetAllEncodedData()
        {
            return _context.EncodedData.ToList();
        }
        
    }
}
