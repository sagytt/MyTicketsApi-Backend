public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ticket = modelBuilder.Entity<Ticket>();
        ticket.HasKey(t => t.Id);
        ticket.Property(t => t.Id).ValueGeneratedOnAdd();
        ticket.Property(t => t.Title).IsRequired();
        ticket.Property(t => t.Description).IsRequired();
        ticket.Property(t => t.Priority).IsRequired().HasConversion<string>();
        ticket.Property(t => t.Status).IsRequired().HasConversion<string>();
        ticket.Property(t => t.CreatedAt).IsRequired();
    }
}
