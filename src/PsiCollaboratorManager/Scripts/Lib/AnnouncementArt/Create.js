document.addEventListener('DOMContentLoaded', function () {
    let imageUpload = document.getElementById('imageUpload');

    imageUpload.addEventListener('change', function (event) {
        const file = event.target.files[0];
        if (file) {
            const hiddenImageInput = document.getElementById('imageBase64');
            const fileURL = URL.createObjectURL(file);

            let tempImage = new Image();
            tempImage.src = fileURL;

            tempImage.onload = () => {

                let image = document.getElementById("Image");
                let canvas = document.createElement("canvas");
                let ctx = canvas.getContext("2d");

                canvas.width = 900;
                canvas.height = 300;

                ctx.drawImage(tempImage, 0, 0, canvas.width, canvas.height);

                let resizedBase64 = canvas.toDataURL("image/jpeg", 1.0);

                image.src = resizedBase64;

                hiddenImageInput.value = resizedBase64.split(',')[1];
                URL.revokeObjectURL(fileURL);
            };
        }
    });

    let form = document.getElementById('CreateAnnouncementArtForm');

    form.addEventListener('submit', function (event) {
        let beginDateInput = document.getElementById('BeginDateInput').value;
        let endDateInput = document.getElementById('EndDateInput').value;

        if (!beginDateInput || !endDateInput) {
            event.preventDefault();
            new Messi("Debe ingresar ambas fechas.", {
                title: 'Error',
                titleClass: 'anim error',
                buttons: [{ id: 0, label: 'Close', val: 'X' }]
            });
            return;
        }

        let beginDate = new Date(beginDateInput);
        let endDate = new Date(endDateInput);

        if (beginDate.toString() === "Invalid Date" || endDate.toString() === "Invalid Date") {
            event.preventDefault();
            new Messi("Ingrese fechas válidas.", {
                title: 'Error',
                titleClass: 'anim error',
                buttons: [{ id: 0, label: 'Close', val: 'X' }]
            });
            return;
        }

        if (beginDate > endDate) {
            event.preventDefault();
            new Messi("La fecha inicial debe ser menor a la fecha final.", {
                title: 'Error',
                titleClass: 'anim error',
                buttons: [{ id: 0, label: 'Close', val: 'X' }]
            });
            return;
        }
    });
});
