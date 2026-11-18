/**
 * PlacementHub - Application JavaScript
 * Exercise 9 – ASP.NET Web Forms using Data Controls
 */

document.addEventListener("DOMContentLoaded", function () {
    // Auto-dismiss alerts after 5 seconds
    var alerts = document.querySelectorAll(".alert-dismissible");
    alerts.forEach(function (alert) {
        setTimeout(function () {
            if (window.bootstrap && bootstrap.Alert) {
                var bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
                bsAlert.close();
            }
        }, 5000);
    });
});
