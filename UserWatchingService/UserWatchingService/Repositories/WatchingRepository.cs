using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using UserWatchingService.CustomException;
using UserWatchingService.Data;
using UserWatchingService.Dtos.WatchingDto;
using UserWatchingService.Entities;
using UserWatchingService.Interfaces;
using UserWatchingService.Logger;

namespace WatchingWatchingService.Repositories
{
    public class WatchingRepository : IWatchingRepository
    {
        private readonly DataAccessLayer _db;
        private readonly IMapper _mapper;
        private readonly MockLogger _logger;

        public WatchingRepository(MockLogger logger, IMapper mapper, DataAccessLayer db)
        {
            _logger = logger;
            _mapper = mapper;
            _db = db;
        }

        public WatchingConfirmationDto Create(WatchingCreateDto dto)
        {
            Watching newWatching = new Watching()
            {
                Id = Guid.NewGuid(),
                WatchedId = dto.WatchedId,
                WatcherId = dto.WatcherId
            };

            _db.Watchings.Add(newWatching);

            _db.SaveChanges();

            _logger.Log("Create Watching!");

            return _mapper.Map<WatchingConfirmationDto>(newWatching);
        }

        public List<WatchingReadDto> Get()
        {
            var list = _db.Watchings.ToList();

            _logger.Log("Get Watching!");

            return _mapper.Map<List<WatchingReadDto>>(list);
        }

        public WatchingReadDto Get(Guid id)
        {
            var entity = _db.Watchings.FirstOrDefault(e => e.Id == id);

            _logger.Log("Get Watching!");

            return _mapper.Map<WatchingReadDto>(entity);
        }

        public WatchingConfirmationDto Update(Guid id, WatchingCreateDto dto)
        {
            var watching = _db.Watchings.FirstOrDefault(e => e.Id == id);

            if (watching == null)
                throw new UserException("Watching does not exist");

            watching.WatchedId = dto.WatchedId;
            watching.WatcherId = dto.WatcherId;

            _db.SaveChanges();

            _logger.Log("Update Watching!");

            return _mapper.Map<WatchingConfirmationDto>(watching);
        }

        public void Delete(Guid id)
        {
            var Watching = _db.Watchings.FirstOrDefault(e => e.Id == id);

            if (Watching == null)
                throw new UserException("Watching does not exist");

            _db.Watchings.Remove(Watching);

            _logger.Log("Delete Watching!");

            _db.SaveChanges();
        }
    }
}
