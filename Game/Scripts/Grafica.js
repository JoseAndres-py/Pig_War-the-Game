window.onload = function () {
    var dataPoints = [];
    var d = new Date();
    var name = "";
    document.getElementById("date").innerHTML = d;
    var options = {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: "Jugadores por minuto conectados"
        },
        axisX: {
            valueFormatString: "DD MMM YYYY - hh mm ss",
        },
        axisY: {
            title: "Jugadores",
            titleFontSize: 24,
            includeZero: false
        },
        data: [{
            type: "line",
            xValueFormatString: "hh:mm:ss TT",
            yValueFormatString: "##",
            dataPoints: dataPoints
        }]
    };
    function addData() {
        //$.getJSON("/Home/Grafica", addData);
        $.ajax({
            url: "/Home/Grafica",
            type: "post",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                for (var i = 0; i < data.time.length; i++) {
                    dataPoints.push({
                        x: new Date(data.time[i]),
                        y: data.usersConected[i]
                    });
                }
                $("#chartContainer").CanvasJSChart(options);
            }
        });
    }

    function addUsers() {
        //$.getJSON("/Home/Grafica", addData);
        var data = {
            Nombre: name
        };
        $.ajax({
            url: "/Home/Users",
            data: JSON.stringify(data),
            type: "post",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                name = data.nombre;
                console.log(name);
                $.each(data.users, function (key, val) {
                    $("#userList").append("<li class='list-group-item'>" + val + "</li>");

                });
                $("#chartContainer").CanvasJSChart(options);
            }
        });
    }

    function refreshUser() {
        //$.getJSON("/Home/Grafica", addData);
        $.ajax({
            url: "/Home/usersWait",
            type: "post",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#userWaiting").empty();
                $.each(data.users, function (key, val) {
                    $("#userWaiting").append("<li class='list-group-item'>" + val + "</li>");

                });

                $('#userPlaying').empty();
                $.each(data.usersConnected, function (key, val) {
                    $('#userPlaying').append("<li class='list-group-item'>" + val + "</li>");

                });
                $("#chartContainer").CanvasJSChart(options);
            }
        });
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
        clearInterval(waitUsers);
        window.location.href = "/Home/Index";
    }

    var d = new Date();
    addData();
    addUsers();
    
    // Refresca la grafica de usuarios cada minuto
    var waitTimer = setInterval(function () { addData() }, 100000)

    // Refresca la grafica de usuarios cada minuto
    var waitUsers = setInterval(function () { refreshUser() }, 1300)
    $("#DISCONNECT").click(function () { Desconectar() });
}