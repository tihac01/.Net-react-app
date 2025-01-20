using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class ListActivities
{
    public class Query : IRequest<Result<List<UserActivityDto>>>
    {
        public string Predicate { get; set; }
        public string UserName { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<UserActivityDto>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Activities
                .Where(x => x.Attendees.Any(a => a.AppUser.UserName == request.UserName) && 
                            x.Date >= DateTime.UtcNow)
                .OrderBy(x => x.Date)
                .AsQueryable();

            query = request.Predicate switch
            {
                "past" => query.Where(x => x.Date < DateTime.UtcNow),
                "hosting" => query.Where(x => x.Attendees.Any(a =>
                    a.AppUser.UserName == request.UserName &&
                    a.IsHost)),
                _ => query
            };

            var activities = await query
                .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider,
                    new { currentUserName = _userAccessor.GetUserName() })
                .ToListAsync(cancellationToken);

            return Result<List<UserActivityDto>>.Success(activities);
        }
    }
}
