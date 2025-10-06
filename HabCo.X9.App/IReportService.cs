using HabCo.X9.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HabCo.X9.App;

public interface IReportService
{
    byte[] CreateSalesPdfReport(List<Booking> bookings, DateTime startDate, DateTime endDate);
}