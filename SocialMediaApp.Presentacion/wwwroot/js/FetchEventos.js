const EVENTOS_API = "http://localhost:5142/api/EventoControllerAPI/obtenerTodosLosEventos";

/**
 * cargarEventos(): Se utiliza para cargar los eventos dentro en el dropdown.
 */
async function cargarEventos() {
    const dropdownMenu = document.getElementById("eventosDropdown");

    // Asegúrate de agregar el botón "Nuevo Evento" primero
    if (!document.getElementById("nuevoEventoButton")) {
        const nuevoEventoButton = document.createElement('li');
        nuevoEventoButton.innerHTML = `
            <button type="button" class="btn btn-lg btn-primary w-100" id="nuevoEventoButton">
                Nuevo Evento
            </button>
        `;
        dropdownMenu.appendChild(nuevoEventoButton);

        // Lógica para el evento del botón "Nuevo Evento"
        nuevoEventoButton.addEventListener("click", () => {
            console.log("Nuevo Evento clicked");
            // Aquí puedes abrir un modal o realizar alguna acción
        });
    }

    try {
        const response = await fetch(EVENTOS_API, { method: 'GET' });
        if (!response.ok) throw new Error("Error al hacer fetch de eventos");

        const data = await response.json();
        const eventos = data || []; // Asegura que eventos esté inicializado

        // Limpia todos los eventos excepto el botón "Nuevo Evento"
        dropdownMenu.innerHTML = dropdownMenu.firstElementChild.outerHTML;

        // Agrega eventos al dropdown
        if (eventos.length === 0) {
            const emptyItem = document.createElement('li');
            emptyItem.innerHTML = `
                <span class="dropdown-item text-muted">No hay eventos disponibles</span>
            `;
            dropdownMenu.appendChild(emptyItem);
        } else {
            eventos.forEach((evento, index) => {
                const { Titulo, Descripcion, FechaEvento, Ubicacion, EsLeida } = evento;

                const itemClass = EsLeida ? "" : "fw-bold text-primary";

                const li = document.createElement('li');
                li.innerHTML = `
                    <a href="#" class="dropdown-item ${itemClass}" data-index="${index}" data-bs-toggle="modal" data-bs-target="#notificationModal">
                        <strong>${Titulo}</strong>
                        <button type="button" class="btn btn-sm btn-primary ms-2">✏</button>
                        <p class="mb-0 text-muted">${Descripcion || 'No description'}</p>
                        <small class="text-muted">${Ubicacion}</small>
                        <br><small>${new Date(FechaEvento).toLocaleString()}</small>
                    </a>
                `;
                dropdownMenu.appendChild(li);
            });
        }
    } catch (error) {
        console.error("Error al cargar eventos", error);
        dropdownMenu.innerHTML = `
            ${dropdownMenu.firstElementChild.outerHTML}
            <li><span class="dropdown-item text-danger">Error al cargar los eventos</span></li>
        `;
    }
}

