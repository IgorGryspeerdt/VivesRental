// Helpers used by OrderDetails.razor
window.clearSelectSelection = (el) => {
    if (!el) return;
    // when called from Blazor, el should be the DOM element
    if (el instanceof HTMLSelectElement) {
        for (let i = 0; i < el.options.length; i++) {
            el.options[i].selected = false;
        }
    }
};

window.getSelectedValues = (el) => {
    if (!el) return [];
    if (!(el instanceof HTMLSelectElement)) return [];
    return Array.from(el.selectedOptions).map(opt => opt.value);
};