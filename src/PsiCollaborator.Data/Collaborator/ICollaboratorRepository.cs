using System;
using System.Collections.Generic;

namespace PsiCollaborator.Data.Collaborator
{
    public interface ICollaboratorRepository
    {
        bool CheckIfPasswordIsRecent(int collaboratorId, string password);
        int Delete(int collaboratorId);
        List<CollaboratorName> GetAllName();
        List<CollaboratorOperator> GetAllOperator();
        List<CollaboratorBase> GetByIsActive(bool isActive);
        ICollaboratorAccount GetCollaboratorAccount(int operatorNumber);
        List<CollaboratorSchedule> GetCollaboratorSchedules();
        List<CollaboratorUniqueData> GetCollaboratorUniqueData();
        CollaboratorDetails GetDetailsById(int collaboratorId);
        CollaboratorFull GetFullById(int collaboratorId);
        List<CollaboratorBirthday> GetLastWeekBirthday();
        List<CollaboratorBirthday> GetNextWeekBirthday();
        List<CollaboratorBirthday> GetTodayBirthday();
        List<CollaboratorOperator> GetWithoutActiveSchedule();
        void Save(CollaboratorFull collaborator);
        int UpdatePassword(int collaboratorId, string password);
        ILockOut VerifyCollaboratorAccountIsLockedOut(int collaboratorId, int maxInvalidAttempts, TimeSpan invalidAttemptsTime, TimeSpan loginLockOutTime);
        int GetHighestOperatorNumber();
    }
}