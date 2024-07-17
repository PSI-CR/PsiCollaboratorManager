using System;

namespace PsiCollaborator.Data.AnnouncementArt
{
    public interface IAnnouncementArt
    {
        int AnnouncementArtId { get; set; }
        DateTime BeginDatePublication { get; set; }
        string Description { get; set; }
        DateTime EndDatePublication { get; set; }
        string Image { get; set; }
    }
}