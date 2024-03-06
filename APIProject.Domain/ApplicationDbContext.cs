
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using APIProject.Domain.Models;
using System.Data;

namespace APIProject.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MemberPointHistory> PointHistories { get; set; }
        public DbSet<SurveySheet> SurveySheets { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Requests> Requests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<GiftEventParticipant> GiftEventParticipants { get; set; }
        public DbSet<EventChannel> EventChannels { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Stall> Stalls { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<RelatedStall> RelatedStalls { get; set; }
        public DbSet<GiftCodeQR> GiftCodeQRs { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<GiftNews> GiftNews { get; set; }
        public DbSet<QRCode> QRCodes { get; set; }
        public DbSet<QRCodeBill> QRCodeBills { get; set; }
        public DbSet<GiftEvent> GiftEvents { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }

        public IDbConnection Connection => Database.GetDbConnection();

    }
}