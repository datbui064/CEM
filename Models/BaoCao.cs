using System;
using System.Collections.Generic;

namespace CEM.Models;

public partial class BaoCao
{
    public string? MaBaoCao { get; set; }

    public string? TieuDe { get; set; }

    public string? NoiDung { get; set; }

    public string? DuongDanTep { get; set; }

    public int MaNguoiDung { get; set; }
    public string? FileName { get; set; }

    public string? BucketName { get; set; }

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
}
