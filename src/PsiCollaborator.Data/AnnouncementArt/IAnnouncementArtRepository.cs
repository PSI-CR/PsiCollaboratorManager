using System.Collections.Generic;

namespace PsiCollaborator.Data.AnnouncementArt
{
    public interface IAnnouncementArtRepository
    {
        int Delete(int announcementArtId);
        IEnumerable<IAnnouncementArt> GetAll();
        List<AnnouncementArt> GetByDate();
        IAnnouncementArt GetById(int announcementArtId);
        void Save(IAnnouncementArt announcementArt);
    }
}