
    const NOTIFICATIONS_API = "http://localhost:5142/api/NotificacionesControllerAPI/ObtenerNotificaciones";

    const dropdownMenu = document.getElementById('notificationDropdown');
    const notificationCount = document.getElementById('notificationCount');

    async function loadNotifications() {
            try {
                const response = await fetch(NOTIFICATIONS_API, {method: 'GET' });
    if (!response.ok) throw new Error("Failed to fetch notifications");

    const data = await response.json();
    const notifications = data || [];

    dropdownMenu.innerHTML = "";
    let unreadCount = 0;

    if (!Array.isArray(notifications)) {
        console.error('Notifications are not in array format.');
    return;
                }

                notifications.forEach(notification => {
                    const {Descripcion, Fecha, EsLeida} = notification;
    const itemClass = EsLeida ? "" : "fw-bold text-primary";
    if (!EsLeida) unreadCount++;

    const li = document.createElement('li');
    li.innerHTML = `
    <a class="dropdown-item ${itemClass}" href="#">
        <small class="text-muted">${Descripcion || 'No description'}</small>
        <br><small>${new Date(Fecha).toLocaleString()}</small>
    </a>`;
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

    document.addEventListener('DOMContentLoaded', loadNotifications);
