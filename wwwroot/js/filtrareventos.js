// Función para abrir el modal de fechas
function abrirModal() {
    document.getElementById("modalFecha").style.display = "flex";

    // Inicializar Flatpickr para ambos campos de fecha
    flatpickr("#fechaInicio", {
        inline: true,  // Hace que el calendario siempre esté visible
        allowInput: true  // Permite ingresar texto además de seleccionar la fecha
    });

    flatpickr("#fechaFin", {
        inline: true,  // Hace que el calendario siempre esté visible
        allowInput: true  // Permite ingresar texto además de seleccionar la fecha
    });
}

// Función para cerrar el modal de fechas
function cerrarModal() {
    document.getElementById("modalFecha").style.display = "none";
}

// Función para aplicar las fechas seleccionadas
function aplicarFechas() {
    let inicio = document.getElementById("fechaInicio").value;
    let fin = document.getElementById("fechaFin").value;

    if (inicio && fin) {
        const fechaInicio = new Date(inicio);
        const fechaFin = new Date(fin);

        if (fechaInicio > fechaFin) {
            alert("La fecha de inicio no puede ser mayor que la fecha de fin.");
            return;
        }

        document.getElementById("fechaInput").value = `${inicio} - ${fin}`;
        cerrarModal();
    } else {
        alert("Seleccione ambas fechas.");
    }
}

// Función para realizar la búsqueda con los filtros
function realizarBusqueda() {
    console.log("Función realizarBusqueda ejecutada");

    // Captura los valores de los filtros
    const categoria = document.getElementById('filtroCategoria').value;
    const lugar = document.getElementById('filtroLugar').value;
    const precio = document.getElementById('filtroPrecio').value;
    const fechaInicio = document.getElementById('fechaInicio').value;
    const fechaFin = document.getElementById('fechaFin').value;

    // Validar y ajustar los valores
    const filtros = {
        categoria: categoria === "Categoría" ? null : parseInt(categoria), // Convertir a número o NULL
        lugar: lugar === "Lugar" ? null : lugar, // NULL si no se selecciona un lugar
        precio: precio === "Precio" ? null : precio, // NULL si no se selecciona un precio
        fechaInicio: fechaInicio || null, // NULL si no se selecciona una fecha
        fechaFin: fechaFin || null // NULL si no se selecciona una fecha
    };

    console.log("Filtros enviados:", filtros);

    // Envía la solicitud al servidor usando fetch
    fetch('/EventosCliente/Filtrar', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(filtros)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error en la solicitud: ' + response.statusText);
            }
            return response.json();
        })
        .then(data => {
            console.log("Respuesta del servidor:", data);
            actualizarEventos(data);
        })
        .catch(error => {
            console.error('Error al buscar eventos:', error);
            alert("Ocurrió un error al buscar eventos. Por favor, inténtelo de nuevo.");
        });
}

// Función para actualizar la sección de eventos con los resultados
function actualizarEventos(data) {
    const eventosGrid = document.querySelector('.eventos-grid');
    eventosGrid.innerHTML = '';

    if (data.length === 0) {
        eventosGrid.innerHTML = `
            <div class="no-eventos">
                <i class="fas fa-calendar-times fa-3x"></i>
                <p>No se encontraron elementos.</p>
            </div>
        `;
        return;
    }

    data.forEach(evento => {
        // Verificar que evento.eventoID esté definido
        if (!evento.eventoID) {
            console.error("eventoID no está definido para el evento:", evento);
            return;
        }

        // Verificar que evento.imagenUrl esté definido
        const imagenUrl = evento.imagenUrl || "/img/imagen_predeterminada.jpg";

        const eventoHTML = `
            <div class="evento">
                <img src="${imagenUrl}" alt="${evento.titulo}" />
                <p class="titulo">${evento.titulo}</p>
                <p class="descripcion">${evento.descripcion}</p>
                <a href="/EventosCliente/ObtenerEventoid?eventoId=${evento.eventoID}" class="btn-ver-evento">VER EVENTO</a>
            </div>
        `;
        eventosGrid.insertAdjacentHTML('beforeend', eventoHTML);
    });
}//termina el actualizar el html

// Asignar el evento al botón de búsqueda
document.addEventListener('DOMContentLoaded', function () {
    document.querySelector('.buscar-filtros').addEventListener('click', realizarBusqueda);
});

//funcion para reestablecer los filtros 
function reestablecerFiltros() {
    // Restablecer el filtro de categoría
    document.getElementById('filtroCategoria').selectedIndex = 0;

    // Restablecer el filtro de lugar
    document.getElementById('filtroLugar').selectedIndex = 0;

    // Restablecer el filtro de precio
    document.getElementById('filtroPrecio').selectedIndex = 0;

    // Restablecer el campo de fecha
    document.getElementById('fechaInput').value = '';
    document.getElementById('fechaInicio').value = '';
    document.getElementById('fechaFin').value = '';

    //Recargar la lista de eventos sin filtros
    realizarBusqueda(); // Llama a la función que realiza la búsqueda sin filtros
}

// Asignar el evento al botón "Reestablecer"
document.addEventListener('DOMContentLoaded', function () {
    document.querySelector('.reestablecer-filtros').addEventListener('click', reestablecerFiltros);
});
