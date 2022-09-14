
function deletePhoto() {
    fetch("/User/DeleteUserImage", {
        method: "POST"
    }).then(function () {
        location.reload();
    });
}

function deleteAccount() {
    document.getElementById("formDeleteAccount").submit();
}