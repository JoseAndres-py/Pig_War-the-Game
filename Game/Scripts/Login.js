$(document).ready(function () {
    $("#SUBMIT").click(function () { Ingresar() });
});
function Ingresar() {
    try {
        var data = {
            Nombre: $("#NombreUsuario").val(),
            Contrasena: $("#PasswordUsuario").val()
        };
        $.ajax({
            url: "/Home/Ingresar",
            data: JSON.stringify(data),
            type: "post",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.hecho == true) {
                    console.log("Su usuario esta registrado");
                    if (data.admin) { Admin(); } else {Esperar();}
                } else {
                    alert('Error: ' + data.mensaje);
                }
            }
        });
    } catch (e) {
        alert(e);
    }
}

function Esperar() {
    window.location.href = "/Game/Esperar";
}

function Admin() {
    window.location.href = "/Home/Administrador";
}