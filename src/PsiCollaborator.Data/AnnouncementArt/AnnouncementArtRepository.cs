using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.AnnouncementArt
{
    public class AnnouncementArtRepository : DbMapper, IAnnouncementArtRepository
    {
        public List<AnnouncementArt> GetByDate()
        {
            return ExecuteListWithParameters<AnnouncementArt>("select_announcement_art_by_date", new List<DbParameter>()).ToList();
        }

        public void Save(IAnnouncementArt announcementArt)
        {
            ExecuteSqlMapObject("save_announcement_art", announcementArt);
        }

        public int Delete(int announcementArtId)
        {
            return ExecuteSql("delete_announcement_art", new List<DbParameter>() { new DbParameter("param_announcement_art_id", ParameterDirection.Input, announcementArtId) });
        }

        public IAnnouncementArt GetById(int announcementArtId)
        {
            return ExecuteSingle<AnnouncementArt>("select_announcement_art_by_id", new List<DbParameter>() { new DbParameter("param_announcement_art_id", ParameterDirection.Input, announcementArtId) });
        }

        public IEnumerable<IAnnouncementArt> GetAll()
        {
            return ExecuteList<AnnouncementArt>("select_all_announcements_art");
        }
    }
}
