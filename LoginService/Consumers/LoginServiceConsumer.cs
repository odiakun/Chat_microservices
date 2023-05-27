namespace LoginService.Consumers{
    using MassTransit;
    using System.Threading.Tasks;
    using Contracts;
    using LoginService.Models;
    using Microsoft.EntityFrameworkCore;

    public class LoginServiceConsumer : IConsumer<AddUser>, IConsumer<GetUser>, IConsumer<DeleteUser>
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly UserDb _db;

        readonly IPublishEndpoint _publishEndpoint;

        public LoginServiceConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _db = _serviceProvider.GetService<UserDb>();
        }

        public async Task Consume(ConsumeContext<GetUser> context){
            string username = context.Message.Username;
            
            User user = await _db.Users.SingleOrDefaultAsync(x => x.Username == username);

            if (user != null)
            {
                await context.Publish<UserFound>(new {
                    Username = username
                });
            }
            else
            {
                await context.Publish<UserNotFound>(new {
                    Username = username
                });
            }
        }

        public async Task Consume(ConsumeContext<DeleteUser> context){
            string username = context.Message.Username;

            User user = await _db.Users.SingleOrDefaultAsync(x => x.Username == username);

            if (user != null)
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                // await context.Publish<UserDeleted>(new {
                //     Username = username
                // });
            }
            else
            {
                Console.WriteLine("Tried deleting non existant user");
            }
        }

        public async Task Consume(ConsumeContext<AddUser> context){
            UserDTO userDTO = context.Message.userDTO;

            var username = userDTO.Username;
            var email = userDTO.Email;
            var gender = userDTO.Gender;

            var user = new User
            {
                Username = username,
                Email = email,
                Gender = gender
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            await context.Publish<UserAdded>(new {
                Username = user.Username
            });
        }
    }
}