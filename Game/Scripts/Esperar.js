var name = "";
var waitTimer;

$(document).ready(function () {
    Actualizar();
    waitTimer = setInterval(function () { Actualizar() }, 1000);
    $("#DISCONNECT").click(function () { Desconectar() });
});

function Actualizar() {
    try {
        var data = {
            Nombre: name
        };
        //console.log(data);
        //console.log(JSON.stringify(data));
        $.ajax({
            url: "/game/Esperar",
            data: JSON.stringify(data),
            type: "post",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.hecho == true) {
                    $('#userWaiting').html(data.registros);
                    name = data.name;
                    console.log(data.name);
                    if (name == "") { Inicio(); }
                    if (data.game == true) { Game(); } 
                    //console.log(data.name);
                    //Porque en el controlador se responde con el HTMl
                } else {
                    console.log(data);
                    alert('Error: ' + data.mensaje);
                }
            }
        });
    } catch (e) {
        alert(e);
    }
}

function Desconectar() {
    try {
        var data = {
            Nombre: name
        };
        $.ajax({
            url: "/Game/Salir",
            data: JSON.stringify(data),
            type: "post",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.hecho == true) {
                    console.log("Ha salido correctamente");
                    Inicio();
                } else {
                    alert('Error: ' + data.mensaje);
                }
            }
        });
    } catch (e) {
        alert(e);
    }
}

function Inicio() {
    clearInterval(waitTimer);
    window.location.href = "/Home/Index";
}

function Game() {
    clearInterval(waitTimer);
    window.location.href = "/Playing/Game";
}