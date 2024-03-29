﻿using AutoMapper;
using RatingService.DTO;
using RatingService.Entities;
using RatingService.Exceptions;
using RatingService.Repositories;
using System;
using System.Collections.Generic;

namespace RatingService.Services
{
    public class RatingTypeService : IRatingTypeService
    {
        private readonly IRatingTypeRepository _ratingTypeRepository;
        private readonly IMapper mapper;

        public RatingTypeService(IRatingTypeRepository ratingTypeRepository, IMapper mapper)
        {
            _ratingTypeRepository = ratingTypeRepository;
            this.mapper = mapper;
        }

        public List<RatingTypeDTO> GetAllRatingTypes()
        {
            var types = _ratingTypeRepository.GetAllRatingTypes();
            return mapper.Map<List<RatingTypeDTO>>(types);
        }

        public RatingTypeDTO GetRatingTypeByID(int ratingTypeID)
        {
            var type = _ratingTypeRepository.GetRatingTypeByID(ratingTypeID);

            if (type == null)
            {
                throw new NotFoundException("Rating type with that ID does not exist");
            }

            return mapper.Map<RatingTypeDTO>(type);
        }

        public RatingTypeDTO CreateRatingType(RatingTypeCreationDTO ratingType)
        {
            RatingType type = mapper.Map<RatingType>(ratingType);

            try
            {
                var createdType = _ratingTypeRepository.CreateRatingType(type);
                _ratingTypeRepository.SaveChanges();
                return mapper.Map<RatingTypeDTO>(createdType);
            }
            catch (Exception ex)
            {
                throw new ErrorOccurException(ex.Message);
            }
        }

        public void UpdateRatingType(RatingTypeModifyingDTO ratingType, int typeID)
        {
            var oldType = _ratingTypeRepository.GetRatingTypeByID(typeID);

            if (oldType == null)
            {
                throw new NotFoundException( "There is no type of rating with that ID");
            }

            var newType = mapper.Map<RatingType>(ratingType);
            newType.RatingTypeID = typeID;

            try
            {
                mapper.Map(newType, oldType);
                _ratingTypeRepository.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new ErrorOccurException( "Error updating type of rating: " + ex.Message);

            }
        }

        public void DeleteRatingType(int ratingTypeID)
        {
            var type = _ratingTypeRepository.GetRatingTypeByID(ratingTypeID);

            if (type == null)
            {
                throw new NotFoundException("There is no rating type with that ID!");
            }
            try
            {
                _ratingTypeRepository.DeleteRatingType(ratingTypeID);
                _ratingTypeRepository.SaveChanges();
            }

            catch (Exception ex)
            {
                throw new ErrorOccurException("Error deleting reaction: " + ex.Message);
            }
        }
    }
}
