/**
 * InventoryHub - Application JavaScript
 * Experiment 08 - ASP.NET Core MVC using ADO.NET & SQL Server
 */

document.addEventListener("DOMContentLoaded", function () {
    // Auto-dismiss alert messages after 5 seconds
    const alerts = document.querySelectorAll(".alert-dismissible");
    alerts.forEach(function (alert) {
        setTimeout(function () {
            if (window.bootstrap && bootstrap.Alert) {
                const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
                bsAlert.close();
            }
        }, 5000);
    });
});
