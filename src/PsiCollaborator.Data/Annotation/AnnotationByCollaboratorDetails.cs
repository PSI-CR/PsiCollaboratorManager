using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.Annotation
{
    public class AnnotationByCollaboratorDetails : IAnnotationByCollaboratorDetails
    {
        public AnnotationByCollaboratorDetails() { }

        public AnnotationByCollaboratorDetails(int collaboratorId, string firstName, string lastName, string dNICollaborator, int operatorNumber, string email, bool isActive,
            int annotationTypeId, string typeName, bool valueInScore, bool visibleToCollaborator, double percentage, int annotationId, int userId, int attachedFileId, string note,
            DateTime? annotationDate, string fileData, string fileName, string fileType, int collaboratorPictureId, string picture)
        {
            CollaboratorId = collaboratorId;
            FirstName = firstName;
            LastName = lastName;
            DNICollaborator = dNICollaborator;
            OperatorNumber = operatorNumber;
            Email = email;
            IsActive = isActive;
            AnnotationTypeId = annotationTypeId;
            TypeName = typeName;
            ValueInScore = valueInScore;
            VisibleToCollaborator = visibleToCollaborator;
            Percentage = percentage;
            AnnotationId = annotationId;
            UserId = userId;
            AttachedFileId = attachedFileId;
            Note = note;
            AnnotationDate = annotationDate;
            FileData = fileData;
            FileName = fileName;
            FileType = fileType;
            CollaboratorPictureId = collaboratorPictureId;
            Picture = picture;
        }

        public int CollaboratorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DNICollaborator { get; set; }
        public int OperatorNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int AnnotationTypeId { get; set; }
        public string TypeName { get; set; }
        public bool ValueInScore { get; set; }
        public bool VisibleToCollaborator { get; set; }
        public double Percentage { get; set; }
        public int AnnotationId { get; set; }
        public int UserId { get; set; }
        public int AttachedFileId { get; set; }
        public string Note { get; set; }
        public DateTime? AnnotationDate { get; set; }
        public string FileData { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int CollaboratorPictureId { get; set; }
        public string Picture { get; set; }
    }
}
