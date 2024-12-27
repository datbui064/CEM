using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CEM.Models;

public partial class QlbContext : DbContext
{
    public QlbContext()
    {
    }

    public QlbContext(DbContextOptions<QlbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaiGiang> BaiGiangs { get; set; }

    public virtual DbSet<BaoCao> BaoCaos { get; set; }

    public virtual DbSet<CauHoi> CauHois { get; set; }

    public virtual DbSet<DeCuong> DeCuongs { get; set; }

    public virtual DbSet<KeHoachGiangDay> KeHoachGiangDays { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<PhuongTienCongCu> PhuongTienCongCus { get; set; }

    public virtual DbSet<QuyetDinh> QuyetDinhs { get; set; }

    public virtual DbSet<TaiLieuHocTap> TaiLieuHocTaps { get; set; }
    public         DbSet<Contact> Contacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=DATBUI\\SQLEXPRESS;Initial Catalog=QLB;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<BaiGiang>(entity =>
        {
            entity.HasKey(e => e.MaBaiGiang).HasName("PK__BaiGiang__0773C41E6231551A");

            entity.ToTable("BaiGiang");

            entity.Property(e => e.BucketName).HasMaxLength(500);
            entity.Property(e => e.DuongDanTep).HasMaxLength(255);
            entity.Property(e => e.FileName).HasMaxLength(500);
            entity.Property(e => e.NguoiLapBaiGiang).HasMaxLength(300);
            entity.Property(e => e.TieuDe).HasMaxLength(255);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.BaiGiangs)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__BaiGiang__MaNguo__3C69FB99");
        });

        modelBuilder.Entity<BaoCao>(entity =>
        {
            entity.HasKey(e => e.MaBaoCao).HasName("PK__BaoCao__25A9188CEBDDC425");

            entity.ToTable("BaoCao");

            entity.Property(e => e.DuongDanTep).HasMaxLength(255);
            entity.Property(e => e.TieuDe).HasMaxLength(255);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.BaoCaos)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__BaoCao__MaNguoiD__46E78A0C");
        });

        modelBuilder.Entity<CauHoi>(entity =>
        {
            entity.HasKey(e => e.MaCauHoi).HasName("PK__CauHoi__1937D77B29083BB0");

            entity.ToTable("CauHoi");

            entity.Property(e => e.LoaiCauHoi).HasMaxLength(200);

            entity.HasOne(d => d.MaBaiGiangNavigation).WithMany(p => p.CauHois)
                .HasForeignKey(d => d.MaBaiGiang)
                .HasConstraintName("FK__CauHoi__MaBaiGia__4AB81AF0");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.CauHois)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__CauHoi__MaNguoiD__4AB81AF0");
        });

        modelBuilder.Entity<DeCuong>(entity =>
        {
            entity.HasKey(e => e.MaDeCuong).HasName("PK__DeCuong__A38A0F32F72F6C6B");

            entity.ToTable("DeCuong");

            entity.Property(e => e.BucketName).HasMaxLength(500);
            entity.Property(e => e.DuongDanTep).HasMaxLength(255);
            entity.Property(e => e.FileName).HasMaxLength(500);
            entity.Property(e => e.TieuDe).HasMaxLength(255);

            entity.HasOne(d => d.MaBaiGiangNavigation).WithMany(p => p.DeCuongs)
                .HasForeignKey(d => d.MaBaiGiang)
                .HasConstraintName("FK__DeCuong__MaBaiGi__4E88ABD4");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DeCuongs)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__DeCuong__MaNguoi__403A8C7D");
        });

        modelBuilder.Entity<KeHoachGiangDay>(entity =>
        {
            entity.HasKey(e => e.MaKeHoach).HasName("PK__KeHoachG__88C5741F0C4D1E09");

            entity.ToTable("KeHoachGiangDay");

            entity.Property(e => e.LoaiKeHoach).HasMaxLength(50);
            entity.Property(e => e.TieuDe).HasMaxLength(255);

            entity.HasOne(d => d.MaBaiGiangNavigation).WithMany(p => p.KeHoachGiangDays)
                .HasForeignKey(d => d.MaBaiGiang)
                .HasConstraintName("FK__KeHoachGi__MaBai__52593CB8");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.KeHoachGiangDays)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__KeHoachGi__MaNgu__440B1D61");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D762C5DC6B5E");

            entity.ToTable("NguoiDung");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.HoTen).HasMaxLength(255);
            entity.Property(e => e.KhoaGiangDay).HasMaxLength(100);
            entity.Property(e => e.MatKhau).HasMaxLength(255);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TenDangNhap).HasMaxLength(100);
            entity.Property(e => e.TenDayDu).HasMaxLength(200);
        });

        modelBuilder.Entity<PhuongTienCongCu>(entity =>
        {
            entity.HasKey(e => e.MaCongCu).HasName("PK__PhuongTi__E45C205A2C007690");

            entity.ToTable("PhuongTienCongCu");

            entity.Property(e => e.BucketName).HasMaxLength(500);
            entity.Property(e => e.DuongDanTep).HasMaxLength(255);
            entity.Property(e => e.FileName).HasMaxLength(500);
            entity.Property(e => e.LoaiCongCu).HasMaxLength(255);
            entity.Property(e => e.TenCongCu).HasMaxLength(255);

            entity.HasOne(d => d.MaBaiGiangNavigation).WithMany(p => p.PhuongTienCongCus)
                .HasForeignKey(d => d.MaBaiGiang)
                .HasConstraintName("FK_PhuongTienCongCu_BaiGiang");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.PhuongTienCongCus)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__PhuongTie__MaNgu__4D94879B");
        });

        modelBuilder.Entity<QuyetDinh>(entity =>
        {
            entity.HasKey(e => e.MaQuyetDinh).HasName("PK__QuyetDin__3F6D3FCBBA9F7A52");

            entity.ToTable("QuyetDinh");

            entity.Property(e => e.AiKy).HasMaxLength(200);
            entity.Property(e => e.NgayBanHanh).HasColumnType("datetime");
            entity.Property(e => e.TieuDe).HasMaxLength(255);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.QuyetDinhs)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK__QuyetDinh__MaNgu__398D8EEE");
        });

        modelBuilder.Entity<TaiLieuHocTap>(entity =>
        {
            entity.HasKey(e => e.MaTaiLieu).HasName("PK__TaiLieuH__FD18A657F61C6A49");

            entity.ToTable("TaiLieuHocTap");

            entity.Property(e => e.BucketName).HasMaxLength(500);
            entity.Property(e => e.DuongDan).HasMaxLength(255);
            entity.Property(e => e.FileName).HasMaxLength(500);
            entity.Property(e => e.LoaiTaiLieu).HasMaxLength(100);
            entity.Property(e => e.NhaXuatBan).HasMaxLength(255);
            entity.Property(e => e.TacGia).HasMaxLength(255);
            entity.Property(e => e.TenTaiLieu).HasMaxLength(255);

            entity.HasOne(d => d.MaBaiGiangNavigation).WithMany(p => p.TaiLieuHocTaps)
                .HasForeignKey(d => d.MaBaiGiang)
                .HasConstraintName("FK__TaiLieuHo__MaBai__5BE2A6F2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
