using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTest.Domain.Dto
{
    public class ClientConnectionDto
    {
        public string connectionId { get; set; }
        [ForeignKey("User")]
        public int userId { get; set; }
    }
}
