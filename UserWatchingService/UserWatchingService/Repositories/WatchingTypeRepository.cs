using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using UserWatchingService.CustomException;
using UserWatchingService.Data;
using UserWatchingService.Dtos.WatchingTypeDto;
using UserWatchingService.Entities;
using UserWatchingService.Interfaces;
using UserWatchingService.Logger;

namespace UserWatchingTypeService.Repositories
{
    public class WatchingTypeRepository : IWatchingTypeRepository
    {
        private readonly DataAccessLayer _db;
        private readonly IMapper _mapper;
        private readonly MockLogger _logger;

        public WatchingTypeRepository(MockLogger logger, IMapper mapper, DataAccessLayer db)
        {
            _logger = logger;
            _mapper = mapper;
            _db = db;
        }

        public WatchingTypeConfirmationDto Create(WatchingTypeCreateDto dto)
        {
            WatchingType newWatchingType = new WatchingType()
            {
                Id = Guid.NewGuid(),
                Type = dto.Type
            };

            _db.WatchingTypes.Add(newWatchingType);

            _db.SaveChanges();

            _logger.Log("Create WatchingType!");

            return _mapper.Map<WatchingTypeConfirmationDto>(newWatchingType);
        }

        public List<WatchingTypeReadDto> Get()
        {
            var list = _db.WatchingTypes.ToList();

            _logger.Log("Get WatchingType!");

            return _mapper.Map<List<WatchingTypeReadDto>>(list);
        }

        public WatchingTypeReadDto Get(Guid id)
        {
            var entity = _db.WatchingTypes.FirstOrDefault(e => e.Id == id);

            _logger.Log("Get WatchingType!");

            return _mapper.Map<WatchingTypeReadDto>(entity);
        }

        public WatchingTypeConfirmationDto Update(Guid id, WatchingTypeCreateDto dto)
        {
            var WatchingType = _db.WatchingTypes.FirstOrDefault(e => e.Id == id);

            if (WatchingType == null)
                throw new UserException("WatchingType does not exist");

            WatchingType.Type = dto.Type;

            _db.SaveChanges();

            _logger.Log("Update WatchingType!");

            return _mapper.Map<WatchingTypeConfirmationDto>(WatchingType);
        }

        public void Delete(Guid id)
        {
            var WatchingType = _db.WatchingTypes.FirstOrDefault(e => e.Id == id);

            if (WatchingType == null)
                throw new UserException("WatchingType does not exist");

            _db.WatchingTypes.Remove(WatchingType);

            _logger.Log("Delete WatchingType!");

            _db.SaveChanges();
        }
    }
}
