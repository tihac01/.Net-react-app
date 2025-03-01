﻿using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public string PhotoId { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
        {
            _context = context;
            _photoAccessor = photoAccessor;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

            if (user == null) return null;

            var photo = user.Photos.FirstOrDefault(x => x.PhotoId == request.PhotoId);

            if (photo == null) return null;

            if (photo.IsMain) return Result<Unit>.Failure("Cannot delete your main photo. Chnage photo and try again");

            var result = await _photoAccessor.DeletePhotoAsyinc(photo.PhotoId);

            if (result == null) return Result<Unit>.Failure("Problem deleting photo from Cloudinary");

            _context.Photos.Remove(photo);

            var success = await _context.SaveChangesAsync() > 0;

            if (success) return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Problem deleting photo from API");
        }
    }
}
