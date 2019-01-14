using Core.Entities.Base;
using nevladinaOrg.Web.Constants;
using nevladinaOrg.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace nevladinaOrg.Web.Areas.Administration.ViewModels
{
    public class EventItemViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessageEventItemTypeReq))]
        public IEnumerable<string> EventItemTypeIds { get; set; }
        public IEnumerable<string> LecturerIds { get; set; }
        public string EventItemTypeName { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessageNameReq))]
        public string Name { get; set; }
        [Required(ErrorMessage = nameof(Localizer.ErrorMessageStartTimeReq))]
        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = nameof(Localizer.ErrorMessageDurationReq))]
        public int Duration { get; set; }
        [Required(ErrorMessage =nameof(Localizer.ErrorMessageConferenceRoomReq))]
        public string ConferenceRoom { get; set; }
        public bool IsLecture { get; set; }
        public string NewLecturer { get; set; }
        public string LecturerFirstName { get; set; }
        public string LecturerLastName { get; set; }
        public string LecturerBiography { get; set; }
        public string AboutLecture { get; set; }

        public Enumerations.SaveAndOptions SaveAndOptions { get; set; }

        public static implicit operator EventItem(EventItemViewModel model)
        {
            EventItem eventItem = new EventItem()
            {
                Id = model.Id,
                Name = model.Name,
                ConferenceRoom=model.ConferenceRoom,
                EndTime=model.EndTime,
                EventId=model.EventId,
                StartTime=model.StartTime
            };

            return eventItem;
        }
        public static implicit operator EventItemViewModel(EventItem model)
        {
            EventItemViewModel eventItem = new EventItemViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                ConferenceRoom = model.ConferenceRoom,
                EndTime = model.EndTime,
                EventId = model.EventId,
                StartTime = model.StartTime
            };

            return eventItem;
        }
    }
}
