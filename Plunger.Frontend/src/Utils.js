import currency from "currency.js";

function objIsEmpty(obj) {
    return Object.keys(obj).length === 0;
}

function objFromForm(form) {
    const formData = new FormData(form);
    return Object.fromEntries(formData.entries());
}

function dateFormat(date) {
    let dateObj = new Date(date);
    
    let options = {
        timeZone: "UTC",
        dateStyle: "short"
        // month: "2-digit",
        // day: "2-digit",
        // year: "numeric"
    };
    return dateObj.toLocaleDateString(undefined, options);
}

function timeFormat(time) {
    const [hours, minutes, seconds] = time.split(':');
    
    // let dateObj = new Date(dateTime);
    // let options = {
    //     timeZone: "UTC",
    //     dateStyle: "short",
    //     timeStyle: "short"
    // }
    //
    // return dateObj.toLocaleString(undefined, options);
}

function formatMoney(price) {
    return `$${Math.floor(Number(ci.purchasePrice) / 100)}.${Number(ci.purchasePrice) % 100}`;
}

function formatCurrency(price) {
    return currency(price, {fromCents: true}).format();
}

export {objIsEmpty, objFromForm, dateFormat, timeFormat, formatCurrency}