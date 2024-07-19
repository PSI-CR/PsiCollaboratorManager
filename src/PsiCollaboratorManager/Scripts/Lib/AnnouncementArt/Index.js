function editAnnouncementArt(id) {
    window.location.replace("../../AnnouncementArt/Edit?announcementArtId=" + id);
}

function deleteAnnouncementArt(id) {
new Messi(
    '¿Desea eliminar este anuncio?.',
    {
        title: 'Eliminar',
        titleClass: 'anim warning',
        buttons: [
            { id: 0, label: 'Si', val: 'Y' },
            { id: 1, label: 'No', val: 'N' }
        ],
        callback: function (val) {
            if (val == 'Y') {
                $.ajax({
                    url: '/AnnouncementArt/Delete',
                    type: 'POST',
                    data: { announcementArtId: id },
                    success: function (response) {
                        window.location.href = '/AnnouncementArt/Index';
                    },
                    error: function () {
                        alert('Error al eliminar el elemento.');
                    }
                });
            }
        }
    });
}