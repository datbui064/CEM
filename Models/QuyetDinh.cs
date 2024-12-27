using System;
using System.Collections.Generic;

namespace CEM.Models;

public partial class QuyetDinh
{
    public string MaQuyetDinh { get; set; }

    public string? TieuDe { get; set; }

    public string? MoTa { get; set; }

    public DateTime? NgayBanHanh { get; set; }

    public int? MaNguoiDung { get; set; }

    public string? AiKy { get; set; }
    public string? FileName { get; set; }

    public string? BucketName { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
}