// Ejecutar la función al cargar la página
document.addEventListener("DOMContentLoaded", cargarEventos);
async function cargarEventos() {
    const dropdownMenu = document.getElementById("eventosDropdown");

    // Asegúrate de agregar el botón "Nuevo Evento" primero
    if (!document.getElementById("nuevoEventoButton")) {
        const nuevoEventoButton = document.createElement('li');
        nuevoEventoButton.innerHTML = `
            <button type="button" class="btn btn-lg btn-primary w-100" data-bs-toggle="modal" data-bs-target="#eventFormModal" id="nuevoEventoButton">Nuevo Evento</button>`;
        dropdownMenu.appendChild(nuevoEventoButton);

        // Lógica para el evento del botón "Nuevo Evento"
        nuevoEventoButton.addEventListener("click", () => {
            console.log("Nuevo Evento clicked");
            // Aquí puedes abrir un modal o realizar alguna acción
        });
    }

    try {
        const response = await fetch(EVENTOS_API, { method: 'GET' });
        if (!response.ok) throw new Error("Error al hacer fetch de eventos");

        const data = await response.json();
        const eventos = data || []; // Asegura que eventos esté inicializado

        // Limpia todos los eventos excepto el botón "Nuevo Evento"
        dropdownMenu.innerHTML = dropdownMenu.firstElementChild.outerHTML;

        // Agrega eventos al dropdown
        if (eventos.length === 0) {
            const emptyItem = document.createElement('li');
            emptyItem.innerHTML = `
                <span class="dropdown-item text-muted">No hay eventos disponibles</span>
            `;
            dropdownMenu.appendChild(emptyItem);
        } else {
            eventos.forEach((evento, index) => {
                const { Titulo, Descripcion, FechaEvento, Ubicacion, EsLeida } = evento;

                const itemClass = EsLeida ? "" : "fw-bold text-primary";

                const li = document.createElement('li');
                li.innerHTML = `
                    <a href="#" class="dropdown-item ${itemClass}" data-index="${index}" data-bs-toggle="modal" data-bs-target="#notificationModal">
                        <strong>${Titulo}</strong>
                        <button type="button" class="btn btn-sm btn-primary ms-2">✏</button>
                        <p class="mb-0 text-muted">${Descripcion || 'No description'}</p>
                        <small class="text-muted">${Ubicacion}</small>
                        <br><small>${new Date(FechaEvento).toLocaleString()}</small>
                    </a>
                `;
                dropdownMenu.appendChild(li);
            });
        }
    } catch (error) {
        console.error("Error al cargar eventos", error);
        dropdownMenu.innerHTML = `
            ${dropdownMenu.firstElementChild.outerHTML}
            <li><span class="dropdown-item text-danger">Error al cargar los eventos</span></li>
        `;
    }
}

document.addEventListener('DOMContentLoaded', function () {
    // Asigna el formulario a una variable
    const form = document.getElementById('eventForm');

    if (form) {
        // Agrega un listener al evento de submit
        form.addEventListener('submit', function (event) {
            event.preventDefault(); // Evita el envío normal del formulario

            // Captura los valores del formulario
            const formData = new FormData(form);

            // Obtener los usuarios seleccionados
            const usuariosInvitar = formData.getAll('usuariosInvitar[]');
            console.log('Usuarios seleccionados para invitar:', usuariosInvitar);

            // Crear el objeto de datos a enviar
            const dataToSend = {
                Titulo: formData.get('Titulo'),
                Descripcion: formData.get('Descripcion'),
                FechaEvento: formData.get('FechaEvento'),
                Ubicacion: formData.get('Ubicacion'),
                usuariosInvitar: usuariosInvitar
            };

            // Enviar la solicitud POST al backend
            fetch('http://localhost:5142/api/EventoControllerAPI/crear', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json' // Asegurarse de que el contenido se envíe en formato JSON
                },
                body: JSON.stringify(dataToSend) // Convierte los datos a formato JSON
            })
                .then(response => response.json())
                .then(data => {
                    console.log('Evento creado con éxito:', data);
                    alert('Evento creado exitosamente!');
                    // Aquí puedes cerrar el modal si lo deseas
                    $('#eventFormModal').modal('hide');
                })
                .catch(error => {
                    console.error('Error al crear el evento:', error);
                    alert('Hubo un error al crear el evento.');
                });
        });
    } else {
        console.error('Formulario no encontrado');
    }
});


// Ejecutar la función al cargar la página
document.addEventListener("DOMContentLoaded", cargarEventos);

// Event listener for "Nuevo Evento" button
document.addEventListener('DOMContentLoaded', () => {
    const nuevoEventoButton = document.getElementById("nuevoEventoButton");
    if (nuevoEventoButton) {
        nuevoEventoButton.addEventListener("click", () => {
            // Lógica para crear un nuevo evento
            console.log("Nuevo Evento clicked");
        });
    }
});

// Call the function when the DOM is loaded
document.addEventListener('DOMContentLoaded', cargarEventos);