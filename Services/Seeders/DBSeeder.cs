using Bogus;
using Microsoft.AspNetCore.Identity;
using NetCorePress.Authentication;
using NetCorePress.Models;
using NetCorePress.Models.Enums;

namespace NetCorePress.Services.Seeders
{
    public class DBSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DBSeeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Seed(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!context.Users.Any())
            {
                var fakeUser = new Faker<ApplicationUser>()
                    .RuleFor(u => u.UserName, "andrea")
                    .RuleFor(u => u.Email, "test@test.com")
                    .Generate();

                // 
                var password = "!Test1234";
                await _userManager.CreateAsync(fakeUser, password);
            }

            var userId = _userManager.Users.First().Id;

            if (!context.Posts.Any())
            {
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

            if (!context.Comments.Any())
            {
                var posts = context.Posts.ToList();

                var fakeComments = new Faker<Comment>()
                    .RuleFor(c => c.Text, f => f.Lorem.Paragraph())
                    .RuleFor(c => c.PostId, f => f.PickRandom(posts).Id)
                    .RuleFor(c => c.UserId, userId)
                    .RuleFor(c => c.CreationDate, f => f.Date.Past())
                    .RuleFor(c => c.UpdateDate, f => f.Date.Recent())
                    .Generate(40);

                context.Comments.AddRange(fakeComments);
                context.SaveChanges();
            }
        }
    }
}