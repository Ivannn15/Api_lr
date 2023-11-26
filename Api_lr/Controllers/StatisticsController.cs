using inmemory.models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/sales/statistics")]
    public class SalesStatisticsController : ControllerBase
    {
        private List<sale> sales = new List<sale>
        {
            new sale { id = 1, price = 10000, time_start_order = new DateTime(2023, 12, 11), user = new user { Id = 1, Name = "User 1" }, preorders = new List<car>() },
            new sale { id = 2, price = 20000, time_start_order = new DateTime(2023, 12, 15), user = new user { Id = 2, Name = "User 2" }, preorders = new List<car>() },
            new sale { id = 2, price = 20000, time_start_order = new DateTime(2023, 12, 15), user = new user { Id = 2, Name = "User 2" }, preorders = new List<car>() },
            new sale { id = 3, price = 20000, time_start_order = new DateTime(2024, 1, 1), user = new user { Id = 1, Name = "User 1" }, preorders = new List<car>() },
            new sale { id = 3, price = 20000, time_start_order = new DateTime(2024, 1, 1), user = new user { Id = 1, Name = "User 1" }, preorders = new List<car>() },
            new sale { id = 4, price = 30000, time_start_order = new DateTime(2024, 1, 20), user = new user { Id = 3, Name = "User 3" }, preorders = new List<car>() },
            new sale { id = 5, price = 40000, time_start_order = new DateTime(2024, 1, 25), user = new user { Id = 2, Name = "User 2" }, preorders = new List<car>() },
        };

        [HttpGet("populary/weak/{date1}/{date2}")]
        public IActionResult GetPopularWeak(string date1, string date2)
        {
            DateTime startDate;
            DateTime endDate;

            if (!DateTime.TryParseExact(date1, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out startDate)
                || !DateTime.TryParseExact(date2, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out endDate))
            {
                return BadRequest("Invalid date format. Ввидите в таком формате 'dd.MM.yyyy' format.");
            }

            var weakSales = sales.Where(s => s.time_start_order >= startDate && s.time_start_order <= endDate)
                                .GroupBy(s => GetWeakDates(s.time_start_order))
                                .Select(g => new
                                {
                                    weak = g.Key,
                                    sale_count = g.Count(),
                                    value = g.Sum(s => s.price)
                                })
                                .OrderByDescending(g => g.sale_count)
                                .ToList();

            return Ok(new { weak = weakSales });
        }

        private string GetWeakDates(DateTime date)
        {
            DateTime startDate = GetStartOfWeek(date);
            DateTime endDate = GetEndOfWeek(date);

            return $"{startDate.ToString("dd.MM.yyyy")} - {endDate.ToString("dd.MM.yyyy")}";
        }

        private DateTime GetStartOfWeek(DateTime date)
        {
            int diff = date.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0)
            {
                diff += 7;
            }
            return date.AddDays(-1 * diff).Date;
        }

        private DateTime GetEndOfWeek(DateTime date)
        {
            return GetStartOfWeek(date).AddDays(6);
        }
    }
}
