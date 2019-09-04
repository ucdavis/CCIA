using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class Standards
    {
       public int Id { get; set; }

       public string Name { get; set; } 
       public string Category { get; set; }
       public string Description { get; set; }
       public decimal Value { get; set; }
       public decimal MinValue { get; set; }
       public decimal MaxValue { get; set; }
       public string ValueType { get; set; }
       public string TextValue { get; set; }
       public string NegativeMessage { get; set; }
       public string PositiveMessage { get; set; }
       public string Program { get; set; }
    }
}