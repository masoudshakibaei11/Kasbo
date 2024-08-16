using Kasbo.AppDbContext;
using Kasbo.Models;
using Microsoft.EntityFrameworkCore;

namespace Kasbo.Services;

public class ChartService : IChartService
{
    private readonly ApplicationDbContext _context;

    public ChartService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Add(AddChartDTO addChartDTO)
    {

        var chart = new Chart
        {
            Open = addChartDTO.Open,
            Close = addChartDTO.Close,
            High = addChartDTO.High,
            Low = addChartDTO.Low,
            StockId = 1,
        };
        await _context.Charts.AddAsync(chart);
        await _context.SaveChangesAsync();


    }

    public async Task CalculateAvarage()
    {


        var exist = await _context.Charts.Where(c => c.Avg == 0).AnyAsync();

        if (exist)
        {
            Chart chart = await _context.Charts
                       .Where(c => c.Avg == 0).FirstAsync();


            var avg = (chart.Open + chart.Close + chart.High + chart.Low) / 4;
            chart.Avg = avg;
            _context.Charts.Update(chart);

        }

        await _context.SaveChangesAsync();

    }

}