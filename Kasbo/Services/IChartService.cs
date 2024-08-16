using Kasbo.Models;

namespace Kasbo.Services;

public interface IChartService
{
    public Task Add(AddChartDTO addChartDTO);

    public Task CalculateAvarage();
}