using NetCorePress.Authentication;
using Bogus;
using NetCorePress.Models;
using NetCorePress.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace NetCorePress.Services.Seeders
{
    public class PostSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PostSeeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public void Seed(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!context.Posts.Any())
            {
                var userId = _userManager.Users.First().Id;
                var fakePosts = new Faker<Post>()
                    .RuleFor(p => p.Title, f => f.Lorem.Sentence())
                    .RuleFor(p => p.Message, f => f.Lorem.Paragraph())
                    .RuleFor(p => p.UserId, userId)
                    .RuleFor(p => p.Category, f => f.PickRandom<Category>())
                    .RuleFor(p => p.CreationDate, f => f.Date.Past())
                    .RuleFor(p => p.UpdateDate, f => f.Date.Recent())
                    .Generate(15);

                context.Posts.AddRange(fakePosts);
                context.SaveChanges();
            }
        }
    }
}