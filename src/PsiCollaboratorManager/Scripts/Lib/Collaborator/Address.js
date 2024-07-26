let provinceSelect = document.getElementById("ProvinceId");
let cantonSelect = document.getElementById("CantonId");
let districtSelect = document.getElementById("DistrictId");

provinceSelect.addEventListener("change", () => {
    SetCantonsByProvince(provinceSelect.value);
});

cantonSelect.addEventListener("change", () => {
    SetDistrictsByCanton(cantonSelect.value);
});
function SetCantonsByProvince(provinceId) {
    $.ajax({
        type: 'POST',
        url: "/Collaborator/GetCantonsByProvince",
        dataType: 'json',
        data: {
            provinceId: provinceId
        },
        success: (data) => {
            clearOptions(cantonSelect);
            let firstCantonId = 101;
            data.forEach((x, index) => {
                if (index == 0) firstCantonId = x.CantonId;
                let option = document.createElement('option');
                option.value = x.CantonId;
                option.innerHTML = x.Name;
                cantonSelect.appendChild(option);
            });
            SetDistrictsByCanton(firstCantonId);
        },
        error: (ex) => {
        }
    });
}
function SetDistrictsByCanton(cantonId) {
    $.ajax({
        type: 'POST',
        url: "/Collaborator/GetDistrictsByCanton",
        dataType: 'json',
        data: {
            cantonId: cantonId
        },
        success: (data) => {
            clearOptions(districtSelect);
            data.forEach((x, index) => {
                let option = document.createElement('option');
                option.value = x.DistrictId;
                option.innerHTML = x.Name;
                districtSelect.appendChild(option);
            });
        },
        error: (ex) => {
        }
    });
}
function clearOptions(selectElement) {
    var i, L = selectElement.options.length - 1;
    for (i = L; i >= 0; i--) {
        selectElement.remove(i);
    }
}