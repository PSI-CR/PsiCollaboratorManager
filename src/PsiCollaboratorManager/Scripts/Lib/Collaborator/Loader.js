let form = document.getElementById("FormCollaboratorContainer");
form.addEventListener("submit", async (e) => {
    e.preventDefault();
    try {
        let errorMessage = "";
        let base64Image = await GetBase64();
        if (base64Image == null) errorMessage += "Por favor ingrese una foto. \n";
        let emergencyContacts = extension.GetContacts();
        if (emergencyContacts.length == 0) errorMessage += "Por favor ingrese al menos un contacto de emergencia. \n";
        if (errorMessage != "") new Messi(errorMessage, { title: 'Error', titleClass: 'anim error', modal: true }).show();
        else {
            let collaborator = CollaboratorFull.FromGui(extension.GetContacts(), base64Image);
            InsertCollaborator(collaborator);
        }
    }
    catch (ex) {
        new Messi(ex, { title: 'Error', titleClass: 'anim error', modal: true }).show();
    }
});
async function GetBase64() {
    let file = document.getElementById('file_input').files[0];
    if (!file) {
        let pictureHidden = document.getElementById("Picture");
        if (pictureHidden == null) return null;
        let pictureHiddenValue = pictureHidden.value;
        if (pictureHiddenValue == "") return null;
        return pictureHiddenValue;
    }
    var reader = new FileReader();
    reader.readAsDataURL(file);
    let promise = new Promise((resolve, reject) => {
        reader.onload = function () {
            resolve(reader.result);
        };
        reader.onerror = function (error) {
            reject("");
        };
    });
    let base64Image = "";
    await promise.then(val => { base64Image = val });
    return base64Image;
}

function InsertCollaborator(collaborator) {
    $.ajax({
        url: "/Collaborator/Save",
        type: 'POST',
        dataType: 'json',
        data: {
            collaborator: JSON.stringify(collaborator)
        },
        success: (data) => {
            if (data != "") {
                new Messi(data, { title: 'Error', titleClass: 'anim error', modal: true }).show();
            }
            else {
                if (collaborator.IsActive) window.location.href = "../../Collaborator/Index";
                else window.location.href = "../../Collaborator/InactiveCollaborators";
            }
        },
        error: (ex) => {
            new Messi(ex, { title: 'Error', titleClass: 'anim error', modal: true }).show();
        }
    });
}