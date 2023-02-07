using Project.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Data.Models;

public abstract class DataModel
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }

    public abstract AggregateRoot ToEntity();

    public static DataModel FromEntity(AggregateRoot root)
    {
        var mapper = new Dictionary<Type, Func<AggregateRoot, DataModel>>();
        return mapper[root.GetType()](root);
    }
}
