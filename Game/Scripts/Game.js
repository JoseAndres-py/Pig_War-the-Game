var heroe = {
    left: 75.0,
    top: 50.0,
    lives: 1
}

var limit = {
    up: 19.0,
    down: 80.0
};
var shot = false, disparoJugador = false, step = 0.5, move = 0;
var id = 1;
var gametimer;

$(document).ready(function () {
    drawHero();
    document.onkeydown = function (e) {
        $(document).keydown(function (event) {
            move = event.which;
            console.log(event.which);
            if (event.which === 32) {
                // shot
                shot = true;
            }
        });

    }
    gametimer = setInterval('gameLoop()', 90);
    setInterval('shoter()', 2000);

});

function drawHero() {
    $("#heroes").html(`<div class='hero${id}' style='left:${heroe.left}%; top:${heroe.top}%'></div>`);
}

function moveHero() {
    if (move === 38) {
        // up
        heroe.top -= step;
        if (heroe.top < limit.up) {
            heroe.top += step;
        }
    }

    else if (move === 40) {
        // down
        heroe.top += step;
        if (heroe.top > limit.down) {
            heroe.top -= step;
        }
    }
    move = 0;
}
function shoter() {
    if (shot) {
        disparoJugador = true;
        shot = false;
    }
}

function gameLoop() {
    moveHero();
    drawHero();
    Actualizar();
}


function Actualizar() {
    try {
        var data = {
            x: heroe.left,
            y: heroe.top,
            fire: disparoJugador
        };
        if (disparoJugador) {disparoJugador = false;console.log("Disparoooo");}
        //console.log(data);
        //console.log(JSON.stringify(data));
        $.ajax({
            url: "/Playing/Game",
            data: JSON.stringify(data),
            type: "post",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.hecho == true) {
                    $('#draw').html("");
                    $('#draw').html(data.registros);
                    if (data.mensaje == "Loser") { Inicio(); }
                    //console.log(data.registros);
                           if (data.idGame == 2) { heroe.left = 0 };
                    //console.log(data.name);
                    //Porque en el controlador se responde con el HTMl
                } else {
                    console.log(data);
                    if (data.idGame == 0) { Inicio(); }
                }
            }
        });
    } catch (e) {
        alert(e);
    }
}

function Inicio() {
    clearInterval(gametimer);
    window.location.href = "/Home/Index";
}





