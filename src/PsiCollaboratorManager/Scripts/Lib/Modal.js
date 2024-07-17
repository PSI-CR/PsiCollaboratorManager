function OpenModal(id) {
    document.getElementById(id).style.display = "block";
}
function OpenModal(id, title) {
    document.getElementById(id).style.display = "block";
    $("#" + id + " .modalHeaderTitle").html(title);
}
function CloseModal(id) {
    document.getElementById(id).style.display = "none";
}
function ToggleModal(id){
    let modal = document.getElementById(id);
    if (modal.style.display == "none") modal.style.display = "block";
    else if (modal.style.display == "block") modal.style.display = "none";
}

$(".modalContainer").on("pointerup", (e) => {
    let parent = e.target.parentElement;
    if ($(parent).hasClass("modalContainer")) {
        CloseModal(parent.id);
    }
    else if ($(parent.parentElement).hasClass("modalContainer")) {
        CloseModal(parent.parentElement.id);
    }
});