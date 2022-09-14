function deletePhoto() {
    fetch("/User/DeleteUserImage", {
        method: "POST"
    }).then(function () {
        location.reload();
    });
}
