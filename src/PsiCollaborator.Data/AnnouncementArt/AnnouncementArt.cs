using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsiCollaborator.Data.AnnouncementArt
{
    public class AnnouncementArt : IAnnouncementArt
    {
        public int AnnouncementArtId { get; set; }
        public string Image { get; set; }
        public DateTime BeginDatePublication { get; set; }
        public DateTime EndDatePublication { get; set; }
        public string Description { get; set; }
    }
}
