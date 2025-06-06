using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Calendar
{
    public class CalendarRepository : DbMapper, ICalendarRepository
    {
        public void Save(Calendar calendarEvent, List<int> collaboratorIds)
        {
            var parameters = new List<DbParameter>
            {
                new DbParameter("param_title", ParameterDirection.Input, calendarEvent.Title ?? string.Empty),
                new DbParameter("param_start", ParameterDirection.Input, calendarEvent.Start),
                new DbParameter("param_end", ParameterDirection.Input, calendarEvent.End == default ? DBNull.Value : (object)calendarEvent.End),
                new DbParameter("param_allday", ParameterDirection.Input, calendarEvent.AllDay),
                new DbParameter("param_description", ParameterDirection.Input, string.IsNullOrEmpty(calendarEvent.Description) ? DBNull.Value : (object)calendarEvent.Description),
                new DbParameter("param_color", ParameterDirection.Input, string.IsNullOrEmpty(calendarEvent.Color) ? DBNull.Value : (object)calendarEvent.Color)
            };

            var eventId = ExecuteFunction<int>("save_calendar_event", parameters, useSelectSyntax: true);


            foreach (var collaboratorId in collaboratorIds)
            {
                var collaboratorParameters = new List<DbParameter>
                {
                    new DbParameter("param_event_id", ParameterDirection.Input, eventId),
                    new DbParameter("param_collaborator_id", ParameterDirection.Input, collaboratorId)
                };

                ExecuteFunction<int>("save_colaborador_event", collaboratorParameters, useSelectSyntax: true);
            }
        }


        public List<Calendar> GetAll()
        {
            return ExecuteList<Calendar>("select_all_calendar_events").ToList();
        }

        public Calendar GetById(int id)
        {
            return ExecuteSingle<Calendar>("select_calendar_event_by_id", new List<DbParameter>
            {
                new DbParameter("param_id", ParameterDirection.Input, id)
            });
        }

        public void Delete(int id)
        {
            ExecuteSql("delete_calendar_event", new List<DbParameter>
            {
                new DbParameter("param_event_id", ParameterDirection.Input, id)
            });
        }
        public void Update(Calendar calendarEvent)
        {
            var parameters = new List<DbParameter>
            {
        new DbParameter("param_event_id", ParameterDirection.Input, calendarEvent.Event_Id),
        new DbParameter("param_title", ParameterDirection.Input, calendarEvent.Title ?? string.Empty),
        new DbParameter("param_start", ParameterDirection.Input, calendarEvent.Start),
        new DbParameter("param_end", ParameterDirection.Input, calendarEvent.End == default ? DBNull.Value : (object)calendarEvent.End),
        new DbParameter("param_allday", ParameterDirection.Input, calendarEvent.AllDay),
        new DbParameter("param_description", ParameterDirection.Input, string.IsNullOrEmpty(calendarEvent.Description) ? DBNull.Value : (object)calendarEvent.Description),
        new DbParameter("param_color", ParameterDirection.Input, string.IsNullOrEmpty(calendarEvent.Color) ? DBNull.Value : (object)calendarEvent.Color)
            };

            ExecuteSql("update_calendar_event", parameters);
        }

        public List<CollaboratorByEvent> GetCollaboratorByEventId(int eventId)
        {
            return ExecuteListWithParameters<CollaboratorByEvent>("select_collaborators_by_event_id", new List<DbParameter>() {
                new DbParameter("param_event_id", ParameterDirection.Input, eventId) 
            }).ToList();
        }
    }
}
