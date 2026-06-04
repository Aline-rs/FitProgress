using System;
using System.Collections.Generic;
using System.Text;

namespace FitProgress.Application.DTOs.PhysicalRecords
{
    public class UpdatePhysicalRecordRequestDTO
    {
        public DateOnly RecordDate { get; set; }
        public decimal Weight { get; set; }
        public string? Notes { get; set; }
    }
}
