using PsiCollaborator.Data.PasswordUtilities;
using PsiCollaborator.Data.UserAccount;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PsiCollaborator.Data.Collaborator
{
    public class CollaboratorRepository : DbMapper, ICollaboratorRepository
    {
        public void Save(CollaboratorFull collaborator)
        {
            ExecuteSqlMapObject("Save_Collaborator", collaborator);
        }
        public List<CollaboratorOperator> GetWithoutActiveSchedule()
        {
            var result = ExecuteList<CollaboratorOperator>("Select_Collaborator_Without_Active_Schedule").ToList();
            return result;
        }
        public int Delete(int collaboratorId)
        {
            return ExecuteSql("Delete_Collaborator", new List<DbParameter>() { new DbParameter("param_collaboratorId", ParameterDirection.Input, collaboratorId) });
        }
        public List<CollaboratorUniqueData> GetCollaboratorUniqueData()
        {
            return ExecuteList<CollaboratorUniqueData>("Select_Collaborator_Unique_Data").ToList();
        }

        public List<CollaboratorBirthday> GetLastWeekBirthday()
        {
            return ExecuteList<CollaboratorBirthday>("Select_Collaborator_LastWeek_Birthday").ToList();
        }

        public List<CollaboratorBirthday> GetTodayBirthday()
        {
            return ExecuteList<CollaboratorBirthday>("Select_Collaborator_Today_Birthday").ToList();
        }

        public List<CollaboratorBirthday> GetNextWeekBirthday()
        {
            return ExecuteList<CollaboratorBirthday>("Select_Collaborator_NextWeek_Birthday").ToList();
        }

        public List<CollaboratorBase> GetByIsActive(bool isActive)
        {
            return ExecuteListWithParameters<CollaboratorBase>("Select_All_Collaborators_By_IsActive", new List<DbParameter>() { new DbParameter("param_isActive", ParameterDirection.Input, isActive) }).ToList();
        }

        public List<CollaboratorName> GetAllName()
        {
            return ExecuteList<CollaboratorName>("Select_All_Collaborators_Name").ToList();
        }

        public List<CollaboratorOperator> GetAllOperator()
        {
            return ExecuteList<CollaboratorOperator>("Select_All_Collaborators_Operator").ToList();
        }

        public CollaboratorFull GetFullById(int collaboratorId)
        {
            var result = ExecuteSingle<CollaboratorFull>("Select_Collaborator_Full_By_Id", new List<DbParameter>() {
                new DbParameter("param_collaboratorId", ParameterDirection.Input, collaboratorId)
            });
            return result;
        }

        public CollaboratorDetails GetDetailsById(int collaboratorId)
        {
            var result = ExecuteSingle<CollaboratorDetails>("Select_Collaborator_Details_By_Id", new List<DbParameter>() {
                new DbParameter("param_collaboratorId", ParameterDirection.Input, collaboratorId)
            });
            return result;
        }

        public List<CollaboratorSchedule> GetCollaboratorSchedules()
        {
            var result = ExecuteList<CollaboratorSchedule>("Select_Collaborator_Schedule_Info");
            return result.ToList();
        }

        public CollaboratorAccount GetCollaboratorAccount(int operatorNumber)
        {
            var collaboratorAccount = ExecuteSingle<CollaboratorAccount>("select_collaborator_account", new List<DbParameter>() { new DbParameter("param_operatornumber", ParameterDirection.Input, operatorNumber) });
            return collaboratorAccount;
        }

        public ILockOut VerifyCollaboratorAccountIsLockedOut(int collaboratorId, int maxInvalidAttempts, TimeSpan invalidAttemptsTime, TimeSpan loginLockOutTime)
        {
            return ExecuteSingle<LockOut>("verify_collaborator_unlock", new List<DbParameter>() {
                new DbParameter("param_collaboratorId", ParameterDirection.Input, collaboratorId),
                new DbParameter("param_maxinvalidattempts", ParameterDirection.Input, maxInvalidAttempts),
                new DbParameter("param_invalidattemptstime", ParameterDirection.Input, invalidAttemptsTime),
                new DbParameter("param_loginlockouttime", ParameterDirection.Input, loginLockOutTime)
            });
        }

        public bool CheckIfPasswordIsRecent(int collaboratorId, string password)
        {
            var lastPasswords = ExecuteListWithParameters<Password>("Select_Collaborator_Last_Passwords", new List<DbParameter>()
            { new DbParameter("param_collaboratorId", ParameterDirection.Input, collaboratorId) });
            foreach (var collaboratorPassword in lastPasswords)
            {
                if (PBKDF2Converter.IsValidPassword(password, collaboratorPassword.PasswordHash))
                    return true;
            }
            return false;
        }

        public int UpdatePassword(int collaboratorId, string password, bool needPasswordChange)
        {
            var result = ExecuteSql("Update_Collaborator_Password", new List<DbParameter>() {
                new DbParameter("param_collaboratorId", ParameterDirection.Input, collaboratorId),
                new DbParameter("param_password", ParameterDirection.Input, PBKDF2Converter.GetHashPassword(password)),
                new DbParameter("param_needPasswordChange", ParameterDirection.Input, needPasswordChange)
            });
            return result;
        }

        public int GetHighestOperatorNumber()
        {
            var operatorNum = 0;
            ExecuteSql("Select_Highest_OperatorNumber", new List<DbParameter>() { new DbParameter("highest_operator_number", ParameterDirection.Output, operatorNum) });
            operatorNum = OutParameters.Count > 0 ? (int)OutParameters[0].Value : 0;
            return operatorNum;
        }

        public CollaboratorPicture GetCollaboratorPictureById(int collaboratorId)
        {
            return ExecuteSingle<CollaboratorPicture>("select_collaborator_picture", new List<DbParameter>() { new DbParameter("param_collaboratorid", ParameterDirection.Input, collaboratorId) });
        }

        public void Update(CollaboratorAccount collaborator)
        {
            ExecuteSqlMapObject("update_collaborator_account", collaborator);
        }
    }
}
