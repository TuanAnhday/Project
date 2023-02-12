using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Data;

public static class ProjectDbContextModelCreatingExtensions
{
    public static void ConfigureProject(this ModelBuilder builder)
    {
        var valueComparer = new ValueComparer<List<Guid>>(
            (c1,c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0,(a,v) => HashCode.Combine(a,v.GetHashCode())),
            c => c.ToList());
        var valueComparerString = new ValueComparer<List<string>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());
    }
}
