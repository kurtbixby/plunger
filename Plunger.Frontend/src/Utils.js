function objIsEmpty(obj) {
    return Object.keys(obj).length === 0;
}

function objFromForm(form) {
    const formData = new FormData(form);
    return Object.fromEntries(formData.entries());
}

export {objIsEmpty, objFromForm}