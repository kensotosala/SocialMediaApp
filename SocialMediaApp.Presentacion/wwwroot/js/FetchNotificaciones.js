const NOTIFICATIONS_API = "http://localhost:5142/api/NotificacionesControllerAPI/ObtenerNotificaciones";
const dropdownMenu = document.getElementById('notificationDropdown');
const notificationCount = document.getElementById('notificationCount');
const markAsReadButton = document.getElementById('markAsReadButton');

let notifications = [];
let currentNotificationIndex = null;

async function loadNotifications() {
    try {
        const response = await fetch(NOTIFICATIONS_API, { method: 'GET' });
        if (!response.ok) throw new Error("Failed to fetch notifications");

        const data = await response.json();
        notifications = data || [];
        dropdownMenu.innerHTML = "";
        let unreadCount = 0;

        notifications.forEach((notification, index) => {
            const { Descripcion, Fecha, EsLeida, NotificacionId } = notification;  // Change Id to NotificacionId
            const itemClass = EsLeida ? "" : "fw-bold text-primary";
            if (!EsLeida) unreadCount++;

            const li = document.createElement('li');
            li.innerHTML = `
                <div class="d-flex align-items-center ${itemClass}">
                    <a href="#" class="dropdown-item flex-grow-1" data-index="${index}" data-bs-toggle="modal" data-bs-target="#notificationModal">
                        <small class="text-muted">${Descripcion || 'No description'}</small>
                        <br><small>${new Date(Fecha).toLocaleString()}</small>
                    </a>
                </div>
                `;
            dropdownMenu.appendChild(li);
        });

        notificationCount.textContent = unreadCount > 0 ? unreadCount : "";

        if (notifications.length === 0) {
            const emptyItem = document.createElement('li');
            emptyItem.innerHTML = '<span class="dropdown-item text-muted">No new notifications</span>';
            dropdownMenu.appendChild(emptyItem);
        }
    } catch (error) {
        console.error("Error loading notifications:", error);
        dropdownMenu.innerHTML = '<li><span class="dropdown-item text-danger">Failed to load notifications</span></li>';
    }
}

dropdownMenu.addEventListener('click', (event) => {
    const targetAnchor = event.target.closest('a[data-bs-toggle="modal"]');

    if (targetAnchor) {
        event.preventDefault();

        const index = targetAnchor.getAttribute('data-index');

        if (index !== null) {
            currentNotificationIndex = parseInt(index);
            openNotificationModal(notifications[currentNotificationIndex]);
        }
    }
});
function openNotificationModal(notification) {
    const { Descripcion, Fecha, EsLeida } = notification;

    document.getElementById('notificationDescription').textContent = Descripcion || 'No description';
    document.getElementById('notificationDate').textContent = new Date(Fecha).toLocaleString();

    markAsReadButton.disabled = EsLeida;

    const modal = new bootstrap.Modal(document.getElementById('notificationModal'));
    modal.show();
}

markAsReadButton.addEventListener('click', async () => {
    if (currentNotificationIndex === null) return;

    try {
        const notification = notifications[currentNotificationIndex];

        // Use NotificacionId instead of Id
        if (!notification || !notification.NotificacionId) {
            console.error("Notification ID is missing or invalid", notification);
            alert("Invalid notification ID");
            return;
        }

        const apiUrl = `http://localhost:5142/api/NotificacionesControllerAPI/${notification.NotificacionId}/marcar-leida`; // Change Id to NotificacionId

        const response = await fetch(apiUrl, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(`Error ${response.status}: ${errorText}`);
        }

        // Update the notification locally
        notifications[currentNotificationIndex].EsLeida = true;

        // Reload notifications to reflect changes
        await loadNotifications();

        // Close the modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('notificationModal'));
        modal.hide();
    } catch (error) {
        console.error("Error marking notification as read:", error);
        alert("No se pudo marcar la notificación como leída");
    }
});

document.addEventListener('DOMContentLoaded', loadNotifications);