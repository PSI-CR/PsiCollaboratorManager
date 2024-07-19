﻿document.addEventListener('DOMContentLoaded', function () {
    let imageUpload = document.getElementById('imageUpload');
    let hiddenImageInput = document.getElementById('imageBase64');

    imageUpload.addEventListener('change', function (event) {
        const file = event.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onloadend = function () {
                hiddenImageInput.value = reader.result.split(',')[1];
            }
            reader.readAsDataURL(file);

            let tempImage = new Image();
            tempImage.src = URL.createObjectURL(file);
            tempImage.onload = () => {
                let image = document.getElementById("Image");
                if (tempImage.width === 900 && tempImage.height === 300) {
                    image.src = URL.createObjectURL(file);
                } else {
                    hiddenImageInput.value = "";
                    imageUpload.value = "";
                    image.src = "/Images/DefaultAnnouncement.jpg";
                    displayError("Dimensiones incorrectas.\n El ancho debe ser de 900 y el alto de 300");
                }
            };
        }
    });

    let form = document.getElementById('EditAnnouncementArtForm');
    form.addEventListener('submit', function (event) {
        let beginDate = new Date(document.getElementById('BeginDateInput').value);
        let endDate = new Date(document.getElementById('EndDateInput').value);

        if (beginDate > endDate) {
            event.preventDefault();
            displayError("La fecha inicial debe ser menor a la fecha final.");
            return;
        }
        if (hiddenImageInput.value === "") {
            event.preventDefault();
            displayError("Se debe cargar una imagen.");
            return;
        }
    });

    function displayError(msj) {
        new Messi(msj, {
            title: 'Error',
            titleClass: 'anim error',
            buttons: [{ id: 0, label: 'Close', val: 'X' }]
        });
    }
});