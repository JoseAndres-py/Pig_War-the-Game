function ShowEdit(contactoId, personaId, tipoContactoId, valor) {
    DetalleContacto.hidden = false;
    try {
        ContactoId.value = contactoId;
        PersonaId.value = personaId;
        TipoContactoId.value = tipoContactoId;
        Valor.value = valor;
    } catch (e) {
        alert(e);
        DetalleContacto.hidden = true;
    }
}
function EditContacto() {
    try {
        var data = {
            ContactoId: ContactoId.value,
            PersonaId: PersonaId.value,
            TipoContactoId: TipoContactoId.value,
            Valor: Valor.value
        };
        console.log(data);
        console.log(JSON.stringify(data));
        $.ajax({
            url: ContactoId.value == 0 ? "/Contacto/Create":"/Contacto/Set",
            data: JSON.stringify(data),
            type:"post",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.hecho == true) {
                    console.log(data);
                    var a = document.getElementsByTagName('tbody')[0];
                    //Porque en el controlador se responde con el HTMl
                    a.innerHTML = data.registros;
                } else {
                    console.log(data);
                    alert('Error: ' + data.mensaje);
                }
            }
        });
    } catch (e) {
        alert(e);
    }
    DetalleContacto.hidden = true;
}

function Borrar() {
    try {
        var data = {
            ContactoId: ContactoId.value,
            PersonaId: PersonaId.value
        };
        console.log(data);
        console.log(JSON.stringify(data));
        $.ajax({
            url: "/Contacto/Delete",
            data: JSON.stringify(data),
            type: "post",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.hecho == true) {
                    console.log(data.registros);
                    var tabla = document.getElementsByTagName('table')[0];
                    while (tabla.rows.length > 1) {
                        tabla.deleteRow(1);
                    }
                    for (var i = 0; i < data.registros.length; i++) {
                        var r = tabla.insertRow();
                        r.insertCell(0).innerText = data.registros[i].DescripcionTipoContacto;
                        r.insertCell(1).innerText = data.registros[i].Valor;
                        r.style.cursor = 'pointer';
                        r.addEventListener('click', function (
                            ContactoId=data.registros[i].ContactoId,
                            PersonaId=data.registros[i].PersonaId,
                            TipoContactoId=data.registros[i].TipoContactoId,
                            Valor=data.registros[i].Valor) {
                            ShowEdit(
                                ContactoId,
                                PersonaId,
                                TipoContactoId,
                                Valor,
                            );
                        });
                    }
                    alert('Eliminado');
                    DetalleContacto.hidden = true;
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