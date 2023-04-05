using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class UpdateAttendence
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAcccesor;

            public Handler(DataContext context, IUserAccessor userAcccesor)
            {
                _userAcccesor = userAcccesor;
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var actitivty = await _context.Activities
                    .Include(a => a.Attendees).ThenInclude(u => u.AppUser)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (actitivty == null) return null;

                var user = await _context.Users.FirstOrDefaultAsync(x => 
                    x.UserName == _userAcccesor.GetUsername());

                if (user == null) return null;

                var hostUserName = actitivty.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

                var attendance = actitivty.Attendees.FirstOrDefault(x => 
                    x.AppUser.UserName == user.UserName);
                
                if (attendance != null && hostUserName == user.UserName)
                    actitivty.IsCancelled = !actitivty.IsCancelled;
                

                if (attendance != null && hostUserName != user.UserName)
                    actitivty.Attendees.Remove(attendance);

                if (attendance == null)
                {
                    attendance = new ActivityAttendee
                    {
                        AppUser = user,
                        Activity = actitivty,
                        IsHost = false
                    };

                    actitivty.Attendees.Add(attendance);
                }

                var result = await _context.SaveChangesAsync() > 0;

                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem update attendence");
            }
        }
    }
}