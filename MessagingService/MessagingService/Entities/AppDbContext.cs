using Microsoft.EntityFrameworkCore;

namespace MessagingService.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<GroupConversation> GroupConversations { get; set; }
        public DbSet<GroupMessage> GroupMessages { get; set; }
    }
}
