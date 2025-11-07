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
        List<ReservaPorPlanDTO> GetReservasPorTipoPLan(int tipo, DateTime desde, DateTime hasta);
    }
}
