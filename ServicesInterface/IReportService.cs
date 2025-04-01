using Domain;
using Dto;
using Dto.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
    public interface IReportService
    {
        List<ReservaPorPlanDTO> GetReservasPorPLan(int planId, DateTime desde, DateTime hasta);
    }
}
