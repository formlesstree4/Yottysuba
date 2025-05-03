using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace YottySuba.Database;

public partial class YottysubaContext : DbContext
{
    public YottysubaContext()
    {
    }

    public YottysubaContext(DbContextOptions<YottysubaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Ban> Bans { get; set; }

    public virtual DbSet<Board> Boards { get; set; }

    public virtual DbSet<Filter> Filters { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Janitor> Janitors { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("admin_pkey");

            entity.ToTable("admin");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("now()")
                .HasColumnName("created");
            entity.Property(e => e.Disabled)
                .HasDefaultValue(false)
                .HasColumnName("disabled");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Salt).HasColumnName("salt");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("image_pkey");

            entity.ToTable("attachment");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v1()")
                .HasColumnName("id");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("now()")
                .HasColumnName("created");
            entity.Property(e => e.Deleted)
                .HasDefaultValue(false)
                .HasColumnName("deleted");
            entity.Property(e => e.Hash).HasColumnName("hash");
            entity.Property(e => e.Location)
                .HasColumnType("character varying")
                .HasColumnName("location");
            entity.Property(e => e.Size)
                .HasDefaultValue(0L)
                .HasColumnName("size");
        });

        modelBuilder.Entity<Ban>(entity =>
        {
            entity.HasKey(e => e.Ip).HasName("ban_pkey");

            entity.ToTable("ban");

            entity.Property(e => e.Ip).HasColumnName("ip");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("now()")
                .HasColumnName("created");
            entity.Property(e => e.Reason)
                .HasDefaultValueSql("''::character varying")
                .HasColumnType("character varying")
                .HasColumnName("reason");
        });

        modelBuilder.Entity<Board>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("board_pkey");

            entity.ToTable("board");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AgeRestricted)
                .HasDefaultValue(false)
                .HasColumnName("age_restricted");
            entity.Property(e => e.Code)
                .HasMaxLength(4)
                .HasColumnName("code");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("now()")
                .HasColumnName("created");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Enabled)
                .HasDefaultValue(true)
                .HasColumnName("enabled");
            entity.Property(e => e.Extensions)
                .HasColumnType("character varying[]")
                .HasColumnName("extensions");
            entity.Property(e => e.IsReadonly)
                .HasDefaultValue(false)
                .HasColumnName("is_readonly");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("now()")
                .HasColumnName("last_updated");
            entity.Property(e => e.MaxActiveThreads)
                .HasDefaultValue(100)
                .HasColumnName("max_active_threads");
            entity.Property(e => e.MaxFilesizeInBytes)
                .HasDefaultValue(0L)
                .HasColumnName("max_filesize_in_bytes");
            entity.Property(e => e.MaxPostsInThread)
                .HasDefaultValue(250)
                .HasColumnName("max_posts_in_thread");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Filter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("filter_pkey");

            entity.ToTable("filter");

            entity.HasIndex(e => e.Name, "filter_name_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(false)
                .HasColumnName("active");
            entity.Property(e => e.Boards).HasColumnName("boards");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("now()")
                .HasColumnName("created");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("''::character varying")
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Expression)
                .HasDefaultValueSql("''::character varying")
                .HasColumnType("character varying")
                .HasColumnName("expression");
            entity.Property(e => e.Name)
                .HasDefaultValueSql("''::character varying")
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Replace)
                .HasDefaultValueSql("''::character varying")
                .HasColumnType("character varying")
                .HasColumnName("replace");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("board_group_pkey");

            entity.ToTable("group");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Boards).HasColumnName("boards");
            entity.Property(e => e.Name)
                .HasDefaultValueSql("''::character varying")
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Order).HasColumnName("order");
        });

        modelBuilder.Entity<Janitor>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("janitor");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("post_pkey");

            entity.ToTable("post");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Board).HasColumnName("board");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("now()")
                .HasColumnName("created");
            entity.Property(e => e.DeletePassword).HasColumnName("delete_password");
            entity.Property(e => e.File).HasColumnName("file");
            entity.Property(e => e.IpV4).HasColumnName("ip_v4");
            entity.Property(e => e.IpV6).HasColumnName("ip_v6");
            entity.Property(e => e.IsStart)
                .HasDefaultValue(false)
                .HasColumnName("is_start");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("now()")
                .HasColumnName("last_updated");
            entity.Property(e => e.Message)
                .HasDefaultValueSql("''::character varying")
                .HasColumnType("character varying")
                .HasColumnName("message");
            entity.Property(e => e.Name)
                .HasDefaultValueSql("'Anonymous'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Parent).HasColumnName("parent");
            entity.Property(e => e.Subject)
                .HasDefaultValueSql("''::character varying")
                .HasColumnType("character varying")
                .HasColumnName("subject");

            entity.HasOne(d => d.BoardNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.Board)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("post_board_fkey");

            entity.HasOne(d => d.ParentNavigation).WithMany(p => p.InverseParentNavigation)
                .HasForeignKey(d => d.Parent)
                .HasConstraintName("post_parent_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
