using AutoMapper;
using MyPhotosDatabase;
using MyPhotosDatabase.Models.DTO;
using System;
using System.Collections.Generic;

namespace PhotoManagerWCF
{
    public class PhotoManagerService : IPhotoManagerService
    {
        public MapperConfiguration _config;
        public IMapper _mapper;
        public PhotoManagerService()
        {
            _config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MediaDTO, Media>();
                cfg.CreateMap<Media, MediaDTO>();
            }) ;
            _mapper = _config.CreateMapper();
        }
        public bool AlreadyInDatabaseAndNotDeleted(string path)
        {
            Console.WriteLine("Recived AlreadyInDatabase(Path: {0})", path);

            bool result = MyPhotosAPI.AlreadyInDatabaseAndNotDeleted(path);

            Console.WriteLine("Response: {0}", result);

            return result;
        }

        public void DeleteMedia(MediaDTO mediaDTO)
        {
            Media media = _mapper.Map<Media>(mediaDTO);

            MyPhotosAPI.DeleteMedia(media);
        }

        public void DeleteMediaById(Guid id)
        {
            Console.WriteLine("Recived DeleteMediaById(Id: {0}).", id);

            MyPhotosAPI.DeleteMediaById(id);

            Console.WriteLine("Response: Media with id {0} has been deleted.", id);
        }

        public List<MediaDTO> GetAll()
        {
            Console.WriteLine("Recived GetAll()");

            List<MediaDTO> result = _mapper.Map<List<Media>, List<MediaDTO>>(MyPhotosAPI.GetAll());

            Console.WriteLine("Response: List with {0} number of media.", result.Count);

            return result;
        }

        public List<MediaDTO> GetAllWhere(string toSearch, bool byDate, bool byEvent, bool byPeople, bool byLocation, bool byTags, bool byDescription)
        {
            Console.WriteLine("Recived GetAllWhere(Search: \"{0}\", ByDate: {1}, ByEvent: {2}, ByPeople: {3}, ByLocation: {4}, ByTages: {5}, ByDescription: {6})",
                              toSearch,
                              byDate,
                              byEvent,
                              byPeople,
                              byLocation,
                              byTags,
                              byDescription);

            Func<Media, bool> predicate = media =>
                !media.Deleted && (media.Name.ToLower().Contains(toSearch.ToLower()) ||
                (media.CreatedDate.ToString().ToLower().Contains(toSearch.ToLower()) && byDate) ||
                (media.Event.ToLower().Contains(toSearch.ToLower()) && byEvent) ||
                (media.People.ToLower().Contains(toSearch.ToLower()) && byPeople) ||
                (media.Location.ToLower().Contains(toSearch.ToLower()) && byLocation) ||
                (media.Tags.ToLower().Contains(toSearch.ToLower()) && byTags) ||
                (media.Description.ToLower().Contains(toSearch.ToLower()) && byDescription));

            List<MediaDTO> result =  _mapper.Map<List<Media>, List<MediaDTO>>(MyPhotosAPI.GetAll(predicate));

            Console.WriteLine("Response: List with {0} number of media.", result.Count);

            return result;
        }

        public MediaDTO GetMediaById(Guid id)
        {
            Console.WriteLine("Recived GetMediaById(Id: {0})", id);

            MediaDTO result = _mapper.Map<MediaDTO>(MyPhotosAPI.GetMediaById(id));

            Console.WriteLine("Respoonse: \n\tMedia Name: {0}", result.ToString());

            return result;
        }

        public void SaveMedia(MediaDTO mediaDTO)
        {
            Console.WriteLine("Recived SaveMedia({0})", mediaDTO.ToString());

            MyPhotosAPI.SaveMedia(_mapper.Map<Media>(mediaDTO));

            Console.WriteLine("Media saved");
        }

        public void UpdateMedia(Guid id, MediaDTO mediaDTO)
        {
            Console.WriteLine("Recived UpdateMedia({0}, {1})", id, mediaDTO.ToString());

            MyPhotosAPI.UpdateMedia(id, _mapper.Map<Media>(mediaDTO));

            Console.WriteLine("Media updated");
        }
    }
}
