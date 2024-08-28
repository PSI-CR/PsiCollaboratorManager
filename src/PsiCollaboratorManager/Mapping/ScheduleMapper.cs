using PsiCollaborator.Data.Schedule;
using PsiCollaborator.Data.Schedule.ScheduleDaily;
using PsiCollaborator.Data.Schedule.WorkingDay;
using PsiCollaboratorManager.Models;
using PsiCollaboratorManager.Models.Schedule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PsiCollaboratorManager.Mapping
{
    public class ScheduleMapper
    {
        public WorkingDay MapDisplay(WorkingDayDisplayModel model)
        {
            return new WorkingDay
            {
                WorkingDayId = model.WorkingDayId,
                Name = model.Name,
                Description = model.Description,
                MaxDays = model.MaxDays,
                MaxHours = model.MaxHours,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                RecordTime = model.RecordTime,
                Accumulative = model.Accumulative,
                Assigned = model.Assigned == "Si" 
            };
        }

        public WorkingDayDisplayModel MapDisplay(WorkingDay dto)
        {
            return new WorkingDayDisplayModel
            {
                WorkingDayId = dto.WorkingDayId,
                Name = dto.Name,
                Description = dto.Description,
                MaxDays = dto.MaxDays,
                MaxHours = dto.MaxHours,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                RecordTime = dto.RecordTime,
                Accumulative = dto.Accumulative,
                Assigned = dto.Assigned ? "Si":"No" 
            };
        }

        public WorkingDay Map(WorkingDayModel model)
        {
            return new WorkingDay
            {
                WorkingDayId = model.WorkingDayId,
                Name = model.Name,
                Description = model.Description,
                MaxDays = model.MaxDays,
                MaxHours = model.MaxHours,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                RecordTime = DateTime.Now,
                Accumulative = model.Accumulative,
                Assigned = model.Assigned
            };
        }

        public WorkingDayModel Map(WorkingDay dto)
        {
            return new WorkingDayModel
            {
                WorkingDayId = dto.WorkingDayId,
                Name = dto.Name,
                Description = dto.Description,
                MaxDays = dto.MaxDays,
                MaxHours = dto.MaxHours,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                RecordTime = dto.RecordTime,
                Accumulative = dto.Accumulative,
                Assigned = dto.Assigned
            };
        }

        public ScheduleData Map(ScheduleDataModel model)
        {
            return new ScheduleData
            {
                ScheduleId = model.ScheduleId,
                Name = model.Name,
                WorkingDayName = model.WorkingDayName,
                WorkingDayDescription = model.WorkingDayDescription,
                Assigned = model.Assigned == "Si"
            };
        }

        public ScheduleDataModel Map(ScheduleData dto)
        {
            return new ScheduleDataModel
            {
                ScheduleId = dto.ScheduleId,
                Name = dto.Name,
                WorkingDayName = dto.WorkingDayName,
                WorkingDayDescription = dto.WorkingDayDescription,
                Assigned = dto.Assigned ? "Si" : "No"
            };
        }

        public Schedule MapScheduleModelToSchedule2(ScheduleModel model)
        {
            if (model == null) return null;

            var scheduleDailys = model.ScheduleDailys?
                .Select(daily =>
                {
                    string beginTime = $"{daily.BeginTime.Hours:D2}:{daily.BeginTime.Minutes:D2}:00";
                    string endTime = $"{daily.EndTime.Hours:D2}:{daily.EndTime.Minutes:D2}:00";
                    return $"{daily.ScheduleDayName},{beginTime},{endTime}";
                })
                .ToArray();

            return new Schedule
            {
                ScheduleId = model.ScheduleId,
                Name = model.Name,
                WorkingdayId = model.WorkingDayId,
                ScheduleDailys = scheduleDailys
            };
        }

        public Schedule MapScheduleModelToSchedule(ScheduleModel model)
        {
            if (model == null) return null;

            var scheduleDailys = model.ScheduleDailys?
                .Select(daily =>
                {
                    DateTime today = DateTime.Today;
                    DateTime beginDateTime = new DateTime(today.Year, today.Month, today.Day, daily.BeginTime.Hours, daily.BeginTime.Minutes, 0);
                    DateTime endDateTime = new DateTime(today.Year, today.Month, today.Day, daily.EndTime.Hours, daily.EndTime.Minutes, 0);

                    string beginTime = beginDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    string endTime = endDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                    return $"{daily.ScheduleDayId},{beginTime},{endTime}";
                })
                .ToArray();

            return new Schedule
            {
                ScheduleId = model.ScheduleId,
                Name = model.Name,
                WorkingdayId = model.WorkingDayId,
                ScheduleDailys = scheduleDailys
            };
        }

        //***************************************************************************************

        private List<ScheduleDailyModel> ConvertScheduleDailys(string[] scheduleDailys)
        {
            var scheduleDailyModels = new List<ScheduleDailyModel>();

            foreach (var daily in scheduleDailys)
            {
                var cleanedData = daily
                    .Replace("ScheduleDailyId:", "").Trim() 
                    .Replace("ScheduleDayId:", "").Trim() 
                    .Replace("ScheduleDayName:", "").Trim()
                    .Replace("BeginTime:", "").Trim()
                    .Replace("EndTime:", "").Trim(); 

                var parts = cleanedData.Split(',').Select(p => p.Trim()).ToArray();
              
                if (parts.Length == 5)
                {
                    try
                    {
                        var scheduleDailyId = int.Parse(parts[0]);
                        var scheduleDayId = int.Parse(parts[1]);
                        var scheduleDayName = parts[2];
                        var beginDateTime = DateTime.Parse(parts[3]);
                        var endDateTime = DateTime.Parse(parts[4]);

                        var scheduleDailyModel = new ScheduleDailyModel
                        {
                            ScheduleDailyId = scheduleDailyId,
                            ScheduleDayId = scheduleDayId, 
                            ScheduleDayName = scheduleDayName,
                            BeginTime = ConvertToTimeModel(beginDateTime),
                            EndTime = ConvertToTimeModel(endDateTime)
                        };
                        scheduleDailyModels.Add(scheduleDailyModel);
                    }
                    catch (FormatException ex)
                    {                    
                        Debug.WriteLine($"FormatException: {ex.Message}");
                    }
                }
                else
                {        
                    Debug.WriteLine($"Unexpected number of parts: {parts.Length}");
                }
            }

            return scheduleDailyModels;
        }

        private TimeModel ConvertToTimeModel(DateTime dateTime)
        {
            return new TimeModel
            {
                Hours = dateTime.Hour,
                Minutes = dateTime.Minute
            };
        }

        private WorkingDayModel ConvertToWorkingDayModel(WorkingDay workingDay)
        {
            return new WorkingDayModel
            {
                WorkingDayId = workingDay.WorkingDayId,
                Name = workingDay.Name,
                Description = workingDay.Description,
                MaxDays = workingDay.MaxDays,
                MaxHours = workingDay.MaxHours,
                StartTime = workingDay.StartTime,
                EndTime = workingDay.EndTime,
                Accumulative = workingDay.Accumulative,
                Assigned = workingDay.Assigned,
                RecordTime = workingDay.RecordTime
            };
        }

        public ScheduleWorkingDayModel MapScheduleToModel(Schedule schedule, WorkingDay workingDay)
        {
            var scheduleModel = new ScheduleWorkingDayModel
            {
                ScheduleId = schedule.ScheduleId,
                Name = schedule.Name,
                WorkingDay = ConvertToWorkingDayModel(workingDay),
                ScheduleDailys = ConvertScheduleDailys(schedule.ScheduleDailys)
            };
            return scheduleModel;
        }

        public ScheduleDailyModel MapToScheduleDailyModel(ScheduleDaily scheduleDaily)
        {
            return new ScheduleDailyModel
            {
                ScheduleDailyId = scheduleDaily.ScheduleDailyId,
                ScheduleDayName = scheduleDaily.ScheduleDayName,
                BeginTime = new TimeModel
                {
                    Hours = scheduleDaily.BeginTime.Hour,
                    Minutes = scheduleDaily.BeginTime.Minute
                },
                EndTime = new TimeModel
                {
                    Hours = scheduleDaily.EndTime.Hour,
                    Minutes = scheduleDaily.EndTime.Minute
                }
            };
        }
    }
}