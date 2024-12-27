using System.Collections.Generic;
using CEM.Models;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class DataService
{
    private readonly QlbContext _context;

    public DataService(QlbContext context)
    {
        _context = context;
    }

    public async Task<List<BaiGiang>> GetBaiGiangsAsync()
    {
        return await _context.BaiGiangs.ToListAsync();
    }
}
