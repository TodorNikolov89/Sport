namespace Sport.Tests.Fakes
{
    using AutoMapper;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Sport.Profiles;
    using System.Threading.Tasks;

    public class FakeSportDbContext
    {
        public FakeSportDbContext(string name)
        {
            var options = new DbContextOptionsBuilder<SportDbContext>()
               .UseInMemoryDatabase(name)
               .Options;            

             this.Data = new SportDbContext(options);
        }


        public SportDbContext Data { get; }

        public async Task Add(params object[] data)
        {
            this.Data.AddRange(data);
           await this.Data.SaveChangesAsync();
        }
    }
}
