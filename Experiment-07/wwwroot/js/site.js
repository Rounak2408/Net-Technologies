/**
 * FeedbackHub - Application JavaScript
 * Experiment 07 - ASP.NET Core MVC
 */

document.addEventListener("DOMContentLoaded", function () {
    initStarRatingWidget();
    initCharCounter();
    initAlertAutoDismiss();
});

/**
 * Initializes the interactive star rating widget on the feedback submission form
 */
function initStarRatingWidget() {
    const starWidget = document.getElementById("starRatingWidget");
    const ratingInput = document.getElementById("ratingValue");
    const ratingLabel = document.getElementById("ratingLabel");

    if (!starWidget || !ratingInput) return;

    const stars = starWidget.querySelectorAll(".star-item");
    const ratingTexts = {
        1: "1 Star - Poor",
        2: "2 Stars - Fair",
        3: "3 Stars - Good",
        4: "4 Stars - Very Good",
        5: "5 Stars - Outstanding"
    };

    function updateStarsDisplay(val) {
        val = parseInt(val, 10) || 0;
        stars.forEach(star => {
            const starVal = parseInt(star.getAttribute("data-value"), 10);
            if (starVal <= val) {
                star.classList.add("active");
            } else {
                star.classList.remove("active");
            }
        });

        if (ratingLabel) {
            ratingLabel.textContent = ratingTexts[val] || (val + " Stars");
        }
    }

    // Set initial display
    const initialVal = parseInt(ratingInput.value, 10) || 5;
    ratingInput.value = initialVal;
    updateStarsDisplay(initialVal);

    // Hover event preview
    stars.forEach(star => {
        star.addEventListener("mouseenter", function () {
            const hoverVal = parseInt(this.getAttribute("data-value"), 10);
            stars.forEach(s => {
                const sVal = parseInt(s.getAttribute("data-value"), 10);
                if (sVal <= hoverVal) {
                    s.classList.add("hovered");
                } else {
                    s.classList.remove("hovered");
                }
            });
            if (ratingLabel) {
                ratingLabel.textContent = ratingTexts[hoverVal] || (hoverVal + " Stars");
            }
        });

        // Click event to select rating
        star.addEventListener("click", function () {
            const chosenVal = parseInt(this.getAttribute("data-value"), 10);
            ratingInput.value = chosenVal;
            updateStarsDisplay(chosenVal);

            // Trigger jQuery unobtrusive validation if active
            if (window.jQuery && jQuery(ratingInput).valid) {
                jQuery(ratingInput).valid();
            }
        });
    });

    // Mouse leave: reset to current selected value
    starWidget.addEventListener("mouseleave", function () {
        stars.forEach(s => s.classList.remove("hovered"));
        updateStarsDisplay(ratingInput.value);
    });

    // Reset button handler
    const resetBtn = document.getElementById("resetBtn");
    if (resetBtn) {
        resetBtn.addEventListener("click", function () {
            setTimeout(function () {
                ratingInput.value = 5;
                updateStarsDisplay(5);
                const charCounter = document.getElementById("charCounter");
                if (charCounter) {
                    charCounter.textContent = "0 characters (min 10)";
                }
            }, 50);
        });
    }
}

/**
 * Character counter for feedback textarea
 */
function initCharCounter() {
    const textarea = document.getElementById("feedbackMessage");
    const counter = document.getElementById("charCounter");

    if (!textarea || !counter) return;

    function updateCount() {
        const len = textarea.value.length;
        if (len < 10) {
            counter.innerHTML = `<span class="text-danger fw-semibold">${len} characters (needs ${10 - len} more)</span>`;
        } else {
            counter.innerHTML = `<span class="text-success fw-semibold">${len} characters</span>`;
        }
    }

    textarea.addEventListener("input", updateCount);
    // Initial check
    updateCount();
}

/**
 * Auto-dismiss flash alerts after 6 seconds
 */
function initAlertAutoDismiss() {
    const alerts = document.querySelectorAll(".alert-dismissible");
    alerts.forEach(alert => {
        setTimeout(() => {
            if (window.bootstrap && bootstrap.Alert) {
                const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
                bsAlert.close();
            }
        }, 6000);
    });
}
