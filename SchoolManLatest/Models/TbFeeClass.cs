using System;
using System.Collections.Generic;

namespace TrackTap.Models;

public partial class TbFeeClass
{
    public long FeeClassId { get; set; }

    public long FeeId { get; set; }

    public decimal Amount { get; set; }

    public long ClassId { get; set; }

    public Guid FeeClassGuid { get; set; }

    public DateTime TimeStamp { get; set; }

    public bool PublishStatus { get; set; }

    public bool IsActive { get; set; }

    public DateTime DueDate { get; set; }

    public int Instalment { get; set; }

    public long? DivisionId { get; set; }

    public virtual TbClass Class { get; set; } = null!;

    public virtual TbDivision? Division { get; set; }

    public virtual TbFee Fee { get; set; } = null!;

    public virtual ICollection<TbDeletedFeeStudent> TbDeletedFeeStudents { get; set; } = new List<TbDeletedFeeStudent>();
}
